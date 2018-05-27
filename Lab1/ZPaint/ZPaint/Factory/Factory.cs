using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ZPaint
{
    abstract public class Factory
    {
        protected Type factoryType;
        public abstract Shape Create(SolidColorBrush color, int thickness, Point point1, Point point2);
        public virtual string PluginName()
        {
            return "N/A";
        }
    }

    class FactoryLine : Factory
    {
        public override Shape Create(SolidColorBrush color, int thickness, Point point1, Point point2)
        {
            factoryType = typeof(FactoryLine);
            return new Line(factoryType, color, thickness, point1, point2);
        }
    }

    class FactorySquare : Factory
    {
        public override Shape Create(SolidColorBrush color, int thickness, Point point1, Point point2)
        {
            factoryType = typeof(FactorySquare);
            return new Square(factoryType, color, thickness, point1, point2);
        }
    }

    class FactoryRectangle : Factory
    {
        public override Shape Create(SolidColorBrush color, int thickness, Point point1, Point point2)
        {
            factoryType = typeof(FactoryRectangle);
            return new Rectangle(factoryType, color, thickness, point1, point2); 
        }
    }

    class FactoryCircle : Factory
    {
        public override Shape Create(SolidColorBrush color, int thickness, Point point1, Point point2)
        {
            factoryType = typeof(FactoryCircle);
            return new Circle(factoryType, color, thickness, point1, point2);
        }
    }

    class FactoryEllipse : Factory
    {
        public override Shape Create(SolidColorBrush color, int thickness, Point point1, Point point2)
        {
            factoryType = typeof(FactoryEllipse);
            return new Ellipse(factoryType, color, thickness, point1, point2);
        }
    }

    class FactoryTriangle : Factory
    {
        public override Shape Create(SolidColorBrush color, int thickness, Point point1, Point point2)
        {
            factoryType = typeof(FactoryTriangle);
            return new Triangle(factoryType, color, thickness, point1, point2);
        }
    }

    class FactoryHexagon : Factory
    {
        public override Shape Create(SolidColorBrush color, int thickness, Point point1, Point point2)
        {
            factoryType = typeof(FactoryHexagon);
            return new Hexagon(factoryType, color, thickness, point1, point2);
        }
    }
}
