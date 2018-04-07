using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ZPaint
{
    public abstract class Shape
    {
        public System.Windows.Shapes.Shape figure;

        //protected Point point1;
        //protected Point point2;
      
        private Color color;

        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
            }
        }

        public double Height
        {
            get
            {
                return figure.Height;
            }
            set
            {
                figure.Height = value;
            }
        }

        public double Width
        {
            get
            {
                return figure.Height;
            }
            set
            {
                figure.Width = value;
            }
        }

        public Shape(Point point1, Point point2)
        {
            Height = Math.Abs(point1.X - point2.X);
            Width = Math.Abs(point1.Y - point2.Y);
            figure = DrawFigure();
        }

        public void DrawInCanvas(Canvas canvas)
        {
            canvas.Children.Add(figure);
        }

        protected void SetScales(Point point1, Point point2)
        {
            //Height = point1.X - point2.X;
            //Width = point1.Y - point2.Y;
        }
         
        public abstract System.Windows.Shapes.Shape DrawFigure();
    }
}
