using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace UserStorage.Interfacies.UserEntities
{
    [Serializable]
    public struct VisaRecord : IXmlSerializable
    {
        public VisaRecord(string country, DateTime start, DateTime end)
        {
            Country = country;
            StartDate = start;
            EndDate = end;
        }

        public string Country { get; private set; }

        public DateTime StartDate { get; private set; }

        public DateTime EndDate { get; private set; }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            reader.ReadStartElement(nameof(VisaRecord));
            Country = reader.ReadElementContentAsString();
            StartDate = reader.ReadElementContentAsDateTime();
            EndDate = reader.ReadElementContentAsDateTime();
            reader.ReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString(nameof(Country), Country);
            writer.WriteElementString(nameof(StartDate), StartDate.ToString("yyyy-MM-dd"));
            writer.WriteElementString(nameof(EndDate), EndDate.ToString("yyyy-MM-dd"));
        }
    }
}
