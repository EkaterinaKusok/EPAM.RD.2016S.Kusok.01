using System;
using UserStorage.Interfacies.UserEntities;

namespace UserStorage.Interfacies.Validators
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