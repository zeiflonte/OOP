using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace ZPaint
{
    [Serializable]
    public abstract class Shape : ISerializable//, INotifyPropertyChanged
    {
        public System.Windows.Shapes.Shape figure;

        public Factory factory;

        public Point point1;

        public Point point2;

        public int thickness;

        public SolidColorBrush actualColor;

        public SolidColorBrush color;

        public SolidColorBrush selectionColor = Brushes.Pink;

        public event PropertyChangedEventHandler SelectionChanged;

        private bool isSelected;

        // Contains a selected/unselected state of a particular figure
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
                OnSelectionChanged(isSelected);
            }
        }

        // Choose a proper color for a selected/unselected figure
        protected void OnSelectionChanged(bool value)
        {
            if (value)
            {
                actualColor = selectionColor;
            }
            else
            {
                actualColor = color;
            }
        }


        protected Point[] points;

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

        public Shape(Factory factory, SolidColorBrush color, int thickness, Point point1, Point point2)
        {
            this.factory = factory;

            figure = DrawFigure();

            SetParameters(color, thickness, point1, point2); 
        }

        protected Shape(SerializationInfo info, StreamingContext context)
        {
            this.color = (SolidColorBrush)info.GetValue("color", typeof(SolidColorBrush));
            this.thickness = (int)info.GetValue("thickness", typeof(int));
            this.point1 = (Point)info.GetValue("point1", typeof(Point));
            this.point2 = (Point)info.GetValue("point2", typeof(Point));
            this.factory = (Factory)info.GetValue("factory", typeof(Factory));

            figure = DrawFigure();
            SetParameters(color, thickness, point1, point2);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("color", color);
            info.AddValue("thickness", thickness);
            info.AddValue("point1", point1);
            info.AddValue("point2", point2);
            info.AddValue("factory", factory);
        }

        public virtual void SetParameters(SolidColorBrush color, int thickness, Point point1, Point point2)
        {
            SetColor(color);

            SetThickness(thickness);

            SetPoints(point1, point2);

            SetScales();

            SetPosition();
        }

        public void SetColor(SolidColorBrush color)
        {
            this.color = color;
            actualColor = color;
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

        protected virtual void SetXY(SerializationInfo info)
        { }

        public void DrawInCanvas(Canvas canvas)
        {
            figure.Stroke = color;
            figure.StrokeThickness = this.thickness;
            canvas.Children.Add(figure);
        }
         
        public abstract System.Windows.Shapes.Shape DrawFigure();
    }
}
