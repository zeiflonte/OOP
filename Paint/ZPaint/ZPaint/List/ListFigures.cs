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

namespace ZPaint
{
    class ListFigures
    {
        public List<List<Shape>> list;

        public ListFigures()
        {
            list = new List<List<Shape>>();
        }

        public void Add(List<Shape> figure)
        {
            list.Add(figure);
        }

        public void Remove(List<Shape> figure)
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

        // ???
        public new Type GetType()
        {
            return list.GetType();
        }

        public void Serialize(DataContractJsonSerializer jsonSerializer, FileStream stream)
        {
            /*foreach (var shape in list)
            {
                jsonSerializer.WriteObject(stream, shape);
            }*/
            jsonSerializer.WriteObject(stream, list);
        }

        public void Deserialize(DataContractJsonSerializer jsonSerializer, FileStream stream)
        {
            list = jsonSerializer.ReadObject(stream) as List<List<Shape>>;
        }

        public void Draw(Canvas canvas)
        {
            canvas.Children.Clear();
            foreach (List<Shape> listFigure in list)
            {
                foreach (Shape figure in listFigure)
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
}
