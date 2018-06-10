using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ZPaint.List
{
    class Tools
    {
        public static Factory factory { get; set; }

        // Standart figures
        ComboBoxItem cbitCursor = new ComboBoxItem();
        ComboBoxItem cbitLine = new ComboBoxItem();
        ComboBoxItem cbitRectangle = new ComboBoxItem();
        ComboBoxItem cbitSquare = new ComboBoxItem();
        ComboBoxItem cbitOval = new ComboBoxItem();
        ComboBoxItem cbitCircle = new ComboBoxItem();
        ComboBoxItem cbitTriangle = new ComboBoxItem();
        ComboBoxItem cbitHexagon = new ComboBoxItem();

        // Storage of plugin figure
        Dictionary<string, Factory> Plugins;
        // Storage of user figures
        Dictionary<string, List<Shape>> UserShapes;

        public event EventHandler FactorySelected;

        public Tools(Dictionary<string, Factory> Plugins, Dictionary<string, List<Shape>> UserShapes, string locale)
        {
            this.Plugins = Plugins;
            this.UserShapes = UserShapes;
            SetLocale(locale);
        }

        private void SetLocale(string _culture)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture(_culture);
            ResourceManager rm = new ResourceManager("ZPaint.locale", typeof(MainWindow).Assembly);
            cbitCursor.Content = rm.GetString("cbitCursor", culture);
            cbitLine.Content = rm.GetString("cbitLine", culture);
            cbitRectangle.Content = rm.GetString("cbitRectangle", culture);
            cbitSquare.Content = rm.GetString("cbitSquare", culture);
            cbitOval.Content = rm.GetString("cbitOval", culture);
            cbitCircle.Content = rm.GetString("cbitCircle", culture);
            cbitTriangle.Content = rm.GetString("cbitTriangle", culture);
            cbitHexagon.Content = rm.GetString("cbitHexagon", culture);
        }

        public void DrawingToolsLoad(ref ComboBox cbFactory)
        {
            // Events for standard items
            cbitCursor.Selected += cbitCursor_Selected;
            cbitLine.Selected += cbitLine_Selected;
            cbitRectangle.Selected += cbitRectangle_Selected;
            cbitSquare.Selected += cbitSquare_Selected;
            cbitOval.Selected += cbitOval_Selected;
            cbitCircle.Selected += cbitCircle_Selected;
            cbitTriangle.Selected += cbitTriangle_Selected;
            cbitHexagon.Selected += cbitHexagon_Selected;

            // Adding standard tools
            cbFactory.Items.Add(cbitCursor);
            cbFactory.Items.Add(cbitLine);
            cbFactory.Items.Add(cbitRectangle);
            cbFactory.Items.Add(cbitSquare);
            cbFactory.Items.Add(cbitOval);
            cbFactory.Items.Add(cbitCircle);
            cbFactory.Items.Add(cbitTriangle);
            cbFactory.Items.Add(cbitHexagon);

            // Adding plugin tools
            foreach (var plugin in Plugins)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = plugin.Key;
                cbFactory.Items.Add(item);
            }

            // Adding user figures
            foreach (var shape in UserShapes)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = shape.Key;
                cbFactory.Items.Add(item);
            }

            cbFactory.SelectedIndex = 0;
            // MainWindow.SetLocale(GetLocale());
        }

        // Choose a type of a figure

        private void cbitCursor_Selected(object sender, RoutedEventArgs e)
        {
            factory = null;
            FactorySelected?.Invoke(this, new EventArgs());
        }

        private void cbitLine_Selected(object sender, RoutedEventArgs e)
        {
            factory = new FactoryLine();
            FactorySelected?.Invoke(this, new EventArgs());
        }

        private void cbitRectangle_Selected(object sender, RoutedEventArgs e)
        {
            factory = new FactoryRectangle();
            FactorySelected?.Invoke(this, new EventArgs());
        }

        private void cbitSquare_Selected(object sender, RoutedEventArgs e)
        {
            factory = new FactorySquare();
            FactorySelected?.Invoke(this, new EventArgs());
        }

        private void cbitOval_Selected(object sender, RoutedEventArgs e)
        {
            factory = new FactoryEllipse();
            FactorySelected?.Invoke(this, new EventArgs());
        }

        private void cbitCircle_Selected(object sender, RoutedEventArgs e)
        {
            factory = new FactoryCircle();
            FactorySelected?.Invoke(this, new EventArgs());
        }

        private void cbitTriangle_Selected(object sender, RoutedEventArgs e)
        {
            factory = new FactoryTriangle();
            FactorySelected?.Invoke(this, new EventArgs());
        }

        private void cbitHexagon_Selected(object sender, RoutedEventArgs e)
        {
            factory = new FactoryHexagon();
            FactorySelected?.Invoke(this, new EventArgs());
        }
    }
}
