using System;
using System.Drawing;
using System.Windows.Forms;
using AutoMouseKeyboard.Utilities;

namespace AutoMouseKeyboard.UI.Controls
{
    /// <summary>
    /// Borderless <see cref="EnterAwareDataGridView"/> styled from the theme
    /// palette: flat 32px header band (<see cref="ThemePalette.MenuBack"/> /
    /// <see cref="ThemePalette.Text"/>, Segoe UI 9 semibold), 30px rows, single
    /// horizontal gridlines (<see cref="ThemePalette.GridLine"/> — also supplies
    /// the header bottom rule), palette selection colors and a
    /// <see cref="ThemePalette.HoverBack"/> row highlight under the mouse.
    /// Keeps native scrollbars, editing and <c>EnterKeyPressed</c>.
    /// </summary>
    public class StyledDataGridView : EnterAwareDataGridView, IThemedControl
    {
        private ThemePalette _palette = ThemeManager.Palette;
        private int _hoveredRow = -1;
        private readonly Font _headerFont = CreateHeaderFont();

        public StyledDataGridView()
        {
            BorderStyle = BorderStyle.None;
            EnableHeadersVisualStyles = false;
            RowHeadersVisible = false;
            CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            ColumnHeadersHeight = 32;
            RowTemplate.Height = 30;
            DoubleBuffered = true;

            // Horizontal breathing room for cell + header text; also insets the
            // editing control. Colors are applied by ApplyTheme below.
            DefaultCellStyle.Padding = new Padding(8, 0, 8, 0);
            ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 0, 8, 0);

            ApplyTheme(_palette);
        }

        private static Font CreateHeaderFont()
        {
            // FontStyle has no SemiBold value; the named family does. It ships
            // with Windows 8+, but fall back to regular bold if absent.
            try
            {
                using (var semibold = new FontFamily("Segoe UI Semibold"))
                {
                    if (semibold.IsStyleAvailable(FontStyle.Regular))
                    {
                        return new Font(semibold, 9f, FontStyle.Regular);
                    }
                }
            }
            catch (ArgumentException)
            {
            }

            return new Font("Segoe UI", 9f, FontStyle.Bold);
        }

        public void ApplyTheme(ThemePalette palette)
        {
            _palette = palette;

            BackgroundColor = palette.CardBack;
            GridColor = palette.GridLine;

            var header = ColumnHeadersDefaultCellStyle;
            header.BackColor = palette.MenuBack;
            header.ForeColor = palette.Text;
            // Never wrap header text — an undersized column ellipsizes instead
            // of growing a second line inside the fixed 32px header band.
            header.WrapMode = DataGridViewTriState.False;
            // Keep the header flat when a column is "selected" via current cell.
            header.SelectionBackColor = palette.MenuBack;
            header.SelectionForeColor = palette.Text;
            header.Font = _headerFont;

            var cells = DefaultCellStyle;
            cells.BackColor = palette.CardBack;
            cells.ForeColor = palette.Text;
            cells.SelectionBackColor = palette.SelectionBack;
            cells.SelectionForeColor = palette.SelectionText;

            var rows = RowsDefaultCellStyle;
            rows.BackColor = palette.CardBack;
            rows.ForeColor = palette.Text;
            rows.SelectionBackColor = palette.SelectionBack;
            rows.SelectionForeColor = palette.SelectionText;

            RowHeadersDefaultCellStyle.BackColor = palette.MenuBack;
            RowHeadersDefaultCellStyle.ForeColor = palette.Text;
            RowHeadersDefaultCellStyle.SelectionBackColor = palette.MenuBack;
            RowHeadersDefaultCellStyle.SelectionForeColor = palette.Text;

            Invalidate();
        }

        protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
        {
            // CellStyle is a per-paint clone: overriding BackColor repaints only
            // unselected cells in the hovered row; SelectionBackColor still wins
            // for selected cells. The cell hosting the editing control is
            // skipped: the hosted TextBox is inset by the cell padding, so a
            // hover-colored band would show on both sides of the editor.
            var editingThisCell = IsCurrentCellInEditMode && CurrentCell != null &&
                e.RowIndex == CurrentCell.RowIndex && e.ColumnIndex == CurrentCell.ColumnIndex;
            if (!editingThisCell && e.RowIndex >= 0 && e.RowIndex == _hoveredRow && e.ColumnIndex >= 0 && e.CellStyle != null)
            {
                e.CellStyle.BackColor = _palette.HoverBack;
            }

            base.OnCellPainting(e);
        }

        protected override void OnEditingControlShowing(DataGridViewEditingControlShowingEventArgs e)
        {
            // Native editor fallback: match the cell surface and the hosted
            // TextBox to the input colors so the padding bands and the editor
            // read as one uniform field. RoundedTextBoxEditingControl paints
            // its own surface and needs the cell to keep its normal colors.
            if (e.Control is DataGridViewTextBoxEditingControl textBox)
            {
                e.CellStyle.BackColor = _palette.InputBack;
                e.CellStyle.ForeColor = _palette.Text;
                e.CellStyle.SelectionBackColor = _palette.InputBack;
                e.CellStyle.SelectionForeColor = _palette.Text;

                textBox.BorderStyle = BorderStyle.FixedSingle;
                textBox.BackColor = _palette.InputBack;
                textBox.ForeColor = _palette.Text;
            }

            base.OnEditingControlShowing(e);
        }

        protected override void OnCellMouseEnter(DataGridViewCellEventArgs e)
        {
            SetHoveredRow(e.RowIndex);
            base.OnCellMouseEnter(e);
        }

        protected override void OnCellMouseLeave(DataGridViewCellEventArgs e)
        {
            // Only clear when leaving the currently hovered row - moving across
            // cells of the same row fires Leave(row) after Enter(next row).
            if (e.RowIndex == _hoveredRow)
            {
                SetHoveredRow(-1);
            }

            base.OnCellMouseLeave(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            SetHoveredRow(-1);
            base.OnMouseLeave(e);
        }

        protected override void OnRowsAdded(DataGridViewRowsAddedEventArgs e)
        {
            SetHoveredRow(-1);
            base.OnRowsAdded(e);
        }

        protected override void OnRowsRemoved(DataGridViewRowsRemovedEventArgs e)
        {
            SetHoveredRow(-1);
            base.OnRowsRemoved(e);
        }

        private void SetHoveredRow(int rowIndex)
        {
            if (rowIndex == _hoveredRow)
            {
                return;
            }

            var old = _hoveredRow;
            _hoveredRow = rowIndex;
            if (old >= 0 && old < Rows.Count)
            {
                InvalidateRow(old);
            }

            if (rowIndex >= 0 && rowIndex < Rows.Count)
            {
                InvalidateRow(rowIndex);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _headerFont.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
