using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ZPaint
{
    public class Triangle : Polygon
    {
        public Triangle(Point point1, Point point2) : base(point1, point2)
        { }

        protected override Point[] DrawPolygon()
        {
            Point[] triangle = new Point[3]
            {
                new Point((point2.X - point1.X) / 2, 0),
                new Point(point2.X - point1.X, point2.Y - point1.Y),
                new Point(0, point2.Y - point1.Y)
            };
            return triangle;
        }
    }
}
