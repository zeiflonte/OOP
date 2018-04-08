﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ZPaint
{
    abstract class Factory
    {
        public abstract Shape Create(Point point1, Point point2);
    }

    class FactoryRectangle : Factory
    {
        public override Shape Create(Point point1, Point point2)
        {
            return new Rectangle(point1, point2); 
        }
    }

    class FactoryEllipse : Factory
    {
        public override Shape Create(Point point1, Point point2)
        {
            return new Ellipse(point1, point2);
        }
    }
}