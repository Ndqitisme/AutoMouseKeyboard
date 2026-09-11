using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;
using AutoMouseKeyboard.Utilities;

namespace AutoMouseKeyboard.UI.Controls
{
    /// <summary>
    /// DropDownList-style selector: a rounded face showing the selected item's
    /// display text plus a drawn chevron. Clicking the face (or pressing
    /// Space / Down / F4) opens a <see cref="SelectDropDown"/> — a borderless
    /// topmost form anchored under the control.
    /// </summary>
    public class ModernSelect : Control, IThemedControl
    {
        private const int ChevronZoneWidth = 26;
        private const int ReopenGuardMs = 200;

        private ThemePalette _palette = ThemeManager.Palette;
        private readonly List<object> _items = new List<object>();
        private string _displayMember = string.Empty;
        private string _valueMember = string.Empty;
        private int _selectedIndex = -1;
        private bool _hover;
        private SelectDropDown? _dropDown;
        private int _lastDropDownCloseTick;

        public ModernSelect()
        {
            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.Opaque, false);
            BackColor = Color.Transparent; // corners outside the rounded fill show the parent surface
            ForeColor = _palette.Text;
            TabStop = true; // Control defaults to false; a selector should be tabbable like ComboBox
            Size = new Size(121, 28);
        }

        public int CornerRadius { get; set; } = 6;

        /// <summary>Objects shown in the drop-down list.</summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<object> Items => _items;

        /// <summary>
        /// Name of the public instance property reflected for display text
        /// (e.g. "Label" on bound option objects). Empty → item.ToString().
        /// </summary>
        [DefaultValue("")]
        public string DisplayMember
        {
            get => _displayMember;
            set
            {
                _displayMember = value ?? string.Empty;
                Invalidate();
            }
        }

        /// <summary>
        /// Stored for ComboBox-compatible call sites only; ModernSelect does
        /// not use it — <see cref="SelectedItem"/> returns the item object.
        /// </summary>
        [DefaultValue("")]
        public string ValueMember
        {
            get => _valueMember;
            set => _valueMember = value ?? string.Empty;
        }

        /// <summary>Selected item index, or −1 for no selection.</summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectedIndex
        {
            get
            {
                // Items is a plain list, so lazily drop a stale selection when
                // callers shrink it (e.g. Items.Clear() followed by a refill).
                if (_selectedIndex >= _items.Count)
                {
                    _selectedIndex = -1;
                }

                return _selectedIndex;
            }
            set
            {
                var index = value < 0 || value >= _items.Count ? -1 : value;
                if (index == _selectedIndex)
                {
                    return;
                }

                _selectedIndex = index;
                Invalidate();
                OnSelectedIndexChanged(EventArgs.Empty);
            }
        }

        /// <summary>The selected item object, or null when nothing is selected.</summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object? SelectedItem
        {
            get
            {
                var index = SelectedIndex;
                return index >= 0 ? _items[index] : null;
            }
            set => SelectedIndex = value == null ? -1 : _items.IndexOf(value);
        }

        /// <summary>
        /// Display text of the selected item. Setting selects the first item
        /// whose display text matches (case-insensitive), like a ComboBox in
        /// DropDownList mode; a non-matching value is ignored.
        /// </summary>
        public override string Text
        {
            get
            {
                var item = SelectedItem;
                return item == null ? string.Empty : GetDisplayText(item);
            }
            set
            {
                var text = value ?? string.Empty;
                for (var i = 0; i < _items.Count; i++)
                {
                    if (string.Equals(GetDisplayText(_items[i]), text, StringComparison.OrdinalIgnoreCase))
                    {
                        SelectedIndex = i;
                        return;
                    }
                }
            }
        }

        /// <summary>Raised when <see cref="SelectedIndex"/> actually changes.</summary>
        public event EventHandler? SelectedIndexChanged;

        protected virtual void OnSelectedIndexChanged(EventArgs e) => SelectedIndexChanged?.Invoke(this, e);

        public void ApplyTheme(ThemePalette palette)
        {
            _palette = palette;
            Invalidate();
        }

        /// <summary>Display text for one item: DisplayMember reflection, else ToString().</summary>
        internal string GetDisplayText(object? item)
        {
            if (item == null)
            {
                return string.Empty;
            }

            if (_displayMember.Length > 0)
            {
                var prop = item.GetType().GetProperty(_displayMember, BindingFlags.Public | BindingFlags.Instance);
                if (prop != null)
                {
                    return Convert.ToString(prop.GetValue(item), CultureInfo.CurrentCulture) ?? string.Empty;
                }
            }

            return item.ToString() ?? string.Empty;
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

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && Enabled)
            {
                Focus();
                if (_dropDown != null)
                {
                    _dropDown.Close();
                }
                else if (unchecked(Environment.TickCount - _lastDropDownCloseTick) >= ReopenGuardMs)
                {
                    // The click that dismissed a drop-down (via its Deactivate)
                    // lands here right after Close; swallow it so the list stays
                    // closed like a native ComboBox instead of reopening.
                    OpenDropDown();
                }
            }

