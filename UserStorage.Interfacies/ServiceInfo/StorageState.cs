using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using UserStorage.Interfacies.UserEntities;

namespace UserStorage.Interfacies.ServiceInfo
{
    /// <summary>
    /// Represents current storage state.
    /// </summary>
    /// <seealso cref="System.Xml.Serialization.IXmlSerializable" />
    public class StorageState : IXmlSerializable
    {
        #region Properties

        /// <summary>
        /// Gets or sets the current User identifier.
        /// </summary>
        public int CurrentId { get; set; }

        /// <summary>
        /// Gets or sets the collection of users.
        /// </summary>
        public List<User> Users { get; set; }
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
            reader.ReadStartElement(nameof(System.Data.SqlTypes.StorageState));
            CurrentId = reader.ReadElementContentAsInt();
            var serializer = new XmlSerializer(typeof(List<User>));
            Users = (List<User>)serializer.Deserialize(reader);
            reader.ReadEndElement();
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter" /> stream to which the object is serialized.</param>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString(nameof(CurrentId), CurrentId.ToString());
            var serializer = new XmlSerializer(typeof(List<User>));
            serializer.Serialize(writer, Users);
        }
        #endregion
    }
}
