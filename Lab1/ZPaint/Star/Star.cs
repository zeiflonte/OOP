using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Star;
using Plugins;
using System.Windows;
using System.Windows.Media;
using ZPaint;

namespace Star
{
    public class Star : IPlugin
    {


       /* protected override void SetScales()
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

        protected override Point[] DrawPolygon()
        {
            Point[] hexagon = new Point[]
            {
                new Point(0.5 * Width, 0),
                new Point(Width, 0.25 * Height),
                new Point(Width, 0.75 * Height),
                new Point(0.5 * Width, Height),
                new Point(0, 0.75 * Height),
                new Point(0, 0.25 * Height),
            };
            points = hexagon;
            return hexagon;
        } */

        public string Name()
        {      
            return "Star";    
        }
    }
}
