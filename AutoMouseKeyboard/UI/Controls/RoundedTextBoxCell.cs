using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using AutoMouseKeyboard.Utilities;

namespace AutoMouseKeyboard.UI.Controls
{
    /// <summary>
    /// <see cref="DataGridViewTextBoxColumn"/> whose cells edit through
    /// <see cref="RoundedTextBoxEditingControl"/> — a rounded, palette-colored
    /// input instead of the default square editing text box.
    /// </summary>
    public class RoundedTextBoxColumn : DataGridViewTextBoxColumn
    {
        public RoundedTextBoxColumn()
        {
            CellTemplate = new RoundedTextBoxCell();
        }
    }

    /// <summary>
    /// Text cell whose <see cref="EditType"/> routes editing to
    /// <see cref="RoundedTextBoxEditingControl"/>.
    /// </summary>
    public class RoundedTextBoxCell : DataGridViewTextBoxCell
    {
        public override Type EditType => typeof(RoundedTextBoxEditingControl);

        public override void InitializeEditingControl(int rowIndex, object? initialFormattedValue,
            DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
            // The stock cell only pushes initialFormattedValue when the editor is
            // a DataGridViewTextBoxEditingControl — seed ours explicitly.
            if (DataGridView?.EditingControl is RoundedTextBoxEditingControl control)
            {
                control.EditingControlFormattedValue = initialFormattedValue;
            }
        }
    }

    /// <summary>
    /// <see cref="IDataGridViewEditingControl"/> built on
    /// <see cref="RoundedTextBox"/>: the grid hosts it inside the cell's padded
    /// content area, so the rounded border reads as a floating input. The
    /// outer <see cref="Control.BackColor"/> is matched to the edited cell's
    /// surface (selection-aware) so the rounded corners blend in.
    /// </summary>
    public class RoundedTextBoxEditingControl : RoundedTextBox, IDataGridViewEditingControl
    {
        public RoundedTextBoxEditingControl()
        {
            CornerRadius = 5;
            Inner.TextChanged += (s, e) =>
            {
                EditingControlValueChanged = true;
                EditingControlDataGridView?.NotifyCurrentCellDirty(true);
            };
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataGridView? EditingControlDataGridView { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int EditingControlRowIndex { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool EditingControlValueChanged { get; set; }

        public Cursor EditingPanelCursor => Cursors.Default;

        public bool RepositionEditingControlOnValueChange => false;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object? EditingControlFormattedValue
        {
            get => Text;
            set => Text = value?.ToString() ?? string.Empty;
        }

        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        {
            Font = dataGridViewCellStyle.Font;
            // Corners outside the rounded fill show this color — keep it equal
            // to what the edited cell paints behind the editor.
            var selected = EditingControlDataGridView?.CurrentCell?.Selected == true;
            BackColor = selected ? dataGridViewCellStyle.SelectionBackColor : dataGridViewCellStyle.BackColor;
        }

        public bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
        {
            switch (keyData & Keys.KeyCode)
            {
                case Keys.Left:
                    return Inner.SelectionStart + Inner.SelectionLength > 0;
                case Keys.Right:
                    return Inner.SelectionStart + Inner.SelectionLength < Inner.TextLength;
                case Keys.Home:
                case Keys.End:
                case Keys.Delete:
                    return true;
                default:
                    // Enter/Escape/arrows/Tab: let the grid commit, cancel or navigate.
                    return false;
            }
        }

        public object? GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        {
            return EditingControlFormattedValue;
        }

        public void PrepareEditingControlForEdit(bool selectAll)
        {
            if (selectAll)
            {
                Inner.SelectAll();
            }
            else
            {
                Inner.SelectionStart = Inner.TextLength;
            }

            Inner.Focus();
        }
    }
}
