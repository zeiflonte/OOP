using System.Xml;
using System.Windows.Forms;
using System.Collections.Generic;

namespace SAX
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> users = new List<string>();

            string documentPath;
            OpenFileDialog fileOpen = new OpenFileDialog
            {
                Filter = "XML File (*.xml)|*.xml|All Files (*.*)|*.*",
                RestoreDirectory = true,
            };
            if (fileOpen.ShowDialog() == DialogResult.OK)
            {
                documentPath = fileOpen.FileName;
                XmlTextReader xmlReader = new XmlTextReader(documentPath);
                xmlReader.MoveToContent();

                while (xmlReader.Read())
                {
                    if (xmlReader.NodeType == XmlNodeType.Element)
                    {
                        if (xmlReader.Name == "skills")
                        {
                            if (xmlReader.
                        }
                    }
                }
            }
        }
    }
}
