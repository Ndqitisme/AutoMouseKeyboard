
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using AutoMouseKeyboard.UI;

namespace AutoMouseKeyboard.Utilities
{
    public enum ThemeMode
    {
    Light,
    Dark,
    Cyan,
    Pink,
    Green,
    Red,
    Orange,
    Yellow,
    Blue,
    Indigo,
    Violet
}

public static class ThemeManager
{
    private static ThemeMode _currentTheme = ThemeMode.Light;
    private static readonly List<Form> RegisteredForms = new List<Form>();

    public static ThemeMode CurrentTheme => _currentTheme;

    /// <summary>The token palette for the currently active theme.</summary>
    public static ThemePalette Palette => ThemePalette.Get(_currentTheme);

    /// <summary>
    /// Tag value that marks a child control whose colors are owned by its themed
    /// parent (e.g. RoundedTextBox's inner TextBox); the recursion skips it.
    /// </summary>
    internal const string SkipThemeTag = "SkipTheme";

    public static event EventHandler<ThemeMode>? ThemeChanged;

    static ThemeManager()
    {
        LoadTheme();
    }

    public static void RegisterForm(Form form)
    {
        if (!RegisteredForms.Contains(form))
        {
            RegisteredForms.Add(form);
            ApplyTheme(form, _currentTheme);
        }
    }

    public static void UnregisterForm(Form form)
    {
        RegisteredForms.Remove(form);
    }

    public static void SetTheme(ThemeMode theme, bool save = true)
    {
        if (_currentTheme == theme)
        {
            return;
        }

        _currentTheme = theme;
        foreach (var form in RegisteredForms.ToList())
        {
            if (form != null && !form.IsDisposed)
            {
                ApplyTheme(form, theme);
            }
        }

        if (save)
        {
            SaveTheme(theme);
        }

        ThemeChanged?.Invoke(null, theme);
    }

    private static void LoadTheme()
    {
        try
        {
            var theme = AppSettings.Theme;
            SetTheme(theme, false);
            
            var baseFolder = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "AutoMouseKeyboard");
            var oldThemeFile = System.IO.Path.Combine(baseFolder, "theme.txt");
            if (System.IO.File.Exists(oldThemeFile))
            {
                try
                {
                    var themeStr = System.IO.File.ReadAllText(oldThemeFile).Trim();
                    if (Enum.TryParse<ThemeMode>(themeStr, out var oldTheme))
                    {
                        AppSettings.Theme = oldTheme;
                        SetTheme(oldTheme, false);
                        System.IO.File.Delete(oldThemeFile);
                    }
                }
                catch
                {
                }
            }
        }
        catch
        {
        }
    }

    private static void SaveTheme(ThemeMode theme)
    {
        try
        {
            AppSettings.Theme = theme;
        }
        catch
        {
        }
    }

    public static void ApplyTheme(Form form, ThemeMode theme)
    {
        if (form == null || form.IsDisposed)
        {
            return;
        }

        var palette = ThemePalette.Get(theme);
        form.BackColor = palette.WindowBack;
        form.ForeColor = palette.Text;

        ApplyThemeToControls(form.Controls, palette);
    }

    /// <summary>
    /// Surface color for a control that paints a container-like background:
    /// <see cref="ThemePalette.CardBack"/> when it sits directly inside a
    /// themed container (e.g. CardPanel), otherwise <see cref="ThemePalette.WindowBack"/>.
    /// </summary>
    private static Color SurfaceBack(Control control, ThemePalette palette)
    {
        return control.Parent is IThemedControl ? palette.CardBack : palette.WindowBack;
    }

    private static void ApplyThemeToControls(Control.ControlCollection controls, ThemePalette palette)
    {
        foreach (Control control in controls)
        {
            if (control == null || control.Tag is string tag && tag == SkipThemeTag)
            {
                continue;
            }

            if (control is IThemedControl themed)
            {
                themed.ApplyTheme(palette);
            }
            else if (control is DataGridView grid)
            {
                ApplyThemeToDataGridView(grid, palette);
            }
            else if (control is GroupBox groupBox)
            {
                groupBox.BackColor = SurfaceBack(groupBox, palette);
                groupBox.ForeColor = palette.Text;
            }
            else if (control is Panel panel)
            {
                panel.BackColor = SurfaceBack(panel, palette);
                panel.ForeColor = palette.Text;
            }
            else if (control is ToolStrip toolStrip)
            {
                toolStrip.BackColor = palette.MenuBack;
                toolStrip.ForeColor = palette.MenuText;
            }
            else if (control is Label label)
            {
                if (label.BackColor != Color.Transparent)
                {
                    label.BackColor = SurfaceBack(label, palette);
                }
                label.ForeColor = palette.Text;
            }
            else if (control is TextBox textBox)
            {
                textBox.BackColor = palette.InputBack;
                textBox.ForeColor = palette.Text;
            }
            else if (control is ComboBox comboBox)
            {
                comboBox.BackColor = palette.InputBack;
                comboBox.ForeColor = palette.Text;
            }
            else if (control is NumericUpDown numericUpDown)
            {
                numericUpDown.BackColor = palette.InputBack;
                numericUpDown.ForeColor = palette.Text;
            }
            else if (control is ListBox listBox)
            {
                listBox.BackColor = palette.InputBack;
                listBox.ForeColor = palette.Text;
            }
            else if (control is Button button)
            {
                button.BackColor = palette.Accent;
                button.ForeColor = palette.AccentText;
                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.BorderColor = palette.Border;
            }
            else if (control is CheckBox checkBox)
            {
                if (checkBox.BackColor != Color.Transparent)
                {
                    checkBox.BackColor = SurfaceBack(checkBox, palette);
                }
                checkBox.ForeColor = palette.Text;
            }

            if (control.HasChildren)
            {
                ApplyThemeToControls(control.Controls, palette);
            }
        }
    }

    private static void ApplyThemeToDataGridView(DataGridView grid, ThemePalette palette)
    {
        grid.BackgroundColor = palette.CardBack;
        grid.DefaultCellStyle.BackColor = palette.InputBack;
        grid.DefaultCellStyle.ForeColor = palette.Text;
        grid.DefaultCellStyle.SelectionBackColor = palette.SelectionBack;
        grid.DefaultCellStyle.SelectionForeColor = palette.SelectionText;
        grid.ColumnHeadersDefaultCellStyle.BackColor = palette.MenuBack;
        grid.ColumnHeadersDefaultCellStyle.ForeColor = palette.Text;
        grid.RowHeadersDefaultCellStyle.BackColor = palette.MenuBack;
        grid.RowHeadersDefaultCellStyle.ForeColor = palette.Text;
        grid.GridColor = palette.GridLine;
    }
    }
}

