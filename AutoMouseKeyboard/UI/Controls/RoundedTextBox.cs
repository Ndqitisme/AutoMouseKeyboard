using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using AutoMouseKeyboard.Utilities;

namespace AutoMouseKeyboard.UI.Controls
{
    /// <summary>
    /// Rounded input box wrapping a borderless inner <see cref="TextBox"/>.
    /// The border turns to the accent color while the inner box has focus.
    /// Height is driven by the inner text box metrics (≈27px by default).
    /// </summary>
    public class RoundedTextBox : UserControl, IThemedControl
    {
        private ThemePalette _palette = ThemeManager.Palette;
        private readonly TextBox _inner;

        public RoundedTextBox()
        {
            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);
            AutoSize = false;
            BackColor = Color.Transparent; // corners outside the rounded fill show the parent surface

            _inner = new TextBox
            {
                BorderStyle = BorderStyle.None,
                Anchor = AnchorStyles.Left | AnchorStyles.Top,
                BackColor = _palette.InputBack,
                ForeColor = _palette.Text,
                Tag = ThemeManager.SkipThemeTag, // themed by ApplyTheme, not ThemeManager recursion
            };
            _inner.Enter += (s, e) => Invalidate();
            _inner.Leave += (s, e) => Invalidate();
            _inner.TextChanged += (s, e) => OnTextChanged(e);
            Controls.Add(_inner);

            Height = _inner.Height + 10;
            LayoutInner();
        }

        /// <summary>The wrapped borderless text box (exposed for MaxLength, PasswordChar, etc.).</summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TextBox Inner => _inner;

        public int CornerRadius { get; set; } = 6;

        /// <summary>Border color override; null falls back to <see cref="ThemePalette.Border"/>.</summary>
        public Color? BorderColor { get; set; }

        /// <summary>Focused border color override; null falls back to <see cref="ThemePalette.Accent"/>.</summary>
        public Color? FocusColor { get; set; }

        /// <summary>Passes through to the inner text box.</summary>
        public override string Text
        {
            get => _inner.Text;
            set => _inner.Text = value;
        }

        public void ApplyTheme(ThemePalette palette)
        {
            _palette = palette;
            _inner.BackColor = palette.InputBack;
            _inner.ForeColor = palette.Text;
            Invalidate();
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            _inner.Focus();
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            // The inner box inherits our font (ambient) so its height just changed.
            Height = _inner.Height + 10;
            LayoutInner();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            LayoutInner();
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var r = new Rectangle(0, 0, Width - 1, Height - 1);

            using (var br = new SolidBrush(_palette.InputBack))
            {
                g.FillRounded(br, r, CornerRadius);
            }

            var border = _inner.Focused
                ? (FocusColor ?? _palette.Accent)
                : (BorderColor ?? _palette.Border);
            using (var pen = new Pen(border))
            {
                g.DrawRoundedBorder(pen, r, CornerRadius);
            }
        }

        private void LayoutInner()
        {
            if (_inner == null)
            {
                return;
            }

            // Bounds are managed explicitly — anchoring Right would capture a
            // negative margin because the ctor runs while Width is still 0, and
            // the inner box would spill past the right edge and cover the border.
            _inner.SetBounds(8, Math.Max(0, (Height - _inner.Height) / 2),
                Math.Max(1, Width - 16), _inner.Height);
        }
    }
}