            base.OnMouseDown(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (!Enabled || e.Handled)
            {
                return;
            }

            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.F4 || e.KeyCode == Keys.Space)
            {
                e.SuppressKeyPress = true;
                ToggleDropDown();
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            Invalidate();
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            Invalidate();
            base.OnLostFocus(e);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            if (!Enabled)
            {
                _dropDown?.Close();
            }

            Invalidate();
            base.OnEnabledChanged(e);
        }

        protected override void OnFontChanged(EventArgs e)
        {
            Invalidate();
            base.OnFontChanged(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var r = new Rectangle(0, 0, Width - 1, Height - 1);
            var p = _palette;

            var face = Enabled && (_hover || _dropDown != null) ? p.HoverBack : p.InputBack;
            using (var br = new SolidBrush(face))
            {
                g.FillRounded(br, r, CornerRadius);
            }

            var borderColor = Focused || _dropDown != null ? p.Accent : p.Border;
            using (var pen = new Pen(borderColor))
            {
                g.DrawRounded(pen, r, CornerRadius);
            }

            // Chevron: two-segment polyline (V) centered in the right zone.
            var cx = Width - ChevronZoneWidth / 2;
            var cy = Height / 2;
            using (var pen = new Pen(Enabled ? p.Text : p.TextMuted, 1.6f))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;
                pen.LineJoin = LineJoin.Round;
                g.DrawLines(pen, new[]
                {
                    new Point(cx - 4, cy - 2),
                    new Point(cx, cy + 2),
                    new Point(cx + 4, cy - 2),
                });
            }

            var textRect = new Rectangle(8, 0, Math.Max(0, Width - 8 - ChevronZoneWidth - 4), Height);
            TextRenderer.DrawText(g, Text, Font, textRect, Enabled ? p.Text : p.TextMuted,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }

        private void ToggleDropDown()
        {
            if (_dropDown != null)
            {
                _dropDown.Close();
                return;
            }

            OpenDropDown();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // The drop-down is an unowned top-level form; without this it
                // would outlive a disposed owner as an orphan window.
                _dropDown?.Close();
            }

            base.Dispose(disposing);
        }

