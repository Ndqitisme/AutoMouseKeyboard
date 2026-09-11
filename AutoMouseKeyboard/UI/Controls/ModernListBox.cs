using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using AutoMouseKeyboard.Utilities;

namespace AutoMouseKeyboard.UI.Controls
{
    /// <summary>
    /// Owner-drawn <see cref="ListBox"/>: items render on the plain list
    /// background, the selected row gets a rounded pill
    /// (<see cref="ThemePalette.SelectionBack"/>/<see cref="ThemePalette.SelectionText"/>,
    /// r=4, inset 2px) and the row under the mouse gets a
    /// <see cref="ThemePalette.HoverBack"/> pill.
    /// <para>
    /// Only item rendering is replaced — DataSource, DisplayMember,
    /// SelectionMode.MultiExtended, IndexFromPoint and drag-drop all remain
    /// native base-class behavior.
    /// </para>
    /// </summary>
    public class ModernListBox : ListBox, IThemedControl
    {
        private ThemePalette _palette = ThemeManager.Palette;
        private int _hoverIndex = -1;

        public ModernListBox()
        {
            // NOTE: deliberately no ControlStyles.UserPaint. ListBox is backed by
            // the native LISTBOX window class; UserPaint would reroute WM_PAINT to
            // OnPaint, the native paint (which raises WM_DRAWITEM) would never run,
            // and the list would render empty. DrawMode.OwnerDrawFixed is the
            // supported custom-paint path for this control.
            DrawMode = DrawMode.OwnerDrawFixed;
            ItemHeight = 28;
            IntegralHeight = false;
            BorderStyle = BorderStyle.None;
            BackColor = _palette.InputBack;
            ForeColor = _palette.Text;
        }

        public void ApplyTheme(ThemePalette palette)
        {
            _palette = palette;
            BackColor = palette.InputBack;
            ForeColor = palette.Text;
            Invalidate();
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= Items.Count)
            {
                e.DrawBackground();
                base.OnDrawItem(e);
                return;
            }

            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var p = _palette;
            var selected = (e.State & DrawItemState.Selected) != 0;
            var hovered = Enabled && e.Index == _hoverIndex;

            // "Transparent" item background = plain list surface, no row fill.
            using (var br = new SolidBrush(BackColor))
            {
                g.FillRectangle(br, e.Bounds);
            }

            var textColor = Enabled ? p.Text : p.TextMuted;
            if (selected || hovered)
            {
                var pill = e.Bounds;
                pill.Inflate(-2, -2);
                using (var br = new SolidBrush(selected ? p.SelectionBack : p.HoverBack))
                {
                    g.FillRounded(br, pill, 4);
                }

                if (selected)
                {
                    textColor = p.SelectionText;
                }
            }

            // GetItemText honors DisplayMember / Format for bound items and
            // falls back to ToString() for plain objects.
            var textRect = new Rectangle(e.Bounds.X + 8, e.Bounds.Y, e.Bounds.Width - 16, e.Bounds.Height);
            TextRenderer.DrawText(g, GetItemText(Items[e.Index]), Font, textRect, textColor,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);

            // Base raises the DrawItem event for any external subscribers.
            base.OnDrawItem(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            // Base first so MouseMove subscribers (e.g. MainForm drag logic)
            // still see the event; hover tracking is purely cosmetic.
            base.OnMouseMove(e);

            var index = IndexFromPoint(e.Location);
            if (index < 0 || index >= Items.Count || !GetItemRectangle(index).Contains(e.Location))
            {
                index = -1;
            }

            SetHoverIndex(index);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            SetHoverIndex(-1);
            base.OnMouseLeave(e);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            Invalidate();
            base.OnEnabledChanged(e);
        }

        private void SetHoverIndex(int index)
        {
            if (index == _hoverIndex)
            {
                return;
            }

            var old = _hoverIndex;
            _hoverIndex = index;
            if (old >= 0 && old < Items.Count)
            {
                Invalidate(GetItemRectangle(old));
            }

            if (index >= 0 && index < Items.Count)
            {
                Invalidate(GetItemRectangle(index));
            }
        }
    }
}
