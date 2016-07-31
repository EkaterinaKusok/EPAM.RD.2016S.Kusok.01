using System;
using System.Linq;
using System.Text.RegularExpressions;
using UserStorage.Interfacies.UserEntities;
using UserStorage.Interfacies.Validators;

namespace UserStorage.Validator
{
    public class CustomUserValidator : IUserValidator
    {
        private Regex firstNameRegex = new Regex("[A-Z][a-z]+");
        private Regex lastNameRegex = new Regex("[A-Z][a-z]+(-[A-Z][a-z]+)?");
        private Regex personalIdRegex = new Regex("[A-Z]{2}[0-9]{7}[A-Z]{2}[0-9]{3}");

        private string[] validCountries = new[]
        {
            "Argentina", "Australia", "Austria", "Belarus", "Bulgaria", "China", "Egypt", "France", "Italy",
            "Japan", "Lithuania", "", "Malta", "Maldives", "New Zealand", "Norway", "Russia", "Spain ", "Turkey",
            "United Kingdom", "Switzerland", "Sweden","Ukraine"
        };

        public bool FirstNameIsValid(string firstName)
        {
            return firstNameRegex.IsMatch(firstName);
        }

        public bool LastNameIsValid(string lastName)
        {
            return lastNameRegex.IsMatch(lastName);
        }

        public bool PersonalIdIsValid(string personalId)
        {
            return personalIdRegex.IsMatch(personalId) ;
        }

        public bool DateOfBirthIsValid(DateTime dateOfBirth)
        {
            return (dateOfBirth < DateTime.Now && DateTime.Now.Year - dateOfBirth.Year < 130);
        }

        public bool VisaRecordsAreValid(VisaRecord[] visaRecords)
        {
            bool result = true;
            foreach (var visa in visaRecords)
            {
                if (!VisaRecordIsValid(visa))
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

        private bool VisaRecordIsValid(VisaRecord visa)
        {
            return (visa.StartDate < visa.EndDate) && validCountries.Contains(visa.Country);
        }

    }
}
