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
    [DataContract]
    public class Rectangle : Shape
    {
        public Rectangle(SolidColorBrush color, int thickness, Point point1, Point point2) : base(color, thickness, point1, point2)
        { }

        public override System.Windows.Shapes.Shape DrawFigure()
        {
            return new System.Windows.Shapes.Rectangle();
        }
    }
}
