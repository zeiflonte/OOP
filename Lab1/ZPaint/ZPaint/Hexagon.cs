using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ZPaint
{
    public class Hexagon : Polygon
    {
        public Hexagon(Point point1, Point point2) : base(point1, point2)
        {
        }

        protected override void SetScales()
        {
            Height = Math.Abs(point1.Y - point2.Y);
            Width = Math.Abs(point1.X - point2.X);
            if (Width > Height)
            {
                Height = Width;
            }
            else
            {
                Width = Height;
            }
        }

        protected override Point[] DrawPolygon()
        {
            Point[] hexagon = new Point[]
            {
                new Point(0.5 * Width, 0),
                new Point(Width, 0.25 * Height),
                new Point(Width, 0.75 * Height),
                new Point(0.5 * Width, Height),
                new Point(0, 0.75 * Height),
                new Point(0, 0.25 * Height),
            };
            return hexagon;
        }
    }
}
