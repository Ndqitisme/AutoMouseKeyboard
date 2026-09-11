using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;
using AutoMouseKeyboard.Utilities;

namespace AutoMouseKeyboard.UI.Controls
{
    /// <summary>
    /// Rounded numeric spinner: a borderless inner <see cref="TextBox"/> plus a
    /// 22px strip on the right split into ▲ (top half) and ▼ (bottom half)
    /// chevron zones. Clicking a zone steps <see cref="Value"/> by
    /// <see cref="Increment"/> (hold to auto-repeat); Up/Down keys step too.
    /// Enter or focus loss commits the typed text — clamped to
    /// <see cref="Minimum"/>–<see cref="Maximum"/>, reverted when it doesn't
    /// parse. <see cref="ValueChanged"/> fires only on an actual change.
    /// </summary>
    public class ModernNumericUpDown : UserControl, IThemedControl
    {
        private const int ChevronStripWidth = 22;
        private const int RepeatDelayMs = 400;
        private const int RepeatRateMs = 70;
        private const int TextPadX = 8;

        private ThemePalette _palette = ThemeManager.Palette;
        private readonly TextBox _inner;
        private readonly Timer _repeat = new Timer();
        private decimal _value;
        private decimal _minimum;
        private decimal _maximum = 100m;
        private decimal _increment = 1m;
        private int _hoverHalf;   // +1 = up zone, -1 = down zone, 0 = none
        private int _pressedHalf; // +1 = up zone, -1 = down zone, 0 = none

        public ModernNumericUpDown()
        {
            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.Opaque, false);
            AutoSize = false;
            BackColor = Color.Transparent; // corners outside the rounded fill show the parent surface

            _inner = new TextBox
            {
                BorderStyle = BorderStyle.None,
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top,
                BackColor = _palette.InputBack,
                ForeColor = _palette.Text,
                Tag = ThemeManager.SkipThemeTag, // themed by ApplyTheme, not ThemeManager recursion
                Text = _value.ToString(CultureInfo.CurrentCulture),
            };
            _inner.Enter += (s, e) => Invalidate();            // focused → accent border
            _inner.Leave += (s, e) => { CommitText(); Invalidate(); };
            _inner.KeyDown += Inner_KeyDown;
            _inner.TextChanged += (s, e) => OnTextChanged(e);
            Controls.Add(_inner);

            _repeat.Tick += (s, e) =>
            {
                if (_repeat.Interval != RepeatRateMs)
                {
                    _repeat.Interval = RepeatRateMs;
                }

                Step(_pressedHalf);
            };

            Size = new Size(90, _inner.Height + 10);
            LayoutInner();
        }

        /// <summary>The wrapped borderless text box.</summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private TextBox Inner => _inner;

        public int CornerRadius { get; set; } = 6;

        /// <summary>Current value; setters clamp into [Minimum, Maximum].</summary>
        [DefaultValue(typeof(decimal), "0")]
        public decimal Value
        {
            get => _value;
            set => SetValue(value);
        }

        [DefaultValue(typeof(decimal), "0")]
        public decimal Minimum
        {
            get => _minimum;
            set
            {
                _minimum = value;
                if (_maximum < _minimum)
                {
                    _maximum = _minimum;
                }

                if (_value < _minimum)
                {
                    SetValue(_minimum);
                }
            }
        }

        [DefaultValue(typeof(decimal), "100")]
        public decimal Maximum
        {
            get => _maximum;
            set
            {
                _maximum = value;
                if (_minimum > _maximum)
                {
                    _minimum = _maximum;
                }

                if (_value > _maximum)
                {
                    SetValue(_maximum);
                }
            }
        }

        /// <summary>Step applied by the chevrons and the Up/Down keys.</summary>
        [DefaultValue(typeof(decimal), "1")]
        public decimal Increment
        {
            get => _increment;
            set => _increment = value;
        }

        /// <summary>Forwards to the inner text box alignment.</summary>
        [DefaultValue(HorizontalAlignment.Left)]
        public HorizontalAlignment TextAlign
        {
            get => _inner.TextAlign;
            set => _inner.TextAlign = value;
        }

        /// <summary>Raw edit text (uncommitted until Enter/Leave validation).</summary>
        public override string Text
        {
            get => _inner.Text;
            set => _inner.Text = value;
        }

        /// <summary>Raised when <see cref="Value"/> actually changes.</summary>
        public event EventHandler? ValueChanged;

        protected virtual void OnValueChanged(EventArgs e) => ValueChanged?.Invoke(this, e);

        public void ApplyTheme(ThemePalette palette)
        {
            _palette = palette;
            _inner.BackColor = palette.InputBack;
            _inner.ForeColor = palette.Text;
            Invalidate();
        }

