using System;
using System.Runtime.Serialization;

namespace UserStorage.Interfacies.UserEntities
{
    [Serializable]
    public class User : ISerializable
    { 
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PersonalId { get; set; } //PassportNumber
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public VisaRecord[] VisaRecords { get; set; }

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

        
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // Use the AddValue method to specify serialized values.
            info.AddValue("Id", Id, typeof(int));
            info.AddValue("FirstName", FirstName, typeof(string));
            info.AddValue("LastName", LastName, typeof(string));
            info.AddValue("PersonalId", PersonalId, typeof(string));
            info.AddValue("DateOfBirth", DateOfBirth, typeof(DateTime));
            info.AddValue("Gender", Gender, typeof(Gender));
            info.AddValue("VisaRecords", VisaRecords, typeof(VisaRecord[]));

        }
        // The special constructor is used to deserialize values.
        public User(SerializationInfo info, StreamingContext context)
        {
            // Reset the property value using the GetValue method.
            Id = (int)info.GetValue("Id", typeof(int));
            FirstName = (string)info.GetValue("FirstName", typeof(string));
            LastName = (string)info.GetValue("LastName", typeof(string));
            PersonalId = (string)info.GetValue("PersonalId", typeof(string));
            DateOfBirth = (DateTime)info.GetValue("DateOfBirth", typeof(DateTime));
            Gender = (Gender)info.GetValue("Gender", typeof(Gender));
            VisaRecords = (VisaRecord[])info.GetValue("VisaRecords", typeof(VisaRecord[]));
        }

    }
}
