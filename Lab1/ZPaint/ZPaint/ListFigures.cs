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

        public void Clear()
        {
            list.Clear();
        }

        public void Draw(Canvas canvas)
        {
            foreach (Shape figure in list)
            {
                figure.figure.Stroke = Brushes.Black;
                canvas.Children.Add(figure.figure);
            }
        }
    }
}
