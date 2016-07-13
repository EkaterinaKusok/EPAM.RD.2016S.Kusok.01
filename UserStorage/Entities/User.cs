using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorage.Entities
{
    public class User
    { 
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public VisaRecord[] VisaRecords { get; set; }

        public User(int id, string firstName, string lastName, DateTime dateOfBirth, Gender gender, VisaRecord[] visaRecords)
        {
            this.Id = id;
            FirstName = firstName;
            LastName = lastName;
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
