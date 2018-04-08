using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ZPaint
{
    public class Ellipse : Shape
    {
        public Ellipse(Point point1, Point point2) : base(point1, point2)
        { }

        public override System.Windows.Shapes.Shape DrawFigure()
        {
            return new System.Windows.Shapes.Ellipse();
        }
    }
}
