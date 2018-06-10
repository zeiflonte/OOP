using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using ZPaint;

namespace Pentagon
{
    public class FactoryPentagon : Factory, IPluginFactory
    {
        public override Shape Create(Factory factory, SolidColorBrush color, int thickness, Point point1, Point point2)
        {
            return new Pentagon(factory, color, thickness, point1, point2);
        }

         public override string PluginName(string _culture)
         {
            CultureInfo culture = CultureInfo.CreateSpecificCulture(_culture);
            ResourceManager rm = new ResourceManager("Pentagon.locale", typeof(FactoryPentagon).Assembly);
            return rm.GetString("PluginName", culture);
        } 
    }

    [Serializable]
    public class Pentagon : Polygon, IPluginFigure
    {
        public Pentagon(Factory factory, SolidColorBrush color, int thickness, Point point1, Point point2) : base(factory, color, thickness, point1, point2)
        { }

        protected Pentagon(SerializationInfo info, StreamingContext context) : base(info, context)
        { }

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
            Point[] pentagon = new Point[]
            {
                new Point(0.5 * Width, 0),
                new Point(Width, 0.40 * Height),
                new Point(0.85 * Width, Height),
                new Point(0.15 * Width, Height),
                new Point(0, 0.40 * Height),
            };
            points = pentagon;
            return pentagon;
        }
    }
}
