﻿using System;
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

        protected Point point1;
        protected Point point2;
      
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
            figure = DrawFigure();

            SetPoints(point1, point2);

            SetScales();

            SetPosition();


            
        }

        protected void SetPoints(Point point1, Point point2)
        {
            double height = Math.Abs(point1.Y - point2.Y);
            double width = Math.Abs(point1.X - point2.X);

            this.point1 = point1;
            this.point2 = point2;

            if (point2.X > point1.X)
            {
                if (point2.Y < point1.Y)
                {
                    this.point1.Y -= height;
                    this.point2.Y += height;
                }
            }
            else
            {
                if (point2.Y < point1.Y)
                {
                    this.point2 = point1;
                    this.point1 = point2;
                }
                else
                {
                    this.point1.X -= width;
                    this.point2.X += width;
                }
            }           
        }

        protected void SetScales()
        {
            Width = Math.Abs(point1.X - point2.X);
            Height = Math.Abs(point1.Y - point2.Y);
        }

        public void SetPosition()
        {
            Canvas.SetLeft(figure, point1.X);
            Canvas.SetTop(figure, point1.Y);
        }

        public void DrawInCanvas(Point point1, Point point2, Canvas canvas)
        {
            figure.Stroke = Brushes.Black;
            canvas.Children.Add(figure);
        }
         
        public abstract System.Windows.Shapes.Shape DrawFigure();
    }
}
