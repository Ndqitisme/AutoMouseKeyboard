using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutoMouseKeyboard.Models;
using AutoMouseKeyboard.Services;
using AutoMouseKeyboard.Utilities;

namespace AutoMouseKeyboard
{
    public partial class MainForm
    {
        private static readonly Dictionary<Keys, string> KeyTokenOverrides = CreateKeyOverrideMap();

        private void ConfigureGrid()
        {
            gridActions.AutoGenerateColumns = false;
            gridActions.AllowUserToAddRows = false;
            gridActions.AllowUserToDeleteRows = false;
            gridActions.DataSource = _actions;
            _actions.ListChanged += Actions_ListChanged;
            gridActions.RowsAdded += (sender, e) => UpdateRowNumbers();
            gridActions.RowsRemoved += (sender, e) => UpdateRowNumbers();
            gridActions.CurrentCellDirtyStateChanged += (sender, e) =>
            {
                if (gridActions.IsCurrentCellDirty)
                {
                    gridActions.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
            };
            gridActions.CellClick += gridActions_CellClick;
            gridActions.CellFormatting += gridActions_CellFormatting;
            gridActions.CellEndEdit += gridActions_CellEndEdit;
            gridActions.CellBeginEdit += gridActions_CellBeginEdit;
            gridActions.EnterKeyPressed += gridActions_EnterKeyPressed;
        }

        private void InitializeActionMenus()
        {
            ctxAddKey.Items.Clear();
            ctxAddMouse.Items.Clear();

            ctxAddKey.Items.Add(CreateKeySection(LanguageManager.GetString("Menu_F1F12Key"), KeyHelper.EnumerateKeys(i => string.Format("F{0}", i), 1, 12), 3));
            ctxAddKey.Items.Add(CreateKeySection(LanguageManager.GetString("Menu_AZKey"), KeyHelper.EnumerateKeys(i => ((char)('A' + i - 1)).ToString(), 1, 26), 4));
            ctxAddKey.Items.Add(CreateKeySection(LanguageManager.GetString("Menu_09Key"), KeyHelper.EnumerateKeys(i => (i - 1).ToString(), 1, 10), 3));
            ctxAddKey.Items.Add(CreateKeySection(LanguageManager.GetString("Menu_SymbolKey"), new[]
            {
                Tuple.Create("-", "OemMinus"),
                Tuple.Create("=", "Oemplus"),
                Tuple.Create("[", "OemOpenBrackets"),
                Tuple.Create("]", "OemCloseBrackets"),
                Tuple.Create(";", "OemSemicolon"),
                Tuple.Create("'", "OemQuotes"),
                Tuple.Create(",", "Oemcomma"),
                Tuple.Create(".", "OemPeriod"),
                Tuple.Create("/", "OemQuestion"),
                Tuple.Create("\\", "OemPipe"),
                Tuple.Create("`", "Oemtilde")
            }, 3));
            ctxAddKey.Items.Add(CreateKeySection(LanguageManager.GetString("Menu_NumKey"), CreateNumPadKeys(), 3));
            ctxAddKey.Items.Add(CreateKeySection(LanguageManager.GetString("Menu_OtherKey"), CreateOtherKeys(), 3));

            foreach (var option in ActionStep.MouseOptions)
            {
                var item = new ToolStripMenuItem(option.Label) { Tag = option.Kind };
                item.Click += MouseMenuItem_Click;
                ctxAddMouse.Items.Add(item);
            }
        }

        private IEnumerable<Tuple<string, string>> CreateNumPadKeys()
        {
            yield return Tuple.Create(LanguageManager.GetString("KeyName_NumLock"), "NumLock");
            yield return Tuple.Create(LanguageManager.GetString("KeyName_Num0"), "NumPad0");
            yield return Tuple.Create(LanguageManager.GetString("KeyName_Num1"), "NumPad1");
            yield return Tuple.Create(LanguageManager.GetString("KeyName_Num2"), "NumPad2");
            yield return Tuple.Create(LanguageManager.GetString("KeyName_Num3"), "NumPad3");
            yield return Tuple.Create(LanguageManager.GetString("KeyName_Num4"), "NumPad4");
            yield return Tuple.Create(LanguageManager.GetString("KeyName_Num5"), "NumPad5");
            yield return Tuple.Create(LanguageManager.GetString("KeyName_Num6"), "NumPad6");
            yield return Tuple.Create(LanguageManager.GetString("KeyName_Num7"), "NumPad7");
            yield return Tuple.Create(LanguageManager.GetString("KeyName_Num8"), "NumPad8");
            yield return Tuple.Create(LanguageManager.GetString("KeyName_Num9"), "NumPad9");
            yield return Tuple.Create(LanguageManager.GetString("KeyName_NumDot"), "Decimal");
            yield return Tuple.Create(LanguageManager.GetString("KeyName_NumPlus"), "Add");
            yield return Tuple.Create(LanguageManager.GetString("KeyName_NumMinus"), "Subtract");
            yield return Tuple.Create(LanguageManager.GetString("KeyName_NumMultiply"), "Multiply");
            yield return Tuple.Create(LanguageManager.GetString("KeyName_NumDivide"), "Divide");
        }

        private IEnumerable<Tuple<string, string>> CreateOtherKeys()
        {
            yield return Tuple.Create(LanguageManager.GetString("KeyName_Enter"), "Enter");
            yield return Tuple.Create(LanguageManager.GetString("KeyName_Space"), "Space");
            yield return Tuple.Create(LanguageManager.GetString("KeyName_Tab"), "Tab");
            yield return Tuple.Create(LanguageManager.GetString("KeyName_Esc"), "Esc");
            yield return Tuple.Create(LanguageManager.GetString("KeyName_Backspace"), "Backspace");
            yield return Tuple.Create(LanguageManager.GetString("KeyName_PrintScreen"), "PrintScreen");
            yield return Tuple.Create(LanguageManager.GetString("KeyName_ScrollLock"), "ScrollLock");
            yield return Tuple.Create(LanguageManager.GetString("KeyName_Pause"), "Pause");
            yield return Tuple.Create(LanguageManager.GetString("KeyName_Insert"), "Insert");
            yield return Tuple.Create(LanguageManager.GetString("KeyName_Delete"), "Delete");
            yield return Tuple.Create(LanguageManager.GetString("KeyName_Home"), "Home");
            yield return Tuple.Create(LanguageManager.GetString("KeyName_End"), "End");
            yield return Tuple.Create(LanguageManager.GetString("KeyName_PageUp"), "PageUp");
            yield return Tuple.Create(LanguageManager.GetString("KeyName_PageDown"), "PageDown");
            yield return Tuple.Create(LanguageManager.GetString("KeyName_Up"), "Up");
            yield return Tuple.Create(LanguageManager.GetString("KeyName_Down"), "Down");
            yield return Tuple.Create(LanguageManager.GetString("KeyName_Left"), "Left");
            yield return Tuple.Create(LanguageManager.GetString("KeyName_Right"), "Right");
            yield return Tuple.Create(LanguageManager.GetString("KeyName_CapsLock"), "Capital");
            yield return Tuple.Create(LanguageManager.GetString("KeyName_Shift"), "Shift");
            yield return Tuple.Create(LanguageManager.GetString("KeyName_Ctrl"), "Ctrl");
            yield return Tuple.Create(LanguageManager.GetString("KeyName_Alt"), "Alt");
            yield return Tuple.Create(LanguageManager.GetString("KeyName_LeftWindows"), "LWin");
            yield return Tuple.Create(LanguageManager.GetString("KeyName_RightWindows"), "RWin");
            yield return Tuple.Create(LanguageManager.GetString("KeyName_Application"), "Apps");
        }

        private ToolStripMenuItem CreateKeySection(string title, IEnumerable<Tuple<string, string>> keys, int columns = 3)
        {
            var keysList = keys.ToList();
            var section = new ToolStripMenuItem(title);

            section.DropDownOpening += (sender, e) =>
            {
                var menu = sender as ToolStripMenuItem;
                if (menu != null && menu.DropDown is MultiColumnMenuStrip)
                {
                    return; // Already created
                }

                var multiColumnMenu = new MultiColumnMenuStrip(keysList, columns);
                multiColumnMenu.ItemClicked += (s, token) =>
                {
                    var step = CreateKeyboardStep(token);
                    AddAction(step);
                    ctxAddKey.Close();
                };

                if (menu != null)
                {
                    menu.DropDown = multiColumnMenu;
                    menu.DropDown.AutoClose = true;
                }
            };

            return section;
        }

        private static Dictionary<Keys, string> CreateKeyOverrideMap()
        {
            var map = new Dictionary<Keys, string>();

            void Add(Keys key, string value) => map[key] = value;

            Add(Keys.ControlKey, "Ctrl"); Add(Keys.LControlKey, "Ctrl"); Add(Keys.RControlKey, "Ctrl");
            Add(Keys.ShiftKey, "Shift"); Add(Keys.LShiftKey, "Shift"); Add(Keys.RShiftKey, "Shift");
            Add(Keys.Menu, "Alt"); Add(Keys.LMenu, "Alt"); Add(Keys.RMenu, "Alt");
            Add(Keys.Enter, "Enter"); Add(Keys.Return, "Enter");
            Add(Keys.Space, "Space"); Add(Keys.Tab, "Tab");
            Add(Keys.Back, "Backspace"); Add(Keys.Escape, "Esc");
            Add(Keys.Insert, "Insert"); Add(Keys.Delete, "Delete");
            Add(Keys.Home, "Home"); Add(Keys.End, "End");
            Add(Keys.PageUp, "PageUp"); Add(Keys.PageDown, "PageDown");
            Add(Keys.Up, "Up"); Add(Keys.Down, "Down"); Add(Keys.Left, "Left"); Add(Keys.Right, "Right");
            Add(Keys.PrintScreen, "PrintScreen"); Add(Keys.Pause, "Pause");
            Add(Keys.Apps, "Apps"); Add(Keys.Sleep, "Sleep");
            Add(Keys.Add, "Add"); Add(Keys.Subtract, "Subtract"); Add(Keys.Divide, "Divide"); Add(Keys.Multiply, "Multiply");
            Add(Keys.Decimal, "Decimal"); Add(Keys.Separator, "Separator");
            Add(Keys.OemSemicolon, nameof(Keys.OemSemicolon)); Add(Keys.OemQuotes, nameof(Keys.OemQuotes));
            Add(Keys.Oemcomma, nameof(Keys.Oemcomma)); Add(Keys.OemPeriod, nameof(Keys.OemPeriod));
            Add(Keys.OemQuestion, nameof(Keys.OemQuestion)); Add(Keys.Oemtilde, nameof(Keys.Oemtilde));
            Add(Keys.OemMinus, nameof(Keys.OemMinus)); Add(Keys.Oemplus, nameof(Keys.Oemplus));
            Add(Keys.OemOpenBrackets, nameof(Keys.OemOpenBrackets)); Add(Keys.OemCloseBrackets, nameof(Keys.OemCloseBrackets));
            Add(Keys.OemPipe, nameof(Keys.OemPipe));

            return map;
        }

        private void MouseMenuItem_Click(object? sender, EventArgs e)
        {
            var menuItem = sender as ToolStripMenuItem;
            if (menuItem == null || !(menuItem.Tag is MouseButtonKind kind))
            {
                return;
            }

            using (var dialog = new MouseActionForm(kind) { Owner = this })
            {
                if (dialog.ShowDialog(this) == DialogResult.OK && dialog.Result != null)
                {
                    AddAction(dialog.Result);
                }
            }
        }

        private void btnDeleteAction_Click(object? sender, EventArgs e) => RemoveSelectedActions();

        private void RemoveSelectedActions()
        {
            var removed = false;
            foreach (DataGridViewRow row in gridActions.SelectedRows)
            {
                if (!row.IsNewRow && row.DataBoundItem is ActionStep step)
                {
                    _actions.Remove(step);
                    removed = true;
                }
            }

            if (removed)
            {
                UpdateRowNumbers();
            }
        }

        private async void gridActions_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0 || e.RowIndex >= _actions.Count)
            {
                return;
            }

