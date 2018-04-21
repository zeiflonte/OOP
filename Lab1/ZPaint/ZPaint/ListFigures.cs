using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Runtime.Serialization.Json;
using System.IO;
using Newtonsoft.Json;

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

        public int Count()
        {
            return list.Count();
        }

        public void Serialize(JsonSerializer jsonSerializer, StreamWriter stream, JsonWriter writer)
        {
            for (int i = 0; i < list.Count(); i++)
            {
                jsonSerializer.Serialize(writer, list[i]);
                if (i != list.Count() - 1)
                {
                    stream.Write("\n");
                }
            }
        }

        public void Deserialize(DataContractJsonSerializer jsonSerializer, FileStream stream)
        {
            list.Add((Shape)jsonSerializer.ReadObject(stream));
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
