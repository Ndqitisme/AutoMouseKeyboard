# Modern UI Redesign — AutoMouseKeyboard (Design Spec)

**Date:** 2026-09-11
**Status:** Approved by user (2026-09-11)
**Scope:** All forms — MainForm, SettingForm, AboutForm, KeyActionForm, MouseActionForm, plus all dropdown menus and tray menu.

## 1. Problem statement

The app is a .NET 6 WinForms tool. Its UI looks dated ("ancient C# look") and its layout breaks when the window is resized:

- `grpConfigs` (sidebar GroupBox) anchors Top|Bottom|Left but its child buttons (`btnNew`, `btnSave`, `btnDelete`, `btnSetting`, `btnAbout`, `btnImport`, `btnExport`) sit at fixed Y coordinates (470–590) with **no bottom anchor** — when the form grows, buttons detach from the bottom and float mid-panel; when it shrinks below design height they clip.
- `grpActions` uses mixed anchoring plus a manual `CenterActionButtons()` repositioning hack; `AdjustLabelSizes()` manually re-measures labels on every resize/language change — fragile.
- All chrome is default WinForms: square GroupBox frames, flat gray buttons, native ListBox/DataGridView/ToolStrip look, no hover feedback.
- Pre-existing defect blocking verification: 8 resource files (`LanguageResources.{bn,hi,ja,ko,ru,vi,zh,zh-TW}.resx`) contain mojibake (`?`/U+FFFD) and corrupted `</value>` tags → `dotnet build` fails with MSB3103. Must be repaired first or nothing compiles.

## 2. Goals

- Modern, soft-rounded UI in the spirit of the VSCode dark theme reference: rounded cards, accent-colored primary actions, subtle borders, smooth hover transitions.
- Resize/zoom must never break layout: every control flows via `TableLayoutPanel`/`FlowLayoutPanel`/Dock — no manual coordinate math.
- Keep all 11 existing themes; default theme follows Windows (read `AppsUseLightTheme` from `HKCU\...\Themes\Personalize`).
- Keep all existing behavior: 19-language localization, config CRUD/drag-reorder/import-export, key/mouse action menus, grid editing, tray icon, Esc-to-stop, etc.
- No third-party UI dependencies. Pure GDI+ custom-drawn controls.

## 3. Architecture

New namespace `AutoMouseKeyboard.UI` (folder `UI/`) holding the design system; forms consume it.

### 3.1 Theme system

`UI/ThemePalette.cs` — immutable token set per `ThemeMode`:

```
WindowBack, CardBack, InputBack, Border, Text, TextMuted,
Accent, AccentHover, AccentPressed, AccentText (contrast),
HoverBack, SelectionBack, SelectionText, GridLine,
MenuBack, MenuHover, MenuText
```

- `ThemePalette.Get(ThemeMode)` returns the palette. Light and the 9 color themes keep their tinted window background but get proper card/border/hover/selection tokens derived per-theme (hand-tuned table, not `+50` math). Dark uses the VSCode-like set: window `#1E1E1E`, card `#252526`, input `#3C3C3C`, border `#3C3C3C`, text `#CCCCCC`, muted `#9D9D9D`, accent `#0E639C`, accentHover `#1177BB`, accentPressed `#0B5A8E`, selection `#094771`, menu `#1B1B1C`/`#2A2D2E` hover.
- `ThemeManager` refactor: `public static ThemePalette Palette => ThemePalette.Get(_currentTheme);` `ApplyTheme` walks controls; controls implementing `IThemedControl { void ApplyTheme(ThemePalette); }` get called directly (custom controls), standard controls keep getting colored recursively as today.
- `UI/WindowsThemeDetector.cs`: `Detect()` → reads registry `HKCU\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize` value `AppsUseLightTheme` (DWORD; missing ⇒ Light). `AppSettings.Theme` getter returns `settings.Theme ?? WindowsThemeDetector.Detect()` so first-run follows the OS.

### 3.2 Custom controls (`UI/Controls/`)

All double-buffered, `SmoothingMode.AntiAlias`, theme via `IThemedControl`, 6px corner radius on controls, 10px on cards:

| Control | Replaces | Notes |
|---|---|---|
| `CardPanel` | GroupBox | Rounded card (r=10), optional `Title` text drawn top-left, child controls clipped inside |
| `ModernButton` | Button | r=6, `StyleKind {Primary, Secondary}`; hover/press color transitions ~120ms via `Timer` color-lerp; Disabled state muted |
| `RoundedTextBox` | TextBox | rounded border + borderless inner TextBox; accent border on focus |
| `ModernCheckBox` | CheckBox | 16px rounded box (r=4) + drawn check glyph |
| `ModernSelect` | ComboBox (DropDownList) | rounded face + drawn chevron; opens `SelectDropDown` — a borderless topmost Form hosting an owner-drawn scrollable list, closes on outside-click/Esc. Supports `Items` (object), `DisplayMember`, `SelectedIndex`, `SelectedItem`, `SelectedIndexChanged` |
| `ModernNumericUpDown` | NumericUpDown | rounded container, borderless numeric text box, drawn ▲▼ chevrons; `Value/Minimum/Maximum/Increment/ValueChanged` API-compatible |
| `ModernListBox` | ListBox | `DrawMode.OwnerDrawFixed`, rounded selection pill (r=4), hover row highlight; keeps `DataSource/DisplayMember/SelectedItems/MultiExtended` |
| `ModernToolStripRenderer` | default renderer | `ToolStripProfessionalRenderer` subclass: rounded hover rects, palette colors, themed dropdown border; used for the action menus and the tray `ContextMenuStrip` |
| `StyledDataGridView` | (extends `EnterAwareDataGridView`) | Borderless, `EnableHeadersVisualStyles=false`, flat themed header + 1px bottom separator, cell padding, themed selection; grid keeps native scrollbars |