        /// <summary>Clamps and stores the value; syncs the text box on change.</summary>
        private void SetValue(decimal value)
        {
            var clamped = Math.Min(Math.Max(value, _minimum), _maximum);
            if (clamped == _value)
            {
                return;
            }

            _value = clamped;
            _inner.Text = _value.ToString(CultureInfo.CurrentCulture);
            OnValueChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Parses the edit text: valid → clamp + store; invalid → revert to the
        /// current value's canonical text.
        /// </summary>
        private void CommitText()
        {
            if (decimal.TryParse(_inner.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out var parsed))
            {
                SetValue(parsed);
                // Normalize formatting even when the value is unchanged ("5." → "5").
                var canonical = _value.ToString(CultureInfo.CurrentCulture);
                if (_inner.Text != canonical)
                {
                    _inner.Text = canonical;
                }
            }
            else
            {
                _inner.Text = _value.ToString(CultureInfo.CurrentCulture);
            }
        }

        /// <summary>Commits any pending edit, then applies one ±Increment step.</summary>
        private void Step(int dir)
        {
            if (dir == 0)
            {
                return;
            }

            CommitText();
            decimal next;
            try
            {
                next = _value + _increment * dir;
            }
            catch (OverflowException)
            {
                next = dir > 0 ? _maximum : _minimum;
            }

            SetValue(next);
        }

        /// <summary>+1 up / −1 down / 0 outside the chevron strip.</summary>
        private int StripHitTest(Point pt)
        {
            if (pt.X < Width - ChevronStripWidth || pt.X >= Width || pt.Y < 0 || pt.Y >= Height)
            {
                return 0;
            }

            return pt.Y < Height / 2 ? 1 : -1;
        }

        private Rectangle StripBounds()
            => new Rectangle(Math.Max(0, Width - ChevronStripWidth - 1), 0,
                ChevronStripWidth + 1, Height);

        private void Inner_KeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    e.SuppressKeyPress = true;
                    Step(1);
                    break;
                case Keys.Down:
                    e.SuppressKeyPress = true;
                    Step(-1);
                    break;
                case Keys.Enter:
                    e.SuppressKeyPress = true;
                    CommitText();
                    break;
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            _inner.Focus();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            // Reachable when the control itself holds focus (inner disabled).
            if (e.KeyCode == Keys.Up)
            {
                e.Handled = true;
                Step(1);
                return;
            }

            if (e.KeyCode == Keys.Down)
            {
                e.Handled = true;
                Step(-1);
                return;
            }

            base.OnKeyDown(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (!Enabled || e.Button != MouseButtons.Left)
            {
                return;
            }

            // Moving focus into the inner box fires its Leave → commits the
            // pending edit before the step is applied (native NUD behavior).
            Focus();
            var half = StripHitTest(e.Location);
            if (half != 0)
            {
                _pressedHalf = half;
                Capture = true; // keep receiving MouseUp outside the strip
                Step(half);
                _repeat.Interval = RepeatDelayMs;
                _repeat.Start();
                Invalidate(StripBounds());
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (_pressedHalf != 0)
            {
                _pressedHalf = 0;
                _repeat.Stop();
                Capture = false;
                Invalidate(StripBounds());
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            var half = Enabled ? StripHitTest(e.Location) : 0;
            if (half != _hoverHalf)
            {
                _hoverHalf = half;
                Invalidate(StripBounds());
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (_hoverHalf != 0)
            {
                _hoverHalf = 0;
                Invalidate(StripBounds());
            }

            base.OnMouseLeave(e);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            _inner.Enabled = Enabled; // disabled TextBox renders GrayText
            _hoverHalf = 0;
            _pressedHalf = 0;
            _repeat.Stop();
            Invalidate();
            base.OnEnabledChanged(e);
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
            var p = _palette;
            var r = new Rectangle(0, 0, Width - 1, Height - 1);

            using (var br = new SolidBrush(p.InputBack))
            {
                g.FillRounded(br, r, CornerRadius);
            }

            var stripX = Math.Max(0, Width - ChevronStripWidth);
            var midY = Height / 2;
            var upRect = new Rectangle(stripX, 0, Width - stripX, midY);
            var downRect = new Rectangle(stripX, midY, Width - stripX, Height - midY);

            // Hover/pressed half fills, clipped inside the rounded outline so
            // the square half-rects can't bleed into the rounded corners.
            var state = g.Save();
            using (var clip = GraphicsExtensions.RoundedRect(r, CornerRadius))
            {
                g.SetClip(clip, CombineMode.Intersect);
            }

            PaintHalf(g, upRect, 1);
            PaintHalf(g, downRect, -1);
            g.Restore(state);

            // Strip separators: left edge + the half split.
            using (var pen = new Pen(p.Border))
            {
                g.DrawLine(pen, stripX, 4, stripX, Math.Max(4, Height - 5));
                g.DrawLine(pen, stripX, midY, Math.Max(stripX, Width - 3), midY);
            }

            var glyphColor = Enabled ? p.Text : p.TextMuted;
            DrawChevron(g, upRect, true, glyphColor);
            DrawChevron(g, downRect, false, glyphColor);

            var border = _inner.Focused ? p.Accent : p.Border;
            using (var pen = new Pen(border))
            {
                g.DrawRounded(pen, r, CornerRadius);
            }
        }

        private void PaintHalf(Graphics g, Rectangle rect, int half)
        {
            Color? fill = null;
            if (Enabled)
            {
                if (_pressedHalf == half)
                {
                    fill = _palette.SelectionBack;
                }
                else if (_hoverHalf == half)
                {
                    fill = _palette.HoverBack;
                }
            }

            if (fill.HasValue)
            {
                using (var br = new SolidBrush(fill.Value))
                {
                    g.FillRectangle(br, rect);
                }
            }
        }

        private static void DrawChevron(Graphics g, Rectangle rect, bool up, Color color)
        {
            var cx = rect.X + rect.Width / 2;
            var cy = rect.Y + rect.Height / 2;
            var pts = up
                ? new[] { new Point(cx - 4, cy + 2), new Point(cx, cy - 2), new Point(cx + 4, cy + 2) }
                : new[] { new Point(cx - 4, cy - 2), new Point(cx, cy + 2), new Point(cx + 4, cy - 2) };
            using (var pen = new Pen(color, 1.6f))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;
                pen.LineJoin = LineJoin.Round;
                g.DrawLines(pen, pts);
            }
        }

        private void LayoutInner()
        {
            var w = Math.Max(1, Width - TextPadX - ChevronStripWidth - 4);
            _inner.SetBounds(TextPadX, Math.Max(0, (Height - _inner.Height) / 2), w, _inner.Height);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repeat.Stop();
                _repeat.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
