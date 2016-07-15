using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UserStorage.Entities;
using UserStorage.Interfacies;

namespace UserStorage
{
    public class XmlRepository : IRepository<User>
    {
        public IEnumerable<User> Load(string path)
        {
            List<User> users = new List<User>();
            try
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.CloseInput = true;
                settings.ConformanceLevel = ConformanceLevel.Fragment;
                settings.IgnoreWhitespace = true;
                using (XmlReader reader = XmlReader.Create(path, settings))
                {
                    while (!reader.EOF)
                    {
                        reader.ReadStartElement("User");
                        string a = reader.ReadElementString("Name");
                        string b = reader.ReadElementString("Surname");
                        string personalId = reader.ReadElementString("PersonalId");
                        //int d = Int32.Parse(reader.ReadElementString("Year"));
                        users.Add(new User(a, b, personalId, DateTime.Now,Gender.Male,null));
                        reader.ReadEndElement();
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                throw new ApplicationException("File doesn't exist!", ex);
            }
            catch (XmlException ex)
            {
                throw new ApplicationException("File can't be processed!", ex);
            }
            return users;
        }

        public bool Save(string path, IEnumerable<User> users)
        {
            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.CloseOutput = true;
                settings.ConformanceLevel = ConformanceLevel.Fragment;

                using (XmlWriter writer = XmlWriter.Create(path, settings))
                {
                    foreach (var user in users)
                    {
                        writer.WriteStartElement("User");
                        writer.WriteElementString("Id", user.Id.ToString());
                        writer.WriteElementString("Name", user.FirstName);
                        writer.WriteElementString("Surname", user.LastName);
                        writer.WriteElementString("PersonalId", user.PersonalId.ToString());
                        //writer.WriteElementString("Gender", user.Gender.ToString());
                        writer.WriteEndElement();
                    }
                }
            }
            catch (XmlException ex)
            {
                throw new ApplicationException("File can't be save!", ex);
            }
            return true;
        }
    }
}
