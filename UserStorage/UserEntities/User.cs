using System;

namespace UserStorage.UserEntities
{
    [Serializable]
    public class User
    { 
        public int Id { get;}
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PersonalId { get; set; } //PassportNumber
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public VisaRecord[] VisaRecords { get; set; }

        public User(int id)
        {
            Id = id;
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
        
    }
}
