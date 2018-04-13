using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ZPaint
{
    class ListFigures
    {
        private List<Shape> list;

        public ListFigures()
        {
            list = new List<Shape>();
        }

        public void Add(Shape figure)
        {
            list.Add(figure);
        }

        public void Remove(Shape figure)
        {
            list.Remove(figure);
        }
    
        public void Clear()
        {
            list.Clear();
        }

        public void Draw(Canvas canvas)
        {
            canvas.Children.Clear();
            foreach (Shape figure in list)
            {

                // Settings for a canvas

                figure.figure.Stroke = figure.color;
                figure.figure.StrokeThickness = figure.thickness;

                // Draw a figure on the canvas

                canvas.Children.Add(figure.figure);
            }
        }
    }
}
