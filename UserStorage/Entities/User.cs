using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorage.Entities
{
    [Serializable]
    public class User
    { 
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int PersonalId { get; set; } //PassportNumber
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public VisaRecord[] VisaRecords { get; set; }

        public User(string firstName, string lastName, int personalId, DateTime dateOfBirth, Gender gender, VisaRecord[] visaRecords)
        {
            Id = 0;
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
            return (Id.GetHashCode()  ^ LastName.GetHashCode());
        }
        
    }
}
