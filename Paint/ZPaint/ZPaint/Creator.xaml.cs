using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
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
using ZPaint.List;

namespace ZPaint
{
    /// <summary>
    /// Interaction logic for Creator.xaml
    /// </summary>
    public partial class Creator : Window
    {
        public static List<Shape> listShape { get; set; }
        List<Shape> userShape;

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

        public Creator(Dictionary<string, Factory> Plugins, Dictionary<string, List<Shape>> UserShapes, string locale)
        {
            this.Plugins = Plugins;
            this.UserShapes = UserShapes;

            InitializeComponent();
            this.Height = 299;
            SetLocale(locale);

            Tools tools = new Tools(Plugins, UserShapes, locale);
            tools.FactorySelected += new EventHandler(tools_FactorySelected);
            tools.DrawingToolsLoad(ref cbFactory);

            listShape = new List<Shape>();
        }

        private void SetLocale(string _culture)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture(_culture);
            ResourceManager rm = new ResourceManager("ZPaint.locale", typeof(MainWindow).Assembly);
            cbitThin.Content = rm.GetString("cbitThin", culture);
            cbitMedium.Content = rm.GetString("cbitMedium", culture);
            cbitThick.Content = rm.GetString("cbitThick", culture);
            cbitBlack.Content = rm.GetString("cbitBlack", culture);
            cbitBlue.Content = rm.GetString("cbitBlue", culture);
            cbitRed.Content = rm.GetString("cbitRed", culture);
            butSave.Content = rm.GetString("butSave", culture);
            butApply.Content = rm.GetString("butApply", culture);
        }

        private void tools_FactorySelected(object sender, EventArgs e)
        {
            factory = Tools.factory;
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
            else
            {
                if (userShape != null)
                {
                    // Save the second position

                    point2 = e.GetPosition(canvas);

                    // Create a new figure

                    foreach (Shape tmp in userShape)
                    {
                        Point actualPoint1 = new Point();
                        Point actualPoint2 = new Point();
                        actualPoint1.X = point1.X + tmp.point1.X;
                        actualPoint1.Y = point1.Y + tmp.point1.Y;
                        actualPoint2.X = point1.X + tmp.point2.X;
                        actualPoint2.Y = point1.Y + tmp.point2.Y;

                        shape = tmp.factory.Create(tmp.factory, tmp.color, tmp.thickness, actualPoint1, actualPoint2);

                        listShape.Add(shape);
                        shape.DrawInCanvas(canvas);
                    }
                    // list.Add(tempList);
                    //  listShapes.Items.Add(tempList);


                    // Reset initial settings

                    shape = null;
                    Cursor = Cursors.Arrow;

                    //!!! listShape = null;
                }
            }
        }

        /*private void canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (factory != null)
            {
                // Save the second position

                point2 = e.GetPosition(canvas);

                // Create a new figure

                shape = factory.Create(factory, color, thickness, point1, point2);
                List<Shape> listShape = new List<Shape>();
                listShape.Add(shape);
                shape.DrawInCanvas(canvas);

                // Reset initial settings

                shape = null;
                Cursor = Cursors.Arrow;
            }
            else
            {
                if (listShape != null)
                {
                    // Save the second position

                    point2 = e.GetPosition(canvas);

                    // Create a new figure

                    List<Shape> tempList = new List<Shape>();

                    foreach (Shape tmp in listShape)
                    {
                        Point actualPoint1 = new Point();
                        Point actualPoint2 = new Point();
                        actualPoint1.X = point1.X + tmp.point1.X;
                        actualPoint1.Y = point1.Y + tmp.point1.Y;
                        actualPoint2.X = point1.X + tmp.point2.X;
                        actualPoint2.Y = point1.Y + tmp.point2.Y;

                        shape = tmp.factory.Create(tmp.factory, tmp.color, tmp.thickness, actualPoint1, actualPoint2);

                        tempList.Add(shape);
                        shape.DrawInCanvas(canvas);
                    }
                   // list.Add(tempList);
                  //  listShapes.Items.Add(tempList);


                    // Reset initial settings

                    shape = null;
                    Cursor = Cursors.Arrow;

                    //!!! listShape = null;
                }
            }
        }*/

        private void cbFactory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbFactory.Items.Count != 0)
            {
                foreach (var plugin in Plugins)
                {
                    // Set selected custom factory
                    if (plugin.Key == (String)((ComboBoxItem)cbFactory.SelectedItem).Content)
                    {
                        factory = plugin.Value;
                        userShape = null;
                        return;
                    }
                }
                foreach (var shape in UserShapes)
                {
                    // Set selected custom shape
                    if (shape.Key == (String)((ComboBoxItem)cbFactory.SelectedItem).Content)
                    {
                        userShape = shape.Value;
                        factory = null;
                        return;
                    }
                }
            }
            userShape = null;
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