Menu note: `menuAddKey`/`menuAddMouse` `ToolStripDropDownButton`s become `ModernButton`s; clicking opens a `ContextMenuStrip` (rendered by `ModernToolStripRenderer`) whose section items still lazily open `MultiColumnMenuStrip` (also re-themed, rounded hover cells).

### 3.3 Layout rebuild — MainForm.Designer.cs

Single root `TableLayoutPanel` (`Dock=Fill`, 10px padding):

```
┌────────────────────────────┬──────────────────────────────────┐
│ sidebarCard (Abs 288px)    │ contentCard (Percent 100)        │
│ ┌────────────────────────┐ │ ┌─name row: label + textbox(fill)┐│
│ │ lstConfigs (fill)      │ │ ├─toolbar: [Add Key] [Add Mouse] ┤│
│ │                        │ │ ├─gridActions (fill)            ┤│
│ │                        │ │ ├─FlowLayout centered:          ┤│
│ │                        │ │ │  [MoveUp][MoveDown][Delete]   ┤│
│ ├─[      New       ]     │ │ └────────────────────────────────┘│
│ ├─[ Save ][ Delete ]     │ │                                  │
│ ├─[Setting][ About ]     │ │                                  │
│ └─[Import ][ Export ]    │ │                                  │
├────────────────────────────┴──────────────────────────────────┤
│ statusCard (spans 2 cols, AutoSize):                          │
│  lblStatus(fill,ellipsis) | Loop+num | Delay+num | [ Start ]  │
└───────────────────────────────────────────────────────────────┘
```

- Sidebar inner: TLP — row0 list (Percent 100), then 4 fixed rows for the button grid (2-col TLP; `New` spans both columns). Buttons dock Fill — they ride with the card, can never detach.
- `CenterActionButtons()` and `AdjustLabelSizes()` are deleted; `grpActions.Resize`/`MainForm_Resize` handlers for them go away. `AdjustColumnWidths()` stays (grid header text measure).
- `MinimumSize` ≈ 1000×640 stays. Font: `Segoe UI` 9pt everywhere (designer files currently mix `Microsoft Sans Serif`).

### 3.4 Dialogs

All keep `FormBorderStyle.FixedDialog`, `StartPosition.CenterParent`, but rebuilt with TLP + new controls:

- **SettingForm**: two `CardPanel`s (Appearance: Theme + Language `ModernSelect`; Behavior: two `ModernCheckBox`es) + `btnOk` ModernButton. Replaces `AdjustComboBoxSizes` hacks with a 2-col TLP (label | fill). `cboTheme`/`cboLanguage` index semantics preserved (index = enum value).
- **AboutForm**: one card — app name (14pt bold), version/author muted, description, donate block + `picQRCode` right, OK bottom-right.
- **KeyActionForm / MouseActionForm**: same row layout (label | control) via TLP; `cboKey` (≈70 items) becomes `ModernSelect` with scrolling dropdown; `btnGetPosition` → ModernButton.

### 3.5 Localization repair (prerequisite)

`LanguageResources.{bn,hi,ja,ko,ru,vi,zh,zh-TW}.resx` are corrupted: non-ASCII text became `?`/U+FFFD and several `</value>` tags lost their `<`. Fix = regenerate each file's `<value>` content by translating the neutral `LanguageResources.resx` key set, restoring well-formed XML. English text, `{0}` placeholders, `&#x0D;&#x0A;` newlines and `KeyName_*` technical names (Enter, Space, Num 0…) are preserved verbatim.

## 4. Interaction & motion

- Buttons: 120ms color lerp on hover/press (Timer-driven, ~8 steps); no other gratuitous animation.
- Dropdowns/menus: appear instantly (native behavior), rounded hover pills.
- Focus: inputs show accent border; buttons show focus via subtle border.
- No custom form title bar (keeps native chrome — consistent with "keep structure").

## 5. Error handling / edge cases

- `ModernNumericUpDown`: invalid text on leave → revert to last valid `Value`; clamp to Min/Max.
- `ModernSelect` dropdown must close on: item pick, Esc, form deactivate, owner move/resize.
- Theme change while dropdown open → close and repaint.
- All custom painting guards `IsDisposed`/`DesignMode`; timers disposed with control.
- `ThemeManager.ApplyTheme` must handle the new control types **and** still paint legacy controls inside cards (labels inside `CardPanel` get card bg).
- Language switch at runtime re-measures nothing manually — TLP `AutoSize` columns reflow.

## 6. Out of scope

- Automation engine, `InputDispatcher`, `ConfigStorage`, models, services — untouched.
- Custom scrollbars, custom title bar, WPF/WinUI migration.
- New features, new resx keys (reuse existing keys only).

## 7. Verification

- `dotnet build` must exit 0 (currently fails — resx repair task unblocks).
- No unit test project exists; verification = clean build + manual smoke (resize window min→max, switch theme/language, open each dialog, run a config).
- Layout invariant to verify: at MinimumSize and maximized, all controls remain inside their cards, nothing overlaps or detaches.
