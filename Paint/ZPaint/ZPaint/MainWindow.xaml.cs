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
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using System.Resources;
using System.Globalization;

namespace ZPaint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private CultureInfo culture;

        private Shape shape;
        private SolidColorBrush exColor;

        private Factory factory;
        private ListFigures list = new ListFigures();
        List<Shape> listShape;
        List<Shape> listExShape;

        // Initial figure properties

        private Point point1;
        private Point point2;
        private int thickness;
        private SolidColorBrush color;

        List<Type> typeList = new List<Type>()
        {
            typeof(Rectangle),
            typeof(Square),
            typeof(Circle),
            typeof(Ellipse),
            typeof(Triangle),
            typeof(Hexagon),
            typeof(SolidColorBrush),
            typeof(Shape),
            typeof(Point),
            typeof(MatrixTransform),
            typeof(Line)
        };

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
            XMLLoad();
            addPluginsFactories();
            addPlugins();
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
                List<Shape> listShape = new List<Shape>();
                listShape.Add(shape);
                list.Add(listShape);
                listShapes.Items.Add(listShape);
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

            if (listShape != null)
            {
                foreach (var shape in listShape)
                {
                    shape.SetThickness(thickness);
                }
                list.Draw(canvas);
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

            if (listShape != null)
            {
                foreach (var shape in listShape)
                {
                    shape.SetColor(color);
                }
                exColor = listShape.First().color;
                list.Draw(canvas);
            }
        }

        private void listShapes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Restore a previous color

            if (listExShape != null)
            {
                foreach (var shape in listExShape)
                {
                    shape.color = exColor;
                }
            }

            listShape = listShapes.SelectedItem as List<Shape>;

            // Save the previous color

            if (listShape != null)
            {
                listExShape = listShape;
                exColor = listShape.First().color;

                // Illuminate a selected figure

                foreach (var shape in listShape)
                {
                    shape.color = Brushes.Pink;
                }
            }

            list.Draw(canvas);
        }

        private void listShapes_LostFocus(object sender, RoutedEventArgs e)
        {
            if (listExShape != null)
            {
                foreach (var shape in listExShape)
                {
                    shape.color = exColor;
                }
                list.Draw(canvas);
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Delete) && (listShapes.SelectedIndex != -1))
            {
                // Delete a selected figure

                list.Remove(listShape);
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

        private void addPluginsFactories()
        {
            // Create a plugin directory if it does not exist

            DirectoryInfo pluginsDirectory = new DirectoryInfo(pluginsPath);
            if (!pluginsDirectory.Exists)
            {
                pluginsDirectory.Create();
                pluginsDirectory.Attributes = FileAttributes.Directory;
            }

            // Get names of .dll files in the plugin directory

            string locale = GetLocale();

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

                            item.Content = plugin.PluginName(locale);

                            Plugins.Add(plugin.PluginName(locale), plugin);

                            cbFactory.Items.Add(item);

                            amountOfPlugins++;
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
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

            string locale = GetLocale();

            string[] pluginFiles = Directory.GetFiles(pluginsPath, "*.dll");

            foreach (var pluginFile in pluginFiles)
            {
                try
                {
                    // Load assembly

                    Assembly assembly = Assembly.LoadFrom(pluginFile);
                    var _type = typeof(IPluginFigure);

                    // Get all types that implement an interface

                    var types = assembly.GetTypes()
                        .Where(p => _type.IsAssignableFrom(p));

                    foreach (var type in types)
                    {
                        // Add instances of received types implementing factories in the program

                        typeList.Add(type);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    continue;
                }
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

        void XMLLoad()
        {
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load("../../config.xml");

                XmlNodeList localeValue = xmlDocument.GetElementsByTagName("locale");
                cbLocale.Text = localeValue[0].InnerText;

                XmlNodeList thicknessValue = xmlDocument.GetElementsByTagName("thickness");
                cbThickness.Text = thicknessValue[0].InnerText;
                XmlNodeList colorValue = xmlDocument.GetElementsByTagName("color");
                cbColor.Text = colorValue[0].InnerText;

                XmlNodeList backgroundValue = xmlDocument.GetElementsByTagName("background");
                // BackgroundColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(backgroundColor[0].InnerText));
                cbBackgroundColor.Text = backgroundValue[0].InnerText;
            }
            catch (Exception ex)
            {
                if (ex is FileNotFoundException || ex is XmlException || ex is NullReferenceException)
                {
                    return;
                }
                throw;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlDeclaration xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = xmlDocument.DocumentElement;
            xmlDocument.InsertBefore(xmlDeclaration, root);

            XmlElement body = xmlDocument.CreateElement(string.Empty, "body", string.Empty);
            xmlDocument.AppendChild(body);

            XmlElement locale = xmlDocument.CreateElement(string.Empty, "locale", string.Empty);
            XmlText localeValue = xmlDocument.CreateTextNode((String)((ComboBoxItem)cbLocale.SelectedItem).Content);
            body.AppendChild(locale);
            locale.AppendChild(localeValue);

            XmlElement tools = xmlDocument.CreateElement(string.Empty, "tools", string.Empty); 
            body.AppendChild(tools);
            XmlElement thickness = xmlDocument.CreateElement(string.Empty, "thickness", string.Empty);
            XmlText thicknessValue = xmlDocument.CreateTextNode((String)((ComboBoxItem)cbThickness.SelectedItem).Content);
            tools.AppendChild(thickness);
            thickness.AppendChild(thicknessValue);
            XmlElement color = xmlDocument.CreateElement(string.Empty, "color", string.Empty);
            XmlText colorValue = xmlDocument.CreateTextNode((String)((ComboBoxItem)cbColor.SelectedItem).Content);
            tools.AppendChild(color);
            color.AppendChild(colorValue);

            XmlElement background = xmlDocument.CreateElement(string.Empty, "background", string.Empty);
            // XmlText backgroundColor = xmlDocument.CreateTextNode(canvas.Background.ToString());
            XmlText backgroundValue = xmlDocument.CreateTextNode((String)((ComboBoxItem)cbBackgroundColor.SelectedItem).Content);
            body.AppendChild(background);
            background.AppendChild(backgroundValue);

            xmlDocument.Save("../../config.xml");
        }

        private void SetLocale(string _culture)
        {
            culture = CultureInfo.CreateSpecificCulture(_culture);
            ResourceManager rm = new ResourceManager("ZPaint.locale", typeof(MainWindow).Assembly);
            mitFile.Header = rm.GetString("mitFile", culture);
            mitOpen.Header = rm.GetString("mitOpen", culture);
            mitSave.Header = rm.GetString("mitSave", culture);
            mitPlugins.Header = rm.GetString("mitPlugins", culture);
            mitSetFolder.Header = rm.GetString("mitSetFolder", culture);
            mitReload.Header = rm.GetString("mitReload", culture);
            cbitCursor.Content = rm.GetString("cbitCursor", culture);
            cbitLine.Content = rm.GetString("cbitLine", culture);
            cbitRectangle.Content = rm.GetString("cbitRectangle", culture);
            cbitSquare.Content = rm.GetString("cbitSquare", culture);
            cbitOval.Content = rm.GetString("cbitOval", culture);
            cbitCircle.Content = rm.GetString("cbitCircle", culture);
            cbitTriangle.Content = rm.GetString("cbitTriangle", culture);
            cbitHexagon.Content = rm.GetString("cbitHexagon", culture);
            cbitThin.Content = rm.GetString("cbitThin", culture);
            cbitMedium.Content = rm.GetString("cbitMedium", culture);
            cbitThick.Content = rm.GetString("cbitThick", culture);
            cbitBlack.Content = rm.GetString("cbitBlack", culture);
            cbitBlue.Content = rm.GetString("cbitBlue", culture);
            cbitRed.Content = rm.GetString("cbitRed", culture);
            cbitBWhite.Content = rm.GetString("cbitBWhite", culture);
            cbitBGreen.Content = rm.GetString("cbitBGreen", culture);
            cbitBYellow.Content = rm.GetString("cbitBYellow", culture);
            cbitBViolet.Content = rm.GetString("cbitBViolet", culture);
        }

        private string GetLocale()
        {
            var dic = new Dictionary<string, string>();
            dic.Add("English", "en-US");
            dic.Add("Русский", "ru-RU");
            string selectedValue;
            try
            {
                selectedValue = (string)((ComboBoxItem)cbLocale.SelectedItem).Content;
            }
            catch (NullReferenceException)
            {
                cbLocale.SelectedIndex = 0;
                selectedValue = (string)((ComboBoxItem)cbLocale.SelectedItem).Content;
            }
            return dic[selectedValue];
        }

        private void cbLocale_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetLocale(GetLocale());
        }

        private void cbBackgroundColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dic = new Dictionary<String, SolidColorBrush>();
            dic.Add("White", new SolidColorBrush(Colors.White));
            dic.Add("Белый", new SolidColorBrush(Colors.White));
            dic.Add("Green", new SolidColorBrush(Colors.Green));
            dic.Add("Зелёный", new SolidColorBrush(Colors.Green));
            dic.Add("Yellow", new SolidColorBrush(Colors.Yellow));
            dic.Add("Жёлтый", new SolidColorBrush(Colors.Yellow));
            dic.Add("Violet", new SolidColorBrush(Colors.Violet));
            dic.Add("Фиолетовый", new SolidColorBrush(Colors.Violet));
            string selectedValue;
            try
            {
                selectedValue = (string)((ComboBoxItem)cbBackgroundColor.SelectedItem).Content;
            }
            catch (NullReferenceException)
            {
                cbBackgroundColor.SelectedIndex = 0;
                selectedValue = (string)((ComboBoxItem)cbBackgroundColor.SelectedItem).Content;
            }
            BackgroundColor = dic[selectedValue];
        }

        private void mitReload_Click(object sender, RoutedEventArgs e)
        {
           // deletePlugins();
           // addPlugins();
        }

        private void mitSave_Click(object sender, RoutedEventArgs e)
        {
            // Restore original color of an illuminated figure

            if (listExShape != null)
            {
                foreach (Shape shape in listExShape)
                {
                    shape.color = exColor;
                }
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
                
                string filename = fileSave.FileName;

                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(list.GetType(), typeList.ToArray());

                using (FileStream stream = new FileStream(filename, FileMode.Create))
                {
                    list.Serialize(jsonSerializer, stream);
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
                
                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(list.GetType(), typeList.ToArray());

                string fileName = fileOpen.FileName;

                using (FileStream stream = new FileStream(fileName, FileMode.Open))
                {
                    try
                    {
                        list.Clear();
                        list.Deserialize(jsonSerializer, stream);
                            
                        // Create figures

                        foreach (var listShape in list.list)
                        {
                            listShapes.Items.Add(listShape);
                            foreach (Shape shape in listShape)
                            {
                                shape.DrawInCanvas(point1, point2, canvas);
                            }
                        }
                        shape = null;
                    }
                    catch (SerializationException)
                    { }
                }

                fileOpen = null;
            }
        }
    }
}
