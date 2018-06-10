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
using ZPaint.List;

namespace ZPaint
{
    /// <summary>
    /// Container for storable user data, such as list of figures and struct of user figure patterns 
    /// </summary>
    [DataContract]
    public class UserData
    {
        [DataMember]
        public List<List<Shape>> list;

        [DataMember]
        public Dictionary<string, List<Shape>> UserShapes = new Dictionary<String, List<Shape>>();
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CultureInfo culture;

        private Shape shape;

        private Factory factory;
        private ListFigures list = new ListFigures();
        List<Shape> listShape;
        List<Shape> userShape;

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
            typeof(Line),
            typeof(Factory),
            typeof(FactoryCircle),
            typeof(FactoryEllipse),
            typeof(FactoryHexagon),
            typeof(FactoryLine),
            typeof(FactoryRectangle),
            typeof(FactorySquare),
            typeof(FactoryTriangle),
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

        Dictionary<string, Factory> Plugins = new Dictionary<string, Factory>();
        Dictionary<string, List<Shape>> UserShapes = new Dictionary<String, List<Shape>>();

        public static string pluginsPath = "../../../Plugins";
        // public static string pluginsPath = "../../../Pentagon/bin/Debug";

        public MainWindow()
        {
            InitializeComponent();
            XMLLoad();
            addPluginsFactories();
            addPlugins();
            LoadTools();
        }

        private void LoadTools()
        {
            string locale = GetLocale();
            Tools tools = new Tools(Plugins, UserShapes, locale);
            // Invoke the event if a tool was selected
            tools.FactorySelected += new EventHandler(tools_FactorySelected);
            tools.DrawingToolsLoad(ref cbFactory);
        }

        private void tools_FactorySelected(object sender, EventArgs e)
        {
            factory = Tools.factory;
        }

        private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Save the first position

            point1 = e.GetPosition(canvas);

            // Set a cursor type

            if (factory != null || userShape != null)
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
                List<Shape> listShape = new List<Shape>();
                listShape.Add(shape);
                list.Add(listShape);
                listShapes.Items.Add(listShape);
                shape.DrawInCanvas(canvas);

                // Reset initial settings

