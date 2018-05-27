﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ZPaint
{
    public class Triangle : Polygon
    {
        public Triangle(Type factoryType, SolidColorBrush color, int thickness, Point point1, Point point2) : base(factoryType, color, thickness, point1, point2)
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