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
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

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

        SolidColorBrush BackgroundColor
        {
            get
            {
                return (SolidColorBrush)canvas.Background;
            }
            set
            {
                canvas.Background = value;
            }
        }

        Dictionary<string, Factory> Plugins = new Dictionary<String, Factory>();
        private int amountOfPlugins = 0;

        public static string pluginsPath = "../../../Plugins";
        // public static string pluginsPath = "../../../Pentagon/bin/Debug";

        public MainWindow()
        {
            InitializeComponent();
            addPlugins();
            XMLLoad();
        }

        // Choose a type of a figure

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

            var dic = new Dictionary<String, int>();
            dic.Add("Thin", 1);

            dic.Add("Medium", 2);

            dic.Add("Thick", 3);
            String selectedValue = (String)((ComboBoxItem)cbThickness.SelectedItem).Content;
            thickness = dic[selectedValue];

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

        private void listShapes_LostFocus(object sender, RoutedEventArgs e)
        {
            if (exShape != null)
            {
                exShape.color = exColor;
                list.Draw(canvas);
            }
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

        Factory plugin;

      /*  private void deletePlugins()
        {
            var count = cbFactory.Items.Count - 1;
            for (int i = count; i >= count - amountOfPlugins + 1; i--)
            {
                cbFactory.Items.RemoveAt(i);
            }
            amountOfPlugins = 0;
        } */

        private void addPlugins()
        {
            // Create a plugin directory if it does not exist

            DirectoryInfo pluginsDirectory = new DirectoryInfo(pluginsPath);
            if (!pluginsDirectory.Exists)
            {
                pluginsDirectory.Create();
                pluginsDirectory.Attributes = FileAttributes.Directory;
            }

            // Get names of .dll files in the plugin directory

            string[] pluginFiles = Directory.GetFiles(pluginsPath, "*.dll");

            try
            {
                foreach (var pluginFile in pluginFiles)
                {
                    try
                    {
                        // Load assembly

                        Assembly assembly = Assembly.LoadFrom(pluginFile);
                        var _type = typeof(IPluginFactory);

                        // Get all types that implement an interface

                        var types = assembly.GetTypes()
                            .Where(p => _type.IsAssignableFrom(p));

                        foreach (var type in types)
                        {
                            // Add instances of received types implementing factories in the program

                            plugin = (Factory)Activator.CreateInstance(type);

                            ComboBoxItem item = new ComboBoxItem();
                            item.Content = plugin.PluginName();

                            Plugins.Add(plugin.PluginName(), plugin);

                            cbFactory.Items.Add(item);

                            amountOfPlugins++;
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }

            }
            catch (ReflectionTypeLoadException ex)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Exception exSub in ex.LoaderExceptions)
                {
                    sb.AppendLine(exSub.Message);
                    FileNotFoundException exFileNotFound = exSub as FileNotFoundException;
                    if (exFileNotFound != null)
                    {
                        if (!string.IsNullOrEmpty(exFileNotFound.FusionLog))
                        {
                            sb.AppendLine("Fusion Log:");
                            sb.AppendLine(exFileNotFound.FusionLog);
                        }
                    }
                    sb.AppendLine();
                }
                string errorMessage = sb.ToString();
                MessageBox.Show(errorMessage);
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

        void XMLconstuct()
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlDeclaration xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = xmlDocument.DocumentElement;
            xmlDocument.InsertBefore(xmlDeclaration, root);

            XmlElement body = xmlDocument.CreateElement(string.Empty, "body", string.Empty);
            xmlDocument.AppendChild(body);

            XmlElement background = xmlDocument.CreateElement(string.Empty, "background", string.Empty);
            XmlText backgroundColor = xmlDocument.CreateTextNode(canvas.Background.ToString());
            body.AppendChild(background);
            background.AppendChild(backgroundColor);

            xmlDocument.Save("../../document.xml");
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //XMLconstuct();
        }

        void XMLLoad()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load("../../document.xml");
           
            XmlNodeList backgroundColor = xmlDocument.GetElementsByTagName("background");
            // BackgroundColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(backgroundColor[0].InnerText));
            cbBackgroundColor.Text = backgroundColor[0].InnerText;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlDeclaration xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = xmlDocument.DocumentElement;
            xmlDocument.InsertBefore(xmlDeclaration, root);

            XmlElement body = xmlDocument.CreateElement(string.Empty, "body", string.Empty);
            xmlDocument.AppendChild(body);

            XmlElement background = xmlDocument.CreateElement(string.Empty, "background", string.Empty);
            XmlText backgroundColor = xmlDocument.CreateTextNode((String)((ComboBoxItem)cbBackgroundColor.SelectedItem).Content);
            body.AppendChild(background);
            background.AppendChild(backgroundColor);

            xmlDocument.Save("../../document.xml");
        }

        private void cbBackgroundColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dic = new Dictionary<String, SolidColorBrush>();
            dic.Add("White", new SolidColorBrush(Colors.White));
            dic.Add("Green", new SolidColorBrush(Colors.Green));
            dic.Add("Yellow", new SolidColorBrush(Colors.Yellow));
            dic.Add("Violet", new SolidColorBrush(Colors.Violet));
            String selectedValue = (String)((ComboBoxItem)cbBackgroundColor.SelectedItem).Content;
            BackgroundColor = dic[selectedValue];
        }

        private void mitReload_Click(object sender, RoutedEventArgs e)
        {
           // deletePlugins();
           // addPlugins();
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

        private void mitSave_Click(object sender, RoutedEventArgs e)
        {
            // Restore original color of an illuminated figure

            if (exShape != null)
            {
                exShape.color = exColor;
                list.Draw(canvas);
            }

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

            factory = null;
            cbFactory.SelectedIndex = 0;
        }

        private void mitOpen_Click(object sender, RoutedEventArgs e)
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
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
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
