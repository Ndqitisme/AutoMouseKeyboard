using System;
using System.Drawing;
using System.Windows.Forms;
using AutoMouseKeyboard.Utilities;

namespace AutoMouseKeyboard.UI.Controls
{
    public enum ButtonStyleKind { Primary, Secondary }

    public class ModernButton : Button, IThemedControl
    {
        private ThemePalette _palette = ThemeManager.Palette;
        private float _hoverAmount;            // 0..1 animated
        private bool _pressed;
        private readonly Timer _anim = new Timer { Interval = 15 };
        private float _hoverTarget;

        public ButtonStyleKind StyleKind { get; set; } = ButtonStyleKind.Secondary;
        public int CornerRadius { get; set; } = 6;

        public ModernButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);
            // ButtonBase sets Opaque, which suppresses the background erase and
            // leaves garbage in the corners outside the rounded fill. Clear it so
            // the transparent BackColor erases to the real parent surface instead.
            SetStyle(ControlStyles.Opaque, false);
            BackColor = Color.Transparent;
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            _anim.Tick += (s, e) => TickAnimation();
            Cursor = Cursors.Hand;
        }

        public void ApplyTheme(ThemePalette palette) { _palette = palette; Invalidate(); }

        private void TickAnimation()
        {
            if (IsDisposed || Disposing)
            {
                _anim.Stop();
                return;
            }

            var step = 0.125f; // 8 ticks × 15ms ≈ 120ms transition
            var next = _hoverAmount + (_hoverTarget > _hoverAmount ? step : -step);
            if ((_hoverTarget > _hoverAmount && next >= _hoverTarget) ||
                (_hoverTarget < _hoverAmount && next <= _hoverTarget))
            { _hoverAmount = _hoverTarget; _anim.Stop(); }
            else _hoverAmount = next;
            Invalidate();
        }

        protected override void OnMouseEnter(EventArgs e) { _hoverTarget = 1f; _anim.Start(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { _hoverTarget = 0f; _anim.Start(); base.OnMouseLeave(e); }
        protected override void OnMouseDown(MouseEventArgs e) { _pressed = true; Invalidate(); base.OnMouseDown(e); }
        protected override void OnMouseUp(MouseEventArgs e) { _pressed = false; Invalidate(); base.OnMouseUp(e); }
        protected override void OnEnabledChanged(EventArgs e) { Invalidate(); base.OnEnabledChanged(e); }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            var r = new Rectangle(0, 0, Width - 1, Height - 1);
            var p = _palette;

            Color back, fore, border;
            if (StyleKind == ButtonStyleKind.Primary)
            {
                back = !Enabled ? p.Border : _pressed ? p.AccentPressed : GraphicsExtensions.Lerp(p.Accent, p.AccentHover, _hoverAmount);
                fore = p.AccentText; border = Color.Transparent;
            }
            else
            {
                back = !Enabled ? p.WindowBack : _pressed ? p.SelectionBack : GraphicsExtensions.Lerp(p.CardBack, p.HoverBack, _hoverAmount);
                fore = Enabled ? p.Text : p.TextMuted; border = p.Border;
            }

            using (var br = new SolidBrush(back)) g.FillRounded(br, r, CornerRadius);
            if (border != Color.Transparent)
                using (var pen = new Pen(border)) g.DrawRounded(pen, r, CornerRadius);

            TextRenderer.DrawText(g, Text, Font, ClientRectangle, fore,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) { _anim.Stop(); _anim.Dispose(); }
            base.Dispose(disposing);
        }
    }
}
