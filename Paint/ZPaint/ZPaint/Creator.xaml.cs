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
using System.Windows.Shapes;

namespace ZPaint
{
    /// <summary>
    /// Interaction logic for Creator.xaml
    /// </summary>
    public partial class Creator : Window
    {
        public static List<Shape> listShape { get; set; }

        public static string shapeName { get; set; }

        private Shape shape;
        private SolidColorBrush exColor;

        private Factory factory;
        //List<Shape> listExShape;

        // Initial figure properties

        private Point point1;
        private Point point2;
        private int thickness;
        private SolidColorBrush color;

        Dictionary<string, Factory> Plugins;
        Dictionary<string, List<Shape>> UserShapes;

        public event EventHandler SubmitClicked;

        private void cbitCursor_Selected(object sender, RoutedEventArgs e)
        {
            factory = null;
        }

        private void cbitLine_Selected(object sender, RoutedEventArgs e)
        {
            factory = new FactoryLine();
        }

        private void cbitRectangle_Selected(object sender, RoutedEventArgs e)
        {
            factory = new FactoryRectangle();
        }

        private void cbitSquare_Selected(object sender, RoutedEventArgs e)
        {
            factory = new FactorySquare();
        }

        private void cbitOval_Selected(object sender, RoutedEventArgs e)
        {
            factory = new FactoryEllipse();
        }

        private void cbitCircle_Selected(object sender, RoutedEventArgs e)
        {
            factory = new FactoryCircle();
        }

        private void cbitTriangle_Selected(object sender, RoutedEventArgs e)
        {
            factory = new FactoryTriangle();
        }

        private void cbitHexagon_Selected(object sender, RoutedEventArgs e)
        {
            factory = new FactoryHexagon();
        }

        public Creator(Dictionary<string, Factory> Plugins, Dictionary<string, List<Shape>> UserShapes)
        {
            this.Plugins = Plugins;
            this.UserShapes = UserShapes;

            InitializeComponent();
            this.Height = 299;

            listShape = new List<Shape>();
        }

        private void cbThickness_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Choose a thickness of a figure

            var dic = new Dictionary<String, int>();
            dic.Add("Thin", 1);
            dic.Add("Тонкий", 1);
            dic.Add("Medium", 2);
            dic.Add("Средний", 2);
            dic.Add("Thick", 3);
            dic.Add("Толстый", 3);
            string selectedValue;
            try
            {
                selectedValue = (string)((ComboBoxItem)cbThickness.SelectedItem).Content;
            }
            catch (NullReferenceException)
            {
                cbThickness.SelectedIndex = 0;
                selectedValue = (string)((ComboBoxItem)cbThickness.SelectedItem).Content;
            }

            thickness = dic[selectedValue];

            // Change parameters of an already existing figure

            if (shape != null)
            {
                shape.SetThickness(thickness);
                Draw();
            }
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Choose a color of a figure

            var dic = new Dictionary<String, SolidColorBrush>();
            dic.Add("Black", Brushes.Black);
            dic.Add("Чёрный", Brushes.Black);
            dic.Add("Blue", Brushes.Blue);
            dic.Add("Синий", Brushes.Blue);
            dic.Add("Red", Brushes.Red);
            dic.Add("Красный", Brushes.Red);
            string selectedValue;
            try
            {
                selectedValue = (string)((ComboBoxItem)cbColor.SelectedItem).Content;
            }
            catch (NullReferenceException)
            {
                cbColor.SelectedIndex = 0;
                selectedValue = (string)((ComboBoxItem)cbColor.SelectedItem).Content;
            }
            color = dic[selectedValue];

            // Change parameters of an already existing figure

            if (shape != null)
            {
                shape.SetColor(color);
                exColor = shape.color;
                Draw();
            }
        }

        private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Save the first position

            point1 = e.GetPosition(canvas);

            // Set a cursor type

            if (factory != null)
            {
                Cursor = Cursors.Pen;
            }
            else
            {
                Cursor = Cursors.Arrow;
            }
        }

        private void canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (factory != null)
            {
                // Save the second position

                point2 = e.GetPosition(canvas);

                // Create a new figure

                shape = factory.Create(factory, color, thickness, point1, point2);
                listShape.Add(shape);
                //list.Add(listShape);
                //listShapes.Items.Add(listShape);
                shape.DrawInCanvas(canvas);

                // Reset initial settings

                shape = null;
                Cursor = Cursors.Arrow;
            }
        }

        private void cbFactory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var plugin in Plugins)
            {
                // Set selected custom factory
                if (plugin.Key == (String)((ComboBoxItem)cbFactory.SelectedItem).Content)
                {
                    factory = plugin.Value;
                    break;
                }
            }
        }

        private void Draw()
        {
            canvas.Children.Clear();
            foreach (Shape figure in listShape)
            {
                // Settings for a canvas

                figure.figure.Stroke = figure.color;
                figure.figure.StrokeThickness = figure.thickness;

                // Draw a figure on the canvas

                canvas.Children.Add(figure.figure);
            }
        }

        private void butSave_Click(object sender, RoutedEventArgs e)
        {
            bool isEmpty = !listShape.Any();
            if (isEmpty)
            {
                MessageBox.Show("The field is empty");
                return;
            }
            this.Height = 332;
            butSave.Visibility = Visibility.Hidden;
        }

        private void butApply_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtName.Text))
            {
                foreach (var plugin in Plugins)
                {
                    if (plugin.Key == txtName.Text)
                    {
                        MessageBox.Show("The name is already in use");
                        return;
                    }
                }
                foreach (var shapeName in UserShapes)
                {
                    if (shapeName.Key == txtName.Text)
                    {
                        MessageBox.Show("The name is already in use");
                        return;
                    }
                }
                shapeName = txtName.Text;
            }
            else
            {
                MessageBox.Show("The name is empty");
                return;
            }
            if (SubmitClicked != null)
            {
                SubmitClicked(this, new EventArgs());
            }
            canvas.Children.Clear();
            this.DialogResult = true;
            this.Close();
        }
    }
}
