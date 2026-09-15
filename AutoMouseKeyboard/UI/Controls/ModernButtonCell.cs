using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using AutoMouseKeyboard.Utilities;

namespace AutoMouseKeyboard.UI.Controls
{
    /// <summary>
    /// <see cref="DataGridViewButtonColumn"/> whose cells render a rounded,
    /// palette-colored button (the <see cref="ModernButton"/> Secondary look)
    /// instead of the native gray button chrome.
    /// </summary>
    public class ModernButtonColumn : DataGridViewButtonColumn
    {
        public ModernButtonColumn()
        {
            CellTemplate = new ModernButtonCell();
        }
    }

    /// <summary>
    /// Custom-painted <see cref="DataGridViewButtonCell"/>: the native button is
    /// suppressed in <see cref="Paint"/> and replaced with a rounded rect filled
    /// from <see cref="ThemePalette"/> (CardBack -> SelectionBack when pressed,
    /// Border -> Accent outline on hover/press).
    /// </summary>
    public class ModernButtonCell : DataGridViewButtonCell
    {
        private const int CornerRadius = 5;
        private const int VerticalInset = 4;

        private bool _hover;
        private bool _pressed;

        protected override void OnMouseEnter(int rowIndex)
        {
            _hover = true;
            Invalidate();
            base.OnMouseEnter(rowIndex);
        }

        protected override void OnMouseLeave(int rowIndex)
        {
            _hover = false;
            _pressed = false;
            Invalidate();
            base.OnMouseLeave(rowIndex);
        }

        protected override void OnMouseDown(DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _pressed = true;
                Invalidate();
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(DataGridViewCellMouseEventArgs e)
        {
            if (_pressed)
            {
                _pressed = false;
                Invalidate();
            }

            base.OnMouseUp(e);
        }

        private void Invalidate()
        {
            DataGridView?.InvalidateCell(this);
        }

        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds,
            int rowIndex, DataGridViewElementStates elementState, object? value, object? formattedValue,
            string? errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle,
            DataGridViewPaintParts paintParts)
        {
            // The native button chrome is drawn under the Content parts; strip
            // them so base only paints the cell surface (back, selection,
            // border, error icon), then draw the themed button on top.
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, elementState, value,
                formattedValue, errorText, cellStyle, advancedBorderStyle,
                paintParts & ~(DataGridViewPaintParts.ContentForeground | DataGridViewPaintParts.ContentBackground));

            var text = formattedValue as string;
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            var pad = cellStyle?.Padding ?? Padding.Empty;
            var button = new Rectangle(
                cellBounds.X + pad.Left,
                cellBounds.Y + VerticalInset,
                Math.Max(0, cellBounds.Width - pad.Horizontal),
                Math.Max(0, cellBounds.Height - VerticalInset * 2));
            if (button.Width <= 0 || button.Height <= 0)
            {
                return;
            }

            var p = ThemeManager.Palette;
            var fill = _pressed ? p.SelectionBack : p.CardBack;
            var outline = (_hover || _pressed) ? p.Accent : p.Border;

            // The grid reuses this Graphics for the whole paint pass; keep the
            // AntiAlias change scoped or later 1px lines render offset and show
            // as dark seams along the column boundaries of the painted row.
            var graphicsState = graphics.Save();
            try
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var br = new SolidBrush(fill))
                {
                    graphics.FillRounded(br, button, CornerRadius);
                }

                using (var pen = new Pen(outline))
                {
                    var borderRect = new Rectangle(button.X, button.Y, button.Width - 1, button.Height - 1);
                    graphics.DrawRounded(pen, borderRect, CornerRadius);
                }
            }
            finally
            {
                graphics.Restore(graphicsState);
            }

            TextRenderer.DrawText(graphics, text, cellStyle?.Font ?? DataGridView?.Font ?? Control.DefaultFont,
                button, p.Text,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }
    }
}
