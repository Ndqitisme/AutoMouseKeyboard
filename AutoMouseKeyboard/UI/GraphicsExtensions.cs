using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace AutoMouseKeyboard.UI
{
    internal static class GraphicsExtensions
    {
        public static GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            var d = radius * 2;
            var path = new GraphicsPath();
            if (d <= 0 || bounds.Width <= 0 || bounds.Height <= 0) { path.AddRectangle(bounds); return path; }
            if (d > bounds.Width) d = bounds.Width;
            if (d > bounds.Height) d = bounds.Height;
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        public static void FillRounded(this Graphics g, Brush brush, Rectangle r, int radius)
        {
            using var path = RoundedRect(r, radius);
            g.FillPath(brush, path);
        }

        public static void DrawRounded(this Graphics g, Pen pen, Rectangle r, int radius)
        {
            using var path = RoundedRect(r, radius);
            g.DrawPath(pen, path);
        }

        /// <summary>
        /// Draws a rounded border kept fully inside the control: the pen stroke is
        /// centered on the path, so a path hugging the control edge loses half its
        /// width to clipping. Insetting by half the pen width keeps the whole stroke
        /// visible on the right/bottom edges.
        /// </summary>
        public static void DrawRoundedBorder(this Graphics g, Pen pen, Rectangle r, int radius)
        {
            var half = pen.Width / 2f;
            var inset = new RectangleF(r.X + half, r.Y + half,
                Math.Max(0f, r.Width - pen.Width), Math.Max(0f, r.Height - pen.Width));
            using var path = RoundedRectF(inset, radius);
            g.DrawPath(pen, path);
        }

        private static GraphicsPath RoundedRectF(RectangleF bounds, int radius)
        {
            var d = radius * 2f;
            var path = new GraphicsPath();
            if (d <= 0 || bounds.Width <= 0 || bounds.Height <= 0) { path.AddRectangle(bounds); return path; }
            if (d > bounds.Width) d = bounds.Width;
            if (d > bounds.Height) d = bounds.Height;
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        public static Color Lerp(Color a, Color b, float t)
        {
            t = Math.Max(0f, Math.Min(1f, t));
            return Color.FromArgb(
                (int)(a.A + (b.A - a.A) * t),
                (int)(a.R + (b.R - a.R) * t),
                (int)(a.G + (b.G - a.G) * t),
                (int)(a.B + (b.B - a.B) * t));
        }
    }
}
