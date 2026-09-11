
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

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
        BackColor = SystemColors.Menu;
        BorderStyle = BorderStyle.FixedSingle;
        
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
        
        for (var i = 0; i < _items.Count; i++)
        {
            var row = i / _columns;
            var col = i % _columns;
            var x = ItemPadding + col * (_itemWidth + ItemPadding);
            var y = ItemPadding + row * (ItemHeight + ItemPadding);
            var rect = new Rectangle(x, y, _itemWidth, ItemHeight);
            
            if (i == _hoveredIndex)
            {
                using (var brush = new SolidBrush(SystemColors.Highlight))
                {
                    g.FillRectangle(brush, rect);
                }
            }
            else
            {
                using (var brush = new SolidBrush(SystemColors.Menu))
                {
                    g.FillRectangle(brush, rect);
                }
            }
            
            using (var pen = new Pen(i == _hoveredIndex ? SystemColors.Highlight : SystemColors.ControlDark))
            {
                g.DrawRectangle(pen, rect);
            }
            
            var text = _items[i].Item1;
            var textFormat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            
            using (var brush = new SolidBrush(i == _hoveredIndex ? SystemColors.HighlightText : SystemColors.MenuText))
            {
                g.DrawString(text, Font, brush, rect, textFormat);
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
        Size = new Size(_panel.Width + 2, _panel.Height + 2);
    }
    }
}

