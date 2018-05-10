using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ZPaint
{
    public interface IPluginFactory
    {
        Shape Create(SolidColorBrush color, int thickness, Point point1, Point point2);
    }
}
