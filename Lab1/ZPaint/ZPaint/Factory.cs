using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ZPaint
{
    abstract class Factory
    {
        public abstract Shape Create(int thickness, Point point1, Point point2);
    }

    class FactoryLine : Factory
    {
        public override Shape Create(int thickness, Point point1, Point point2)
        {
            return new Line(thickness, point1, point2);
        }
    }

    class FactorySquare : Factory
    {
        public override Shape Create(int thickness, Point point1, Point point2)
        {
            return new Square(thickness, point1, point2);
        }
    }

    class FactoryRectangle : Factory
    {
        public override Shape Create(int thickness, Point point1, Point point2)
        {
            return new Rectangle(thickness, point1, point2); 
        }
    }

    class FactoryCircle : Factory
    {
        public override Shape Create(int thickness, Point point1, Point point2)
        {
            return new Circle(thickness, point1, point2);
        }
    }

    class FactoryEllipse : Factory
    {
        public override Shape Create(int thickness, Point point1, Point point2)
        {
            return new Ellipse(thickness, point1, point2);
        }
    }

    class FactoryTriangle : Factory
    {
        public override Shape Create(int thickness, Point point1, Point point2)
        {
            return new Triangle(thickness, point1, point2);
        }
    }

    class FactoryHexagon : Factory
    {
        public override Shape Create(int thickness, Point point1, Point point2)
        {
            return new Hexagon(thickness, point1, point2);
        }
    }
}