            if (gridActions.Columns[e.ColumnIndex].Name != "colGetPosition")
            {
                return;
            }

            var step = _actions[e.RowIndex];
            if (step == null || step.Type != ActionKind.Mouse)
            {
                return;
            }

            var point = await CaptureMousePositionAsync();
            if (!point.HasValue)
            {
                return;
            }

            step.X = point.Value.X;
            step.Y = point.Value.Y;
            gridActions.Refresh();
        }

        private async Task<Point?> CaptureMousePositionAsync()
        {
            if (!_hasShownCapturePositionHint && FirstRunHelper.ShouldShowCapturePositionHint())
            {
                MessageBox.Show(LanguageManager.GetString("Msg_CapturePosition"), LanguageManager.GetString("Msg_CapturePositionTitle"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                FirstRunHelper.MarkCapturePositionHintShown();
                _hasShownCapturePositionHint = true;
            }

            using (FormHelper.HideFormForOperation(this))
            {
                try
                {
                    return await MouseCaptureService.CaptureAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, LanguageManager.GetString("Msg_CaptureFailed"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
        }

        private void gridActions_DataError(object? sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void UpdateRowNumbers()
        {
            for (var i = 0; i < gridActions.Rows.Count; i++)
            {
                var row = gridActions.Rows[i];
                if (row.IsNewRow)
                {
                    continue;
                }

                row.Cells[colIndex.Name].Value = (i + 1).ToString();
            }
        }

        private void gridActions_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (gridActions.Columns[e.ColumnIndex].Name == "colGetPosition")
            {
                if (e.RowIndex >= 0 && e.RowIndex < _actions.Count)
                {
                    var step = _actions[e.RowIndex];
                    if (step != null && step.Type == ActionKind.Mouse)
                    {
                        e.Value = LanguageManager.GetString("MainForm_ColSetPosition");
                        e.FormattingApplied = true;
                    }
                    else
                    {
                        e.Value = "";
                        e.FormattingApplied = true;
                    }
                }
            }
        }

        private void gridActions_EnterKeyPressed(object? sender, KeyEventArgs e)
        {
            var currentRowIndex = gridActions.CurrentCell?.RowIndex ?? -1;

            gridActions.EndEdit();

            if (currentRowIndex >= 0 && currentRowIndex < gridActions.Rows.Count)
            {
                gridActions.ClearSelection();
                gridActions.Rows[currentRowIndex].Selected = true;
                gridActions.CurrentCell = gridActions.Rows[currentRowIndex].Cells[0];
            }

            e.Handled = true;
        }

        private void gridActions_KeyDown(object? sender, KeyEventArgs e)
        {
            if (IsAltTab(e))
            {
                _modifierTracker.CancelSession();
                e.SuppressKeyPress = true;
                e.Handled = true;
                return;
            }

            _modifierTracker.HandleKeyDown(e.KeyCode);

            if (ModifierKeyTracker.IsModifierKey(e.KeyCode))
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
                return;
            }

            if (gridActions.IsCurrentCellInEditMode)
            {
                return;
            }

            if (TryCreateActionFromKey(e, out var step))
            {
                _modifierTracker.MarkNonModifierUsed();
                AddAction(step!);
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
            else
            {
                if (!_hasShownTelexWarning)
                {
                    MessageBox.Show(
                        LanguageManager.GetString("Msg_TelexWarning"),
                        LanguageManager.GetString("Msg_TelexWarningTitle"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    _hasShownTelexWarning = true;
                }
            }
        }

        private void gridActions_KeyUp(object? sender, KeyEventArgs e)
        {
            var combo = _modifierTracker.HandleKeyUp(e.KeyCode);
            if (combo.HasValue)
            {
                var modifierCount = (combo.HoldCtrl ? 1 : 0) + (combo.HoldAlt ? 1 : 0) + (combo.HoldShift ? 1 : 0);
                if (modifierCount < 2)
                {
                    e.SuppressKeyPress = true;
                    e.Handled = true;
                    return;
                }

                var modifierStep = CreateModifierOnlyStep(combo);
                AddAction(modifierStep);
                e.SuppressKeyPress = true;
                e.Handled = true;
                return;
            }

            if (ModifierKeyTracker.IsModifierKey(e.KeyCode))
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }

        private void ResetModifierTracking()
        {
            _modifierTracker.Reset();
        }

        private void gridActions_PreviewKeyDown(object? sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Menu || e.KeyCode == Keys.LMenu || e.KeyCode == Keys.RMenu ||
                e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.ShiftKey || e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
            }
        }

        private bool TryCreateActionFromKey(KeyEventArgs e, out ActionStep? step)
        {
            step = null;

            if (ModifierKeyTracker.IsModifierKey(e.KeyCode))
            {
                return false;
            }

            if (IsAltTab(e))
            {
                return false;
            }

            var baseToken = ResolveKeyToken(e.KeyCode);
            if (baseToken == null)
            {
                return false;
            }

            var (holdCtrl, holdAlt, holdShift) = _modifierTracker.GetHoldStates();

            step = CreateKeyboardStep(
                baseToken,
                holdCtrl || e.Control,
                holdAlt || e.Alt,
                holdShift || e.Shift);
            return true;
        }

        private static bool IsAltTab(KeyEventArgs e)
        {
            return e != null && e.Alt && e.KeyCode == Keys.Tab;
        }

        private static string ResolveKeyToken(Keys keyCode)
        {
            if (KeyTokenOverrides.TryGetValue(keyCode, out var token))
            {
                return token;
            }

            if (keyCode >= Keys.A && keyCode <= Keys.Z)
            {
                return ((char)('A' + (keyCode - Keys.A))).ToString();
            }

            if (keyCode >= Keys.D0 && keyCode <= Keys.D9)
            {
                return ((char)('0' + (keyCode - Keys.D0))).ToString();
            }

            if (keyCode >= Keys.NumPad0 && keyCode <= Keys.NumPad9)
            {
                return string.Format("NumPad{0}", keyCode - Keys.NumPad0);
            }

            if (keyCode >= Keys.F1 && keyCode <= Keys.F24)
            {
                return string.Format("F{0}", keyCode - Keys.F1 + 1);
            }

            return null!;
        }

        private ActionStep CreateKeyboardStep(string token, bool holdCtrl = false, bool holdAlt = false, bool holdShift = false)
        {
            return new ActionStep
            {
                Type = ActionKind.Keyboard,
                Key = token ?? string.Empty,
                Delay = DefaultActionDelay,
                Repeat = 1,
                HoldCtrl = holdCtrl,
                HoldAlt = holdAlt,
                HoldShift = holdShift
            };
        }

        private ActionStep CreateModifierOnlyStep(ModifierComboResult combo)
        {
            var tokens = new List<string>();
            if (combo.HoldCtrl) tokens.Add("Ctrl");
            if (combo.HoldAlt) tokens.Add("Alt");
            if (combo.HoldShift) tokens.Add("Shift");

            var keyToken = string.Join("+", tokens);
            return new ActionStep
            {
                Type = ActionKind.Keyboard,
                Key = keyToken,
                Delay = DefaultActionDelay,
                Repeat = 1
            };
        }

        private void AddAction(ActionStep step)
        {
            _actions.Add(step);
            UpdateRowNumbers();
        }

        private void Actions_ListChanged(object? sender, ListChangedEventArgs e) => UpdateRowNumbers();

        private void gridActions_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _actions.Count)
            {
                return;
            }

            var step = _actions[e.RowIndex];
            if (step == null)
            {
                return;
            }

            ActionStep? result = null;
            if (step.Type == ActionKind.Mouse)
            {
                using (var dialog = new MouseActionForm(step) { Owner = this })
                {
                    if (dialog.ShowDialog(this) == DialogResult.OK)
                    {
                        result = dialog.Result;
                    }
                }
            }
            else if (step.Type == ActionKind.Keyboard)
            {
                using (var dialog = new KeyActionForm(step) { Owner = this })
                {
                    if (dialog.ShowDialog(this) == DialogResult.OK)
                    {
                        result = dialog.Result;
                    }
                }
            }

            if (result != null)
            {
                var index = _actions.IndexOf(step);
                if (index >= 0)
                {
                    _actions[index] = result;
                    gridActions.Refresh();
                }
            }
        }

        private void gridActions_CellBeginEdit(object? sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                var column = gridActions.Columns[e.ColumnIndex];
                if (column.Name != "colCount" && column.Name != "colDelay")
                {
                    e.Cancel = true;
                    return;
                }

                _editingColumnName = column.Name;
                _editingRowIndex = e.RowIndex;

                _selectedRowIndices = gridActions.SelectedRows.Cast<DataGridViewRow>()
                    .Where(row => row.Index >= 0 && row.Index < _actions.Count)
                    .Select(row => row.Index)
                    .ToList();

                if (!_selectedRowIndices.Contains(e.RowIndex))
                {
                    _selectedRowIndices.Add(e.RowIndex);
                }
            }
        }

        private void gridActions_CellEndEdit(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _actions.Count || _editingColumnName == null)
            {
                return;
            }

            var currentCell = gridActions.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (currentCell.Value == null)
            {
                return;
            }

            if (!int.TryParse(currentCell.Value.ToString(), out var intValue))
            {
                return;
            }

            var currentStep = _actions[e.RowIndex];
            if (currentStep != null)
            {
                if (_editingColumnName == "colCount")
                {
                    currentStep.Repeat = Math.Max(1, intValue);
                }
                else if (_editingColumnName == "colDelay")
                {
                    currentStep.Delay = Math.Max(0, intValue);
                }
            }

            var rowsToUpdate = _selectedRowIndices
                .Where(index => index != e.RowIndex && index >= 0 && index < _actions.Count)
                .ToList();

            foreach (var rowIndex in rowsToUpdate)
            {
                var step = _actions[rowIndex];
                if (step != null)
                {
                    if (_editingColumnName == "colCount")
                    {
                        step.Repeat = Math.Max(1, intValue);
                    }
                    else if (_editingColumnName == "colDelay")
                    {
                        step.Delay = Math.Max(0, intValue);
                    }
                }
            }

            gridActions.Refresh();

            _editingColumnName = null;
            _editingRowIndex = -1;
            _selectedRowIndices.Clear();
        }

        private void btnMoveUp_Click(object? sender, EventArgs e)
        {
            MoveAction(-1);
        }

        private void btnMoveDown_Click(object? sender, EventArgs e)
        {
            MoveAction(1);
        }

        private void MoveAction(int direction)
        {
            if (gridActions.CurrentRow == null)
            {
                return;
            }
            var step = gridActions.CurrentRow.DataBoundItem as ActionStep;
            if (step == null)
            {
                return;
            }

            var index = _actions.IndexOf(step);
            var newIndex = index + direction;

            if (direction < 0 && index > 0)
            {
                _actions.RemoveAt(index);
                _actions.Insert(newIndex, step);
                SelectAndFocusRow(newIndex);
            }
            else if (direction > 0 && index >= 0 && index < _actions.Count - 1)
            {
                _actions.RemoveAt(index);
                _actions.Insert(newIndex, step);
                SelectAndFocusRow(newIndex);
            }
        }

        private void SelectAndFocusRow(int rowIndex)
        {
            gridActions.ClearSelection();
            gridActions.Rows[rowIndex].Selected = true;
            gridActions.CurrentCell = gridActions.Rows[rowIndex].Cells[0];
            UpdateRowNumbers();
        }
    }
}

