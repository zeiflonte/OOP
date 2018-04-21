using Microsoft.Win32;
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
using System.IO;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace ZPaint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private Shape shape;
        private Shape exShape;
        private SolidColorBrush exColor;

        private Factory factory;
        private ListFigures list = new ListFigures();

        // Initial figure properties

        private Point point1;
        private Point point2;
        private int thickness;
        private SolidColorBrush color;

        public MainWindow()
        {
            InitializeComponent();
        }

        // Choose a type of a figure

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

                shape = factory.Create(color, thickness, point1, point2);
                list.Add(shape);
                listShapes.Items.Add(shape);
                shape.DrawInCanvas(point1, point2, canvas);

                // Reset initial settings

                shape = null;
                Cursor = Cursors.Arrow;
            }
        }

        private void cbThickness_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Choose a thickness of a figure

            if ((cbThickness.SelectedIndex == -1) || (cbThickness.SelectedIndex == 0))
            {
                thickness = 1;
            }
            if (cbThickness.SelectedIndex == 1)
            {
                thickness = 2;
            }
            if (cbThickness.SelectedIndex == 2)
            {
                thickness = 3;
            }

            // Change parameters of an already existing figure

            if (shape != null)
            {
                shape.SetThickness(thickness);
                list.Draw(canvas);
            }
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            // Choose a color of a figure
            var dic = new Dictionary<String, SolidColorBrush>();
            dic.Add("Black", Brushes.Black);

            dic.Add("Blue", Brushes.Blue);

            dic.Add("Red", Brushes.Red);
            String selectedValue = (String)((ComboBoxItem)cbColor.SelectedItem).Content;
            color = dic[selectedValue];
            //if ((cbColor.SelectedIndex == -1) || (cbColor.SelectedIndex == 0))
            //{
            //    color = Brushes.Black;
            //}
            //if (cbColor.SelectedIndex == 1)
            //{
            //    color = Brushes.Blue;
            //}
            //if (cbColor.SelectedIndex == 2)
            //{
            //    color = Brushes.Red;
            //}

            // Change parameters of an already existing figure

            if (shape != null)
            {
                shape.SetColor(color);
                exColor = shape.color;
                list.Draw(canvas);
            }
        }

        private void listShapes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Restore a previous color

            if (exShape != null)
            {
                exShape.color = exColor;
            }
            shape = listShapes.SelectedItem as Shape;

            // Save the previous color

            if (shape != null)
            {
                exShape = shape;
                exColor = shape.color;

                // Illuminate a selected figure

                shape.color = Brushes.Pink;
            }

            list.Draw(canvas);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Delete) && (listShapes.SelectedIndex != -1))
            {
                // Delete a selected figure

                list.Remove(shape);
                listShapes.Items.Remove(listShapes.SelectedItem);
            }
        }

        // Temporary structure aimed to hold data from .json file entries

        struct ShapeImage
        {
            public Type FactoryType;
            public Point point1;
            public Point point2;
            public int thickness;
            public SolidColorBrush color;
            
        }

        private void butSave_Click(object sender, RoutedEventArgs e)
        {
            // Open a save file dialogue

            SaveFileDialog fileSave = new SaveFileDialog
            {
                Filter = "JSON File (*.json)|*.json|All Files (*.*)|*.*",
                RestoreDirectory = true,
            };
            if (fileSave.ShowDialog() == true)
            {
                // Serialization of figures

                JsonSerializer jsonSerializer = new JsonSerializer();

                string filename = fileSave.FileName;

                using (StreamWriter stream = new StreamWriter(filename))
                {
                    using (JsonWriter writer = new JsonTextWriter(stream))
                    {
                        list.Serialize(jsonSerializer, stream, writer);
                    }
                }
            }

            fileSave = null;
        }

        private void DeleteFigures()
        {
            list.Clear();
            listShapes.Items.Clear();
            canvas.Children.Clear();
        }

        private void butLoad_Click(object sender, RoutedEventArgs e)
        {
            DeleteFigures();

            // Open a load file dialogue

            OpenFileDialog fileOpen = new OpenFileDialog
            {
                Filter = "JSON File (*.json)|*.json|All Files (*.*)|*.*",
                RestoreDirectory = true,
            };
            if (fileOpen.ShowDialog() == true)
            {
                // Deserialization of figures

                JsonSerializer jsonSerializer = new JsonSerializer();

                string fileName = fileOpen.FileName;

                using (StreamReader stream = new StreamReader(fileName))
                {
                    string data = stream.ReadToEnd();
                    {
                        string[] dataArray = data.Split('\n');
                        foreach (string dataBlock in dataArray)
                        {
                            try
                            {
                                ShapeImage image = JsonConvert.DeserializeObject<ShapeImage>(dataBlock, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include });

                                // Create a figure

                                Factory factory = (Factory)Activator.CreateInstance(image.FactoryType);
                                shape = factory.Create(image.color, image.thickness, image.point1, image.point2);
                                list.Add(shape);
                                listShapes.Items.Add(shape);
                                shape.DrawInCanvas(point1, point2, canvas);
                                shape = null;
                            }
                            catch
                            {
                                continue;
                            }
                        }
                    }
                }

                fileOpen = null;
            }
        }
    }
}
