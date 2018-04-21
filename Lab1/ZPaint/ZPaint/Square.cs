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
    public class Square : Rectangle
    {
        public Square(Type factoryType, SolidColorBrush color, int thickness, Point point1, Point point2) : base(factoryType, color, thickness, point1, point2)
        {
            factoryType = typeof(FactoryRectangle);
        }

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
