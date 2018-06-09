using System.Windows;
using System.Windows.Media;

namespace ZPaint
{
    public interface IPluginFactory
    {
        string PluginName(string culture);
        Shape Create(Factory factory, SolidColorBrush color, int thickness, Point point1, Point point2);
    }
}
