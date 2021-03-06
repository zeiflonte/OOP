﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ZPaint
{
    [Serializable]
    public class Square : Rectangle
    {
        public Square(Factory factory, SolidColorBrush color, int thickness, Point point1, Point point2) : base(factory, color, thickness, point1, point2)
        {
        }

        protected Square(SerializationInfo info, StreamingContext context) : base(info, context)
        { }

        protected override void SetScales()
        {
            Height = Math.Abs(point1.Y - point2.Y);
            Width = Math.Abs(point1.X - point2.X);
            if (Width > Height)
            {
                Height = Width;
            }
            else
            {
                Width = Height;
            }
        }
    }
}
