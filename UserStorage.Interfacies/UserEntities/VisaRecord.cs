using System;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace UserStorage.Interfacies.UserEntities
{
    /// <summary>
    /// Contains Visa information.
    /// </summary>
    /// <seealso cref="System.Xml.Serialization.IXmlSerializable" />
    [Serializable]
    [DataContract]
    public struct VisaRecord : IXmlSerializable
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VisaRecord"/> struct.
        /// </summary>
        /// <param name="country">The country.</param>
        /// <param name="start">The visa start date.</param>
        /// <param name="end">The visa end date.</param>
        public VisaRecord(string country, DateTime start, DateTime end)
        {
            Country = country;
            StartDate = start;
            EndDate = end;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets the country for visa.
        /// </summary>
        [DataMember]
        public string Country { get; private set; }

        /// <summary>
        /// Gets the visa start date.
        /// </summary>
        [DataMember]
        public DateTime StartDate { get; private set; }

        /// <summary>
        /// Gets the visa end date.
        /// </summary>
        [DataMember]
        public DateTime EndDate { get; private set; }
        #endregion

        #region Public Methods

        /// <summary>
        /// This method is reserved and should not be used. When implementing the IXmlSerializable interface, you should return null (Nothing in Visual Basic) from this method, and instead, if specifying a custom schema is required, apply the <see cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute" /> to the class.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Xml.Schema.XmlSchema" /> that describes the XML representation of the object that is produced by the <see cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)" /> method and consumed by the <see cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)" /> method.
        /// </returns>
        public XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader" /> stream from which the object is deserialized.</param>
        public void ReadXml(XmlReader reader)
        {
            reader.ReadStartElement(nameof(VisaRecord));
            Country = reader.ReadElementContentAsString();
            StartDate = reader.ReadElementContentAsDateTime();
            EndDate = reader.ReadElementContentAsDateTime();
            reader.ReadEndElement();
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter" /> stream to which the object is serialized.</param>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString(nameof(Country), Country);
            writer.WriteElementString(nameof(StartDate), StartDate.ToString("yyyy-MM-dd"));
            writer.WriteElementString(nameof(EndDate), EndDate.ToString("yyyy-MM-dd"));
        }
        #endregion
    }
}
