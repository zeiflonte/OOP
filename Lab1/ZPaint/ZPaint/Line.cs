using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ZPaint
{
    public class Line : Shape
    {
/*        public new System.Windows.Shapes.Line figure;

        public double X1
        {
            get
            {
                return figure.X1;
            }
            set
            {
                figure.X1 = value;
            }
        }
        public double X2
        {
            get
            {
                return figure.X2;
            }
            set
            {
                figure.X2 = value;
            }
        }
        public double Y1
        {
            get
            {
                return figure.Y1;
            }
            set
            {
                figure.Y1 = value;
            }
        }
        public double Y2
        {
            get
            {
                return figure.Y2;
            }
            set
            {
                figure.Y2 = value;
            }
        }   */

        public Line(Point point1, Point point2) : base(point1, point2)
        {
            
            figure.StrokeThickness = 2;
        }

        public override System.Windows.Shapes.Shape DrawFigure()
        {
            return new System.Windows.Shapes.Line();
        }
    }
}
