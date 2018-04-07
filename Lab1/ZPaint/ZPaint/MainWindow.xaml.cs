using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ZPaint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
  
    

    public partial class MainWindow : Window
    {
        private Shape shape;
        //private Factory factory;
        private Point point1;
        private Point point2;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void butRectangle_Click(object sender, RoutedEventArgs e)
        {
            Shape shape = new Rectangle(point1, point2);
            shape.DrawInCanvas(canvas);

        //    Canvas.SetLeft(shape, X);
        //    Canvas.SetTop(shape, Y);
        }

        private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Cursor = Cursors.Pen;
            point1 = e.GetPosition(canvas);
        }

        

        private void canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            point2 = e.GetPosition(canvas);
            Cursor = Cursors.Arrow;
        }

     
    }
}
