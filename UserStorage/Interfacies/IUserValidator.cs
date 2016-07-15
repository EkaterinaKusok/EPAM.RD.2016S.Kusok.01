using System;
using UserStorage.Entities;

namespace UserStorage.Interfacies
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