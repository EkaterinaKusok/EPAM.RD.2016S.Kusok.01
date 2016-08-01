using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using UserStorage.Interfacies.UserEntities;

namespace UserStorage.Interfacies.ServiceInfo
{     
    public class StorageState : IXmlSerializable
    {
        public int CurrentId { get; set; }

        public List<User> Users { get; set; }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            reader.ReadStartElement(nameof(System.Data.SqlTypes.StorageState));
            CurrentId = reader.ReadElementContentAsInt();
            var serializer = new XmlSerializer(typeof(List<User>));
            Users = (List<User>)serializer.Deserialize(reader);
            reader.ReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString(nameof(CurrentId), CurrentId.ToString());
            var serializer = new XmlSerializer(typeof(List<User>));
            serializer.Serialize(writer, Users);
        }
    }
}
