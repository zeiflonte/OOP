using System.Xml;
using System.Collections.Generic;
using System.IO;
using System;
using Microsoft.Win32;

namespace SAX
{
    class Program
    {
        static void Main(string[] args)
        {
            List<User> users = new List<User>();
            User user = new User();

            Console.Write("Enter xml file path: ");
            string documentPath = Console.ReadLine();
            
            /* OpenFileDialog fileOpen = new OpenFileDialog()
             {
                 Filter = "XML File (*.xml)|*.xml|All Files (*.*)|*.*",
                 RestoreDirectory = true,
             };*/
            // if (fileOpen.ShowDialog() == true)

            try
            {
                //documentPath = fileOpen.FileName;
                XmlTextReader xmlReader = new XmlTextReader(documentPath);
                xmlReader.MoveToContent();

                // Move to the next node
                while (xmlReader.Read())
                {
                    if (xmlReader.NodeType == XmlNodeType.Element)
                    {
                        switch (xmlReader.Name)
                        {
                            // Initialize a user instance
                            case "user":
                                user = new User();
                                break;
                            // Collect information about user
                            case "uid":
                                if (user != null)
                                {
                                    user.Id = xmlReader.ReadString();
                                }
                                break;
                            case "name":
                                if (user != null)
                                {
                                    user.Name = xmlReader.ReadString();
                                }
                                break;
                            case "skill":
                                var skill = xmlReader.ReadString();
                                if (skill == "CodeIgniter" || skill == "CSS3" || skill == "Sinatra")
                                {
                                    user.skills.Add(skill);
                                }
                                break;
                        }
                    }
                    else
                    {
                        // Process the end of a user entry
                        if (xmlReader.NodeType == XmlNodeType.EndElement && xmlReader.Name == "user"
                            // Add an entry if the user has required skills 
                            && user.skills.Count != 0)
                        {
                            users.Add(user);
                            user = null;
                        }
                    }
                }

                TextWriter writer = new StreamWriter("users.txt");

                foreach (var entry in users)
                {
                    // Read users unformation in the console and the output file
                    Console.WriteLine("Name: {0};  ID: {1}", entry.Name, entry.Id);
                    writer.WriteLine("Name: {0}; ID: {1}", entry.Name, entry.Id);
                    Console.Write("Skills: ");
                    writer.Write("Skills: ");
                    foreach (var skill in entry.skills)
                    {
                        Console.Write("{0}  ", skill);
                        writer.Write("{0}  ", skill);
                    }
                    writer.WriteLine("\r\n");
                    Console.WriteLine("\r\n");
                }

                Console.WriteLine("Done!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadLine();
        }
    }
}