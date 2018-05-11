using System.Windows;
using System.Windows.Media;

namespace ZPaint
{
    public interface IPluginFactory
    {
        string PluginName();
        Shape Create(SolidColorBrush color, int thickness, Point point1, Point point2);
    }
}
