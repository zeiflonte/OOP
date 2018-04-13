using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ZPaint
{
    public class Square : Rectangle
    {
        public Square(int thickness, Point point1, Point point2) : base(thickness, point1, point2)
        { }

        protected override void SetScales(int thickness)
        {
            this.thickness = thickness;
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
