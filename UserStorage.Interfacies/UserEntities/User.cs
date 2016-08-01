using System;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace UserStorage.Interfacies.UserEntities
{
    [DataContract]
    [Serializable]
    public class User : IXmlSerializable
    {
        public User()
        {
        }

        public User(int id, string firstName, string lastName, string personalId, DateTime dateOfBirth, Gender gender, VisaRecord[] visaRecords)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            PersonalId = personalId;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            VisaRecords = visaRecords;
        }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string PersonalId { get; set; } // PassportNumber

        [DataMember]
        public DateTime DateOfBirth { get; set; }

        [DataMember]
        public Gender Gender { get; set; }

        [DataMember]
        public VisaRecord[] VisaRecords { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || this.GetType() != obj.GetType())
            {
                return false;
            }

            User user = (User)obj;
            return user.FirstName.Equals(FirstName) && user.LastName.Equals(LastName);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            reader.ReadStartElement(nameof(User));
            Id = reader.ReadElementContentAsInt();
            FirstName = reader.ReadElementContentAsString();
            LastName = reader.ReadElementContentAsString();
            PersonalId = reader.ReadElementContentAsString();
            DateOfBirth = reader.ReadElementContentAsDateTime();
            Gender = (Gender)reader.ReadElementContentAsInt();
            var serializer = new XmlSerializer(typeof(VisaRecord[]));
            VisaRecords = (VisaRecord[])serializer.Deserialize(reader);
            reader.ReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString(nameof(Id), Id.ToString());
            writer.WriteElementString(nameof(FirstName), FirstName);
            writer.WriteElementString(nameof(LastName), LastName);
            writer.WriteElementString(nameof(PersonalId), PersonalId);
            writer.WriteElementString(nameof(DateOfBirth), DateOfBirth.ToString("yyyy-MM-dd"));
            writer.WriteElementString(nameof(Gender), ((int)Gender).ToString());
            var serializer = new XmlSerializer(typeof(VisaRecord[]));
            serializer.Serialize(writer, VisaRecords);
        }
    }
}
