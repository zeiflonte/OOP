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
    abstract public class Factory
    {
        public abstract Shape Create(Factory factory, SolidColorBrush color, int thickness, Point point1, Point point2);
        public virtual string PluginName(string culture)
        {
            return "N/A";
        }
    }

    [Serializable]
    class FactoryLine : Factory
    {
        public override Shape Create(Factory factory, SolidColorBrush color, int thickness, Point point1, Point point2)
        {
            return new Line(factory, color, thickness, point1, point2);
        }
    }

    [Serializable]
    class FactorySquare : Factory
    {
        public override Shape Create(Factory factory, SolidColorBrush color, int thickness, Point point1, Point point2)
        {
            return new Square(factory, color, thickness, point1, point2);
        }
    }

    [Serializable]
    class FactoryRectangle : Factory
    {
        public override Shape Create(Factory factory, SolidColorBrush color, int thickness, Point point1, Point point2)
        {
            return new Rectangle(factory, color, thickness, point1, point2); 
        }
    }

    [Serializable]
    class FactoryCircle : Factory
    {
        public override Shape Create(Factory factory, SolidColorBrush color, int thickness, Point point1, Point point2)
        {
            return new Circle(factory, color, thickness, point1, point2);
        }
    }

    [Serializable]
    class FactoryEllipse : Factory
    {
        public override Shape Create(Factory factory, SolidColorBrush color, int thickness, Point point1, Point point2)
        {
            return new Ellipse(factory, color, thickness, point1, point2);
        }
    }

    [Serializable]
    class FactoryTriangle : Factory
    {
        public override Shape Create(Factory factory, SolidColorBrush color, int thickness, Point point1, Point point2)
        {
            return new Triangle(factory, color, thickness, point1, point2);
        }
    }

    [Serializable]
    class FactoryHexagon : Factory
    {
        public override Shape Create(Factory factory, SolidColorBrush color, int thickness, Point point1, Point point2)
        {
            return new Hexagon(factory, color, thickness, point1, point2);
        }
    }
}
