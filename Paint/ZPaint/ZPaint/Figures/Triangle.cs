using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ZPaint
{
    [Serializable]
    public class Triangle : Polygon
    {
        public Triangle(Factory factory, SolidColorBrush color, int thickness, Point point1, Point point2) : base(factory, color, thickness, point1, point2)
        { }

        protected Triangle(SerializationInfo info, StreamingContext context) : base(info, context)
        { }

        public override Point[] DrawPolygon()
        {
            Point[] triangle = new Point[3]
            {
                new Point((point2.X - point1.X) / 2, 0),
                new Point(point2.X - point1.X, point2.Y - point1.Y),
                new Point(0, point2.Y - point1.Y)
            };
            points = triangle;
            return triangle;
        }
    }
}
