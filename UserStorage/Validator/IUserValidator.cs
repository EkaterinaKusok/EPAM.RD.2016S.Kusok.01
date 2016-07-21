using System;
using UserStorage.UserEntities;

namespace UserStorage.Validator
{
    public interface IUserValidator
    {
        bool FirstNameIsValid(string firstName);
        bool LastNameIsValid(string lastName);
        bool PersonalIdIsValid(string personalId);
        bool DateOfBirthIsValid(DateTime dateOfBirth);
        bool VisaRecordsAreValid(VisaRecord[] visaRecords);
    }
}