using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using ZPaint;

namespace Pentagon
{
    class FactoryPentagon : Factory, IPluginFactory
    {
        public override Shape Create(SolidColorBrush color, int thickness, Point point1, Point point2)
        {
            factoryType = typeof(FactoryPentagon);
            return new Star(factoryType, color, thickness, point1, point2);
        }

        /* public string PluginName()
         {
             return "⬠ Pentagon";
         } */
    }

    public class Star : Polygon, IPluginFigure
    {
        public Star(Type factoryType, SolidColorBrush color, int thickness, Point point1, Point point2) : base(factoryType, color, thickness, point1, point2)
        {
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

        public override Point[] DrawPolygon()
        {
            Point[] hexagon = new Point[]
            {
                new Point(0.5 * Width, 0),
                new Point(0.85 * Width, Height),
                new Point(0, 0.40 * Height),
                new Point(Width, 0.40 * Height),
                new Point(0.15 * Width, Height),
                //new Point(0, 0.25 * Height),
            };
            points = hexagon;
            return hexagon;
        }
    }
}