                shape = null;
                Cursor = Cursors.Arrow;
            }
            else
            {
                // Selected a user figure
                if (userShape != null)
                {
                    // Save the second position

                    point2 = e.GetPosition(canvas);

                    // Create a new figure

                    List<Shape> tempList = new List<Shape>(); 

                    foreach (Shape tmp in userShape)
                    {
                        Point actualPoint1 = new Point();
                        Point actualPoint2 = new Point();

                        // Set position of a new figure
                        actualPoint1.X = point1.X + tmp.point1.X;
                        actualPoint1.Y = point1.Y + tmp.point1.Y;
                        actualPoint2.X = point1.X + tmp.point2.X;
                        actualPoint2.Y = point1.Y + tmp.point2.Y;

                        // Create a new part of a collection based on a user pattern
                        shape = tmp.factory.Create(tmp.factory, tmp.color, tmp.thickness, actualPoint1, actualPoint2);

                        // Save each part of a collection
                        tempList.Add(shape);
                        shape.DrawInCanvas(canvas);
                    }

                    // Save a whole collection
                    list.Add(tempList);
                    listShapes.Items.Add(tempList);
                    
                    // Reset initial settings
                    shape = null;
                    Cursor = Cursors.Arrow;
                }
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
                list.Draw(canvas);
            }
        }

        private void listShapes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Restore a previous color

            foreach (var shapes in list.list)
            {
                foreach (var shape in shapes)
                {
                    if (shape.IsSelected)
                    {
                        shape.IsSelected = false;
                    }
                }
            }

            listShape = listShapes.SelectedItem as List<Shape>;

            if (listShape != null)
            {
                // Illuminate a selected figure

                foreach (var shape in listShape)
                {
                    shape.IsSelected = true;
                }
            }

            list.Draw(canvas);
        }

        private void listShapes_LostFocus(object sender, RoutedEventArgs e)
        {
            // Restore a previous color

            foreach (var shapes in list.list)
            {
                foreach (var shape in shapes)
                {
                    if (shape.IsSelected)
                    {
                        shape.IsSelected = false;
                    }
                }
            }
            list.Draw(canvas);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Delete) && (listShapes.SelectedIndex != -1))
            {
                // Delete a selected figure

                list.Remove(listShape);
                listShapes.Items.Remove(listShapes.SelectedItem);
            }
            list.Draw(canvas);
        }

        Factory plugin;

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

                            typeList.Add(type);

                            plugin = (Factory)Activator.CreateInstance(type);

                            Plugins.Add(plugin.PluginName(locale), plugin);
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
            listShape = null;
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

        // Save an XML document with all configuration
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
            XmlText backgroundValue = xmlDocument.CreateTextNode((String)((ComboBoxItem)cbBackgroundColor.SelectedItem).Content);
            body.AppendChild(background);
            background.AppendChild(backgroundValue);

            xmlDocument.Save("../../config.xml");
        }

        // Set component names arrording to the selected language
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

            foreach (var shapes in list.list)
            {
                foreach (var shape in shapes)
                {
                    if (shape.IsSelected)
                    {
                        shape.IsSelected = false;
                    }
                }
            }
            list.Draw(canvas);

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

                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(UserData), typeList.ToArray());

                using (FileStream stream = new FileStream(filename, FileMode.Create))
                {
                    // Save data in a UserData serializable instance
                    UserData dataToSave = new UserData()
                    {
                        list = list.list,
                        UserShapes = UserShapes
                    };

                    jsonSerializer.WriteObject(stream, dataToSave);
                }
            }

            fileSave = null;
        }

        private void DeleteFigures()
        {
            // Clear canvas
            list.Clear();
            listShapes.Items.Clear();
            canvas.Children.Clear();

            // Delete user figures
            UserShapes.Clear();           

            // Set default values
            factory = null;
            userShape = null;
            cbFactory.Items.Clear();
            LoadTools();
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
                
                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(UserData), typeList.ToArray());

                string fileName = fileOpen.FileName;

                using (FileStream stream = new FileStream(fileName, FileMode.Open))
                {
                    try
                    {
                        list.Clear();
                        
                        // Read data contained in a UserData serialized instance
                        UserData dataToLoad = (UserData)jsonSerializer.ReadObject(stream);
                        list.list = dataToLoad.list;
                        UserShapes = dataToLoad.UserShapes; 

                        // Create figures

                        foreach (var listShape in list.list)
                        {
                            // Add in the sidebar
                            listShapes.Items.Add(listShape);
                            foreach (Shape shape in listShape)
                            {
                                shape.DrawInCanvas(canvas);
                            }
                        }
                        shape = null;
                    }
                    catch (SerializationException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

                fileOpen = null;

                // Load the tool list 
                cbFactory.Items.Clear();
                LoadTools();
            }
        }

        string shapeName;

        // Invoked if submit button was clicked
        private void window_SubmitClicked(object sender, EventArgs e)
        {
            // Get values from the Creator window
            listShape = Creator.listShape;
            shapeName = Creator.shapeName;
        }

        private void butCreator_Click(object sender, RoutedEventArgs e)
        {
            // Determine the current language
            string locale = GetLocale();
            // Open the Creator window
            Creator window = new Creator(Plugins, UserShapes, locale);
            window.SubmitClicked += new EventHandler(window_SubmitClicked);
            if (window.ShowDialog() == true)
            {
                // Save a new user figure
                ComboBoxItem item = new ComboBoxItem();
                item.Content = shapeName;
                UserShapes.Add(shapeName, listShape);
                // Add in the tool list
                cbFactory.Items.Add(item);
                listShape = null;
            }
        }
    }
}
