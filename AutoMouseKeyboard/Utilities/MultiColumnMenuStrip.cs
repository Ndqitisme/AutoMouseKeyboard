
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using AutoMouseKeyboard.UI;
using AutoMouseKeyboard.UI.Controls;

namespace AutoMouseKeyboard.Utilities
{
    public class MultiColumnMenuPanel : Panel
    {
    private readonly List<Tuple<string, string>> _items;
    private readonly int _columns;
    private readonly int _itemWidth;
    private const int ItemHeight = 25;
    private const int ItemPadding = 3;
    private int _hoveredIndex = -1;
    private ToolStripDropDown? _parentDropDown;

    public event EventHandler<string>? ItemClicked;

    public MultiColumnMenuPanel(IEnumerable<Tuple<string, string>> items, int columns = 4, ToolStripDropDown? parentDropDown = null)
    {
        _items = items.ToList();
        _columns = columns;
        _parentDropDown = parentDropDown;
        
        using (var tempBitmap = new Bitmap(1, 1))
        using (var g = Graphics.FromImage(tempBitmap))
        {
            var maxWidth = 0;
            foreach (var item in _items)
            {
                var textSize = g.MeasureString(item.Item1, Font);
                maxWidth = Math.Max(maxWidth, (int)Math.Ceiling(textSize.Width));
            }
            _itemWidth = Math.Max(maxWidth + 12, 75);
        }
        
        var rows = (int)Math.Ceiling((double)_items.Count / _columns);
        var width = _columns * _itemWidth + (_columns + 1) * ItemPadding;
        var height = rows * ItemHeight + (rows + 1) * ItemPadding;
        
        Size = new Size(width, height);
        MinimumSize = Size;
        MaximumSize = Size;
        BackColor = ThemeManager.Palette.MenuBack;
        BorderStyle = BorderStyle.None;
        
        SetStyle(ControlStyles.AllPaintingInWmPaint | 
                 ControlStyles.UserPaint | 
                 ControlStyles.DoubleBuffer | 
                 ControlStyles.ResizeRedraw, true);
        
        Paint += MultiColumnMenuPanel_Paint;
        MouseMove += MultiColumnMenuPanel_MouseMove;
        MouseLeave += MultiColumnMenuPanel_MouseLeave;
        MouseClick += MultiColumnMenuPanel_MouseClick;
    }

    private void MultiColumnMenuPanel_Paint(object? sender, PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        // Read the palette live so an open menu picks up theme changes on the
        // next repaint without subscribing to ThemeManager.ThemeChanged.
        var palette = ThemeManager.Palette;

        for (var i = 0; i < _items.Count; i++)
        {
            var row = i / _columns;
            var col = i % _columns;
            var x = ItemPadding + col * (_itemWidth + ItemPadding);
            var y = ItemPadding + row * (ItemHeight + ItemPadding);
            var rect = new Rectangle(x, y, _itemWidth, ItemHeight);

            if (i == _hoveredIndex)
            {
                // Hovered cell: rounded MenuHover fill, no outline.
                using (var brush = new SolidBrush(palette.MenuHover))
                {
                    g.FillRounded(brush, rect, 4);
                }
            }
            else
            {
                using (var brush = new SolidBrush(palette.MenuBack))
                {
                    g.FillRounded(brush, rect, 4);
                }

                using (var pen = new Pen(palette.Border))
                {
                    g.DrawRounded(pen, rect, 4);
                }
            }

            var text = _items[i].Item1;
            using (var textFormat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            })
            using (var brush = new SolidBrush(palette.MenuText))
            {
                g.DrawString(text, Font, brush, rect, textFormat);
            }
        }

        // Panel edge: rounded 1px border replaces the old FixedSingle frame.
        var borderRect = new Rectangle(0, 0, Width - 1, Height - 1);
        if (borderRect.Width > 0 && borderRect.Height > 0)
        {
            using (var pen = new Pen(palette.Border))
            {
                g.DrawRounded(pen, borderRect, 4);
            }
        }
    }

    private void MultiColumnMenuPanel_MouseMove(object? sender, MouseEventArgs e)
    {
        var index = GetItemIndexAt(e.Location);
        if (index != _hoveredIndex)
        {
            _hoveredIndex = index;
            Invalidate();
        }
    }

    private void MultiColumnMenuPanel_MouseLeave(object? sender, EventArgs e)
    {
        if (_hoveredIndex != -1)
        {
            _hoveredIndex = -1;
            Invalidate();
        }
    }

    private void MultiColumnMenuPanel_MouseClick(object? sender, MouseEventArgs e)
    {
        var index = GetItemIndexAt(e.Location);
        if (index >= 0 && index < _items.Count)
        {
            ItemClicked?.Invoke(this, _items[index].Item2);
            _parentDropDown?.Close();
        }
    }

    private int GetItemIndexAt(Point location)
    {
        for (var i = 0; i < _items.Count; i++)
        {
            var row = i / _columns;
            var col = i % _columns;
            var x = ItemPadding + col * (_itemWidth + ItemPadding);
            var y = ItemPadding + row * (ItemHeight + ItemPadding);
            var rect = new Rectangle(x, y, _itemWidth, ItemHeight);
            
            if (rect.Contains(location))
            {
                return i;
            }
        }
        
        return -1;
    }
}

public class MultiColumnMenuStrip : ToolStripDropDown
{
    private readonly MultiColumnMenuPanel _panel;

    public new event EventHandler<string> ItemClicked
    {
        add => _panel.ItemClicked += value;
        remove => _panel.ItemClicked -= value;
    }

    public MultiColumnMenuStrip(IEnumerable<Tuple<string, string>> items, int columns = 4)
    {
        _panel = new MultiColumnMenuPanel(items, columns, this);
        var host = new ToolStripControlHost(_panel)
        {
            AutoSize = false,
            Margin = Padding.Empty,
            Padding = Padding.Empty
        };
        
        Items.Add(host);
        AutoSize = false;
        // The panel draws its own rounded border, so it fills the drop-down
        // exactly; the themed renderer + BackColor keep any stray edge pixels
        // off SystemColors.
        Padding = Padding.Empty;
        BackColor = ThemeManager.Palette.MenuBack;
        Renderer = new ModernToolStripRenderer(ThemeManager.Palette);
        Size = new Size(_panel.Width, _panel.Height);
    }
    }
}

