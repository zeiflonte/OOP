using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ZPaint
{
    public class Line : Shape
    {
        public double X1
        {
            get
            {
                return ((System.Windows.Shapes.Line)figure).X1;
            }
            set
            {
                ((System.Windows.Shapes.Line)figure).X1 = value;
            }
        }
        public double X2
        {
            get
            {
                return ((System.Windows.Shapes.Line)figure).X2;
            }
            set
            {
                ((System.Windows.Shapes.Line)figure).X2 = value;
            }
        }
        public double Y1
        {
            get
            {
                return ((System.Windows.Shapes.Line)figure).Y1;
            }
            set
            {
                ((System.Windows.Shapes.Line)figure).Y1 = value;
            }
        }
        public double Y2
        {
            get
            {
                return ((System.Windows.Shapes.Line)figure).Y2;
            }
            set
            {
                ((System.Windows.Shapes.Line)figure).Y2 = value;
            }
        }

        public Line(SolidColorBrush color, int thickness, Point point1, Point point2) : base(color, thickness, point1, point2)
        { }

        public override void SetParameters(SolidColorBrush color, int thickness, Point point1, Point point2)
        {
            this.point1 = point1;
            this.point2 = point2;

            X1 = this.point1.X;
            Y1 = this.point1.Y;
            X2 = this.point2.X;
            Y2 = this.point2.Y;

            this.thickness = thickness;
            this.color = color;
        }

        public override System.Windows.Shapes.Shape DrawFigure()
        {
            return new System.Windows.Shapes.Line();
        }
    }
}
