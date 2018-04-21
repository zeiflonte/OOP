using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ZPaint
{
    [Serializable]
    public abstract class Polygon : Shape
    {
        protected Polygon(SolidColorBrush color, int thickness, Point point1, Point point2) : base(color, thickness, point1, point2)
        {
            ((System.Windows.Shapes.Polygon)figure).Points = new PointCollection(DrawPolygon());
        }

        public override System.Windows.Shapes.Shape DrawFigure()
        {
            return new System.Windows.Shapes.Polygon();
        }

        protected abstract Point[] DrawPolygon();
    }
}