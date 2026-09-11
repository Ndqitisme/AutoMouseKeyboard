using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using AutoMouseKeyboard.Utilities;

namespace AutoMouseKeyboard.UI.Controls
{
    /// <summary>
    /// Fully custom-painted <see cref="CheckBox"/>: a 16px rounded box plus a
    /// check polyline when checked, with the label text drawn to its right.
    /// Clicking still toggles <see cref="CheckBox.Checked"/> natively.
    /// </summary>
    public class ModernCheckBox : CheckBox, IThemedControl
    {
        private const int BoxSize = 16;
        private const int BoxRadius = 4;
        private const int TextLeft = 22; // BoxSize + 6px gap

        private ThemePalette _palette = ThemeManager.Palette;
        private bool _hover;

        public ModernCheckBox()
        {
            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
            AutoSize = false;
            Appearance = Appearance.Normal;
        }

        public void ApplyTheme(ThemePalette palette)
        {
            _palette = palette;
            Invalidate();
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            var textSize = TextRenderer.MeasureText(Text, Font);
            return new Size(20 + textSize.Width + 6, Math.Max(20, textSize.Height + 4));
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            _hover = true;
            Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _hover = false;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnCheckedChanged(EventArgs e)
        {
            Invalidate();
            base.OnCheckedChanged(e);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            Invalidate();
            base.OnEnabledChanged(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var p = _palette;

            var box = new Rectangle(0, Math.Max(0, (Height - BoxSize) / 2), BoxSize, BoxSize);

            Color fill;
            if (!Enabled)
            {
                fill = p.InputBack;
            }
            else if (Checked)
            {
                fill = p.Accent;
            }
            else
            {
                fill = _hover ? p.HoverBack : p.InputBack;
            }

            using (var br = new SolidBrush(fill))
            {
                g.FillRounded(br, box, BoxRadius);
            }

            using (var pen = new Pen(Checked ? p.Accent : p.Border))
            {
                g.DrawRounded(pen, box, BoxRadius);
            }

            if (Checked)
            {
                // Check mark polyline {(4,8),(7,11),(12,5)} scaled to the box.
                var pts = new[]
                {
                    new Point(box.X + box.Width * 4 / 16, box.Y + box.Height * 8 / 16),
                    new Point(box.X + box.Width * 7 / 16, box.Y + box.Height * 11 / 16),
                    new Point(box.X + box.Width * 12 / 16, box.Y + box.Height * 5 / 16),
                };
                using (var pen = new Pen(p.AccentText, 1.8f))
                {
                    pen.StartCap = LineCap.Round;
                    pen.EndCap = LineCap.Round;
                    pen.LineJoin = LineJoin.Round;
                    g.DrawLines(pen, pts);
                }
            }

            var textRect = new Rectangle(TextLeft, 0, Math.Max(0, Width - TextLeft), Height);
            TextRenderer.DrawText(g, Text, Font, textRect,
                Enabled ? p.Text : p.TextMuted,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }
    }
}
