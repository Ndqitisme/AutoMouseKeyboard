using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AutoMouseKeyboard.UI.Controls
{
    /// <summary>
    /// Flat <see cref="ToolStripProfessionalRenderer"/> that paints menus from a
    /// <see cref="ThemePalette"/>: <see cref="ThemePalette.MenuBack"/> surfaces,
    /// a rounded (r=4) <see cref="ThemePalette.MenuHover"/> item highlight,
    /// <see cref="ThemePalette.MenuText"/> text/arrows and a 1px rounded
    /// <see cref="ThemePalette.Border"/> edge on drop-downs.
    /// <para>
    /// Assign to <see cref="ToolStrip.Renderer"/> on ContextMenuStrip /
    /// ToolStripDropDown instances. The palette is captured in the constructor;
    /// recreate menus (or re-assign the renderer) after a theme change.
    /// </para>
    /// </summary>
    public class ModernToolStripRenderer : ToolStripProfessionalRenderer
    {
        private const int CornerRadius = 4;
        private const int SeparatorMargin = 4;
        private const int ImageMarginWidth = 30;

        private readonly ThemePalette _palette;

        public ModernToolStripRenderer(ThemePalette palette)
            : base(new PaletteColorTable(palette))
        {
            _palette = palette;
        }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            var item = e.Item;
            if ((item.Selected || item.Pressed) && item.Enabled)
            {
                // e.Graphics is already translated so (0,0) is the item origin.
                var rect = new Rectangle(Point.Empty, item.Size);
                if (rect.Width <= 0 || rect.Height <= 0)
                {
                    return;
                }

                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var brush = new SolidBrush(_palette.MenuHover))
                {
                    e.Graphics.FillRounded(brush, rect, CornerRadius);
                }
            }
            else
            {
                // Nothing to paint for idle items - the surface was already
                // filled via ToolStripDropDownBackground / strip gradient.
                base.OnRenderMenuItemBackground(e);
            }
        }

        protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
        {
            // Flat fill instead of the default 3-stop gradient.
            using (var brush = new SolidBrush(_palette.MenuBack))
            {
                e.Graphics.FillRectangle(brush, e.AffectedBounds);
            }
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            if (e.ToolStrip is ToolStripDropDown dropDown)
            {
                var rect = new Rectangle(0, 0, dropDown.Width - 1, dropDown.Height - 1);
                if (rect.Width <= 0 || rect.Height <= 0)
                {
                    return;
                }

                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var pen = new Pen(_palette.Border))
                {
                    e.Graphics.DrawRounded(pen, rect, CornerRadius);
                }
            }
            else
            {
                base.OnRenderToolStripBorder(e);
            }
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            e.TextColor = e.Item.Enabled ? _palette.MenuText : _palette.TextMuted;
            base.OnRenderItemText(e);
        }

        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            var bounds = new Rectangle(Point.Empty, e.Item.Size);
            using (var pen = new Pen(_palette.Border))
            {
                if (e.Vertical)
                {
                    var x = bounds.Width / 2;
                    e.Graphics.DrawLine(pen, x, SeparatorMargin, x, bounds.Height - SeparatorMargin - 1);
                }
                else
                {
                    var y = bounds.Height / 2;
                    // On a menu, start the rule after the image margin.
                    var left = e.Item.IsOnDropDown ? ImageMarginWidth : SeparatorMargin;
                    e.Graphics.DrawLine(pen, left, y, bounds.Width - SeparatorMargin - 1, y);
                }
            }
        }

        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            e.ArrowColor = e.Item.Enabled ? _palette.MenuText : _palette.TextMuted;
            base.OnRenderArrow(e);
        }

        /// <summary>
        /// Maps every renderer color the base implementation can query onto the
        /// palette so no system-color gradient leaks through.
        /// </summary>
        private sealed class PaletteColorTable : ProfessionalColorTable
        {
            private readonly ThemePalette _p;

            public PaletteColorTable(ThemePalette palette)
            {
                _p = palette;
            }

            // Menu item highlight + edges.
            public override Color MenuItemSelected => _p.MenuHover;
            public override Color MenuItemBorder => _p.MenuHover;
            public override Color MenuBorder => _p.Border;
            public override Color MenuItemSelectedGradientBegin => _p.MenuHover;
            public override Color MenuItemSelectedGradientEnd => _p.MenuHover;
            public override Color MenuItemPressedGradientBegin => _p.MenuHover;
            public override Color MenuItemPressedGradientMiddle => _p.MenuHover;
            public override Color MenuItemPressedGradientEnd => _p.MenuHover;

            // Surfaces.
            public override Color ToolStripDropDownBackground => _p.MenuBack;
            public override Color ImageMarginGradientBegin => _p.MenuBack;
            public override Color ImageMarginGradientMiddle => _p.MenuBack;
            public override Color ImageMarginGradientEnd => _p.MenuBack;
            public override Color MenuStripGradientBegin => _p.MenuBack;
            public override Color MenuStripGradientEnd => _p.MenuBack;
            public override Color ToolStripGradientBegin => _p.MenuBack;
            public override Color ToolStripGradientMiddle => _p.MenuBack;
            public override Color ToolStripGradientEnd => _p.MenuBack;
            public override Color ToolStripContentPanelGradientBegin => _p.MenuBack;
            public override Color ToolStripContentPanelGradientEnd => _p.MenuBack;
            public override Color StatusStripGradientBegin => _p.MenuBack;
            public override Color StatusStripGradientEnd => _p.MenuBack;
            public override Color OverflowButtonGradientBegin => _p.MenuBack;
            public override Color OverflowButtonGradientMiddle => _p.MenuBack;
            public override Color OverflowButtonGradientEnd => _p.MenuBack;

            // Details.
            public override Color SeparatorDark => _p.Border;
            public override Color SeparatorLight => _p.MenuBack;
            public override Color GripDark => _p.Border;
            public override Color GripLight => _p.MenuBack;
            public override Color CheckBackground => _p.MenuBack;
            public override Color CheckSelectedBackground => _p.MenuHover;
            public override Color CheckPressedBackground => _p.MenuHover;

            // ToolStripButton states (if the renderer is used on a ToolStrip).
            public override Color ButtonSelectedHighlight => _p.MenuHover;
            public override Color ButtonSelectedHighlightBorder => _p.MenuHover;
            public override Color ButtonPressedHighlight => _p.MenuHover;
            public override Color ButtonPressedHighlightBorder => _p.MenuHover;
            public override Color ButtonCheckedHighlight => _p.MenuHover;
            public override Color ButtonCheckedHighlightBorder => _p.MenuHover;
            public override Color ButtonSelectedBorder => _p.MenuHover;
            public override Color ButtonPressedBorder => _p.MenuHover;
            public override Color ButtonSelectedGradientBegin => _p.MenuHover;
            public override Color ButtonSelectedGradientMiddle => _p.MenuHover;
            public override Color ButtonSelectedGradientEnd => _p.MenuHover;
            public override Color ButtonPressedGradientBegin => _p.MenuHover;
            public override Color ButtonPressedGradientMiddle => _p.MenuHover;
            public override Color ButtonPressedGradientEnd => _p.MenuHover;
            public override Color ButtonCheckedGradientBegin => _p.MenuHover;
            public override Color ButtonCheckedGradientMiddle => _p.MenuHover;
            public override Color ButtonCheckedGradientEnd => _p.MenuHover;
        }
    }
}
