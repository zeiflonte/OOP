﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Runtime.Serialization;

namespace ZPaint
{
    [DataContract]
    public abstract class Shape
    {
        public System.Windows.Shapes.Shape figure;

        [DataMember]
        private Type factoryType; 
        [DataMember]
        public Point point1;
        [DataMember]
        public Point point2;
        [DataMember]
        public int thickness;
        [DataMember]
        public SolidColorBrush color;
        
        protected Point[] points;

        public Type FactoryType
        {
            get
            {
                return factoryType;
            }
            set
            {
                factoryType = value;
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
                return figure.Width;
            }
            set
            {
                figure.Width = value;
            }
        }

        public Shape(Type factoryType, SolidColorBrush color, int thickness, Point point1, Point point2)
        {
            figure = DrawFigure();

            SetFactoryType(factoryType);

            SetParameters(color, thickness, point1, point2); 
        }

        public virtual void SetParameters(SolidColorBrush color, int thickness, Point point1, Point point2)
        {
            SetColor(color);

            SetThickness(thickness);

            SetPoints(point1, point2);

            SetScales();

            SetPosition();
        }

        protected virtual void SetFactoryType(Type _factoryType)
        {
            factoryType = _factoryType;
        }

        public void SetColor(SolidColorBrush color)
        {
            this.color = color;
        }

        public void SetThickness(int thickness)
        {
            this.thickness = thickness;
        }

        private void SetPoints(Point point1, Point point2)
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

        protected virtual void SetScales()
        {
            Width = Math.Abs(point1.X - point2.X);
            Height = Math.Abs(point1.Y - point2.Y);
        }

        private void SetPosition()
        {
            Canvas.SetLeft(figure, point1.X);
            Canvas.SetTop(figure, point1.Y);
        }

        public void DrawInCanvas(Point point1, Point point2, Canvas canvas)
        {
            figure.Stroke = color;
            figure.StrokeThickness = this.thickness;
            canvas.Children.Add(figure);
        }
         
        public abstract System.Windows.Shapes.Shape DrawFigure();
    }
}