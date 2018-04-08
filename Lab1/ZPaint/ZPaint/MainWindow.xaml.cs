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
        private Factory factory;

        private Point point1;
        private Point point2;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void butCursor_Click(object sender, RoutedEventArgs e)
        {
            factory = null;
        }

        private void butLine_Click(object sender, RoutedEventArgs e)
        {
            factory = new FactoryLine();
        }

        private void butSquare_Click(object sender, RoutedEventArgs e)
        {
            factory = new FactorySquare();
        }

        private void butRectangle_Click(object sender, RoutedEventArgs e)
        {
            factory = new FactoryRectangle();
        }

        private void butCircle_Click(object sender, RoutedEventArgs e)
        {
            factory = new FactoryCircle();
        }

        private void butEllipse_Click(object sender, RoutedEventArgs e)
        {
            factory = new FactoryEllipse();
        }

        private void butTriangle_Click(object sender, RoutedEventArgs e)
        {
            factory = new FactoryTriangle();
        }

        private void butHexagon_Click(object sender, RoutedEventArgs e)
        {
            factory = new FactoryHexagon();
        }

        private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (factory != null)
            {
                Cursor = Cursors.Pen;
            }
            point1 = e.GetPosition(canvas);
        }

        private void canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            point2 = e.GetPosition(canvas);

            if (factory != null)
            {
                shape = factory.Create(point1, point2);
                shape.DrawInCanvas(point1, point2, canvas);
            }

            Cursor = Cursors.Arrow;
        }

        private void butLab1_Click(object sender, RoutedEventArgs e)
        {
            ListFigures list = new ListFigures();
            list.Add(new Line(new Point(50, 50), new Point(50, 200)));
            list.Add(new Rectangle(new Point(60, 50), new Point(150, 200)));
            list.Add(new Ellipse(new Point(170, 50), new Point(300, 200)));
            list.Add(new Square(new Point(310, 50), new Point(400, 200)));
            list.Add(new Circle(new Point(500, 50), new Point(600, 200)));
            list.Add(new Triangle(new Point(680, 50), new Point(780, 200)));
            list.Add(new Hexagon(new Point(800, 50), new Point(900, 200)));       
            list.Draw(canvas);
        }
    }
}