        private void OpenDropDown()
        {
            if (_dropDown != null || _items.Count == 0 || !Enabled)
            {
                return;
            }

            var dd = new SelectDropDown(_palette, Font, _items, GetDisplayText, SelectedIndex,
                index => SelectedIndex = index);

            var height = SelectDropDown.PreferredHeight(_items.Count);
            var origin = PointToScreen(new Point(0, Height + 2));
            var workArea = Screen.FromControl(this).WorkingArea;

            var x = origin.X;
            var y = origin.Y;
            if (y + height > workArea.Bottom)
            {
                // No room below — flip above the control when there is space.
                var aboveY = PointToScreen(Point.Empty).Y - height - 2;
                if (aboveY >= workArea.Top)
                {
                    y = aboveY;
                }
                else
                {
                    height = Math.Max(SelectDropDown.MinimumHeight, workArea.Bottom - y);
                }
            }

            if (x + Width > workArea.Right)
            {
                x = Math.Max(workArea.Left, workArea.Right - Width);
            }

            dd.Bounds = new Rectangle(x, y, Width, height);

            // Close when the owner or its top-level form moves / resizes / hides.
            EventHandler close = (s, ev) => dd.Close();
            Move += close;
            Resize += close;
            VisibleChanged += close;

            var ownerForm = FindForm();
            FormClosedEventHandler? ownerClosed = null;
            if (ownerForm != null)
            {
                ownerClosed = (s, ev) => dd.Close();
                ownerForm.Move += close;
                ownerForm.Resize += close;
                ownerForm.FormClosed += ownerClosed;
            }

            dd.FormClosed += (s, ev) =>
            {
                Move -= close;
                Resize -= close;
                VisibleChanged -= close;
                if (ownerForm != null)
                {
                    ownerForm.Move -= close;
                    ownerForm.Resize -= close;
                    if (ownerClosed != null)
                    {
                        ownerForm.FormClosed -= ownerClosed;
                    }
                }

                // Only a Deactivate-dismissal arms the mouse reopen guard —
                // Esc/Enter closes shouldn't block a deliberate click or key.
                if (dd.ClosedByDeactivate)
                {
                    _lastDropDownCloseTick = Environment.TickCount;
                }

                _dropDown = null;
                Invalidate();
            };

            _dropDown = dd;
            Invalidate();
            dd.Show();
            dd.Activate(); // ensures a later Deactivate fires for click-outside close
        }
    }

    /// <summary>
    /// Borderless topmost popup hosting the owner-drawn item list for
    /// <see cref="ModernSelect"/>. Closes on Deactivate (click outside), Esc,
    /// or item confirm; the palette is captured in the constructor.
    /// </summary>
    internal sealed class SelectDropDown : Form
    {
        internal const int ItemHeight = 26;
        private const int MaxVisibleItems = 10;
        private const int MaxDropDownHeight = 260; // spec: min(items*26+2, 260)
        private const int BorderPad = 1;

        private readonly ThemePalette _palette;
        private readonly Func<object?, string> _getText;
        private readonly Action<int> _confirm;
        private readonly ListBox _list;

        public SelectDropDown(ThemePalette palette, Font font, IReadOnlyList<object> items,
            Func<object?, string> getText, int selectedIndex, Action<int> confirm)
        {
            _palette = palette;
            _getText = getText;
            _confirm = confirm;

            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;
            TopMost = true;
            StartPosition = FormStartPosition.Manual;
            KeyPreview = true;
            BackColor = palette.InputBack;

            // 1px border: the panel paints Border color behind a 1px padding gap.
            var borderPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(BorderPad),
                BackColor = palette.Border,
                Tag = ThemeManager.SkipThemeTag,
            };

            _list = new ListBox
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.None,
                DrawMode = DrawMode.OwnerDrawFixed,
                ItemHeight = ItemHeight,
                IntegralHeight = false,
                BackColor = palette.InputBack,
                ForeColor = palette.Text,
                Font = font,
                Tag = ThemeManager.SkipThemeTag,
            };
            foreach (var item in items)
            {
                _list.Items.Add(item);
            }

            _list.DrawItem += List_DrawItem;
            _list.MouseMove += List_MouseMove;
            _list.MouseClick += List_MouseClick;
            _list.KeyDown += List_KeyDown;

            borderPanel.Controls.Add(_list);
            Controls.Add(borderPanel);

            // Sync the highlight to the owner's current selection on open.
            if (selectedIndex >= 0 && selectedIndex < _list.Items.Count)
            {
                _list.SelectedIndex = selectedIndex;
                if (selectedIndex >= MaxVisibleItems)
                {
                    _list.TopIndex = selectedIndex - MaxVisibleItems + 1;
                }
            }

            Size = new Size(120, PreferredHeight(items.Count));
        }

        /// <summary>Height for a given item count, capped at 260px total.</summary>
        internal static int PreferredHeight(int itemCount)
            => Math.Min(Math.Max(itemCount, 1) * ItemHeight + BorderPad * 2, MaxDropDownHeight);

        internal static int MinimumHeight => ItemHeight + BorderPad * 2;

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            _list.Focus();
        }

        /// <summary>
        /// True when the close was triggered by losing activation (click
        /// outside / owner click) rather than by Esc or item confirm. The owner
        /// uses this to decide whether to arm its click-reopen guard.
        /// </summary>
        internal bool ClosedByDeactivate { get; private set; }

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);
            ClosedByDeactivate = true;
            Close(); // click outside / focus loss dismisses without selecting
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            // KeyPreview routes keys here even while the inner list has focus.
            if (e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
                Close(); // Esc must NOT select — just dismiss
                return;
            }

            base.OnKeyDown(e);
        }

        private void List_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space)
            {
                e.SuppressKeyPress = true;
                Confirm(_list.SelectedIndex);
            }
        }

        private void List_MouseMove(object? sender, MouseEventArgs e)
        {
            var index = _list.IndexFromPoint(e.Location);
            if (index >= 0 && index < _list.Items.Count &&
                _list.GetItemRectangle(index).Contains(e.Location))
            {
                _list.SelectedIndex = index; // hover moves the highlight
            }
        }

        private void List_MouseClick(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            var index = _list.IndexFromPoint(e.Location);
            if (index >= 0 && index < _list.Items.Count &&
                _list.GetItemRectangle(index).Contains(e.Location))
            {
                Confirm(index);
            }
        }

        private void Confirm(int index)
        {
            if (index < 0 || index >= _list.Items.Count)
            {
                return;
            }

            _confirm(index); // sets owner SelectedIndex → fires SelectedIndexChanged on change
            Close();
        }

        private void List_DrawItem(object? sender, DrawItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= _list.Items.Count)
            {
                e.DrawBackground();
                return;
            }

            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var selected = (e.State & DrawItemState.Selected) != 0;

            using (var br = new SolidBrush(_palette.InputBack))
            {
                g.FillRectangle(br, e.Bounds);
            }

            var textColor = _palette.Text;
            if (selected)
            {
                // Same pill style as ModernListBox: r=4, inset 2px.
                var pill = e.Bounds;
                pill.Inflate(-2, -2);
                using (var br = new SolidBrush(_palette.SelectionBack))
                {
                    g.FillRounded(br, pill, 4);
                }

                textColor = _palette.SelectionText;
            }

            var textRect = new Rectangle(e.Bounds.X + 8, e.Bounds.Y, e.Bounds.Width - 16, e.Bounds.Height);
            TextRenderer.DrawText(g, _getText(_list.Items[e.Index]), _list.Font, textRect, textColor,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }
    }
}
