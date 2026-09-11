using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using AutoMouseKeyboard.Utilities;

namespace AutoMouseKeyboard.UI.Controls
{
    /// <summary>
    /// Rounded "card" container: CardBack fill, 1px border, optional muted title.
    /// <see cref="Text"/> proxies <see cref="Title"/> so existing assignments like
    /// <c>grpConfigs.Text = "…"</c> render as the card heading unchanged.
    /// Children are laid out inside the card inset by <see cref="Control.Padding"/>.
    /// </summary>
    public class CardPanel : Panel, IThemedControl
    {
        private ThemePalette _palette = ThemeManager.Palette;
        private string _title = string.Empty;
        private Font _titleFont;

        public CardPanel()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);
            BackColor = _palette.WindowBack;
            ForeColor = _palette.Text;
            Padding = new Padding(12);
            _titleFont = CreateTitleFont();
        }

        public int CornerRadius { get; set; } = 10;

        /// <summary>
        /// Card heading drawn in the title strip. Null/empty removes the title
        /// area and shrinks the padding back to a uniform margin.
        /// </summary>
        public string Title
        {
            get => _title;
            set
            {
                var newTitle = value ?? string.Empty;
                if (_title == newTitle)
                {
                    return;
                }

                var hadTitle = _title.Length > 0;
                _title = newTitle;
                var hasTitle = _title.Length > 0;
                if (hasTitle != hadTitle)
                {
                    Padding = hasTitle ? new Padding(12, 32, 12, 12) : new Padding(12);
                }

                Invalidate();
            }
        }

        /// <summary>Proxies <see cref="Title"/> so <c>.Text = "…"</c> callers keep working.</summary>
        public override string Text
        {
            get => Title;
            set => Title = value;
        }

        public void ApplyTheme(ThemePalette palette)
        {
            _palette = palette;
            // Corner pixels outside the rounded card match the window surface.
            BackColor = palette.WindowBack;
            ForeColor = palette.Text;
            Invalidate();
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            var oldFont = _titleFont;
            _titleFont = CreateTitleFont();
            oldFont.Dispose();
            Invalidate();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var r = new Rectangle(0, 0, Width - 1, Height - 1);

            using (var br = new SolidBrush(_palette.CardBack))
            {
                g.FillRounded(br, r, CornerRadius);
            }

            using (var pen = new Pen(_palette.Border))
            {
                g.DrawRounded(pen, r, CornerRadius);
            }

            if (_title.Length > 0)
            {
                TextRenderer.DrawText(g, _title, _titleFont,
                    new Point(Padding.Left, 8), _palette.TextMuted);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _titleFont.Dispose();
            }

            base.Dispose(disposing);
        }

        private Font CreateTitleFont() => new Font(Font.FontFamily, 9f, FontStyle.Bold);
    }
}
