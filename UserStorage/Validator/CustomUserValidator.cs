using System;
using System.Linq;
using System.Text.RegularExpressions;
using UserStorage.Interfacies.UserEntities;
using UserStorage.Interfacies.Validators;

namespace UserStorage.Validator
{
    /// <summary>
    /// Implements functions for validation users.
    /// </summary>
    /// <seealso cref="UserStorage.Interfacies.Validators.IUserValidator" />
    public class CustomUserValidator : IUserValidator
    {
        private Regex firstNameRegex = new Regex("[A-Z][a-z]+");
        private Regex lastNameRegex = new Regex("[A-Z][a-z]+(-[A-Z][a-z]+)?");
        private Regex personalIdRegex = new Regex("[A-Z]{2}[0-9]{7}[A-Z]{2}[0-9]{3}");

        private string[] validCountries = new[]
        {
            "Argentina", "Australia", "Austria", "Belarus", "Bulgaria", "China", "Egypt", "France", "Italy",
            "Japan", "Lithuania", "Malta", "Maldives", "New Zealand", "Norway", "Russia", "Spain ", "Turkey",
            "United Kingdom", "Switzerland", "Sweden", "Ukraine"
        };

        /// <summary>
        /// Validates the user first name.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <returns>
        /// True if parameter is valid; otherwise - false.
        /// </returns>
        public bool FirstNameIsValid(string firstName)
        {
            return firstNameRegex.IsMatch(firstName);
        }

        /// <summary>
        /// Validates the user last name.
        /// </summary>
        /// <param name="lastName">The first name.</param>
        /// <returns>
        /// True if parameter is valid; otherwise - false.
        /// </returns>
        public bool LastNameIsValid(string lastName)
        {
            return lastNameRegex.IsMatch(lastName);
        }

        /// <summary>
        /// The personal identifier is valid.
        /// </summary>
        /// <param name="personalId">The personal identifier.</param>
        /// <returns>
        /// True if parameter is valid; otherwise - false.
        /// </returns>
        public bool PersonalIdIsValid(string personalId)
        {
            return personalIdRegex.IsMatch(personalId);
        }

        /// <summary>
        /// Date the of birth is valid.
        /// </summary>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <returns>
        /// True if parameter is valid; otherwise - false.
        /// </returns>
        public bool DateOfBirthIsValid(DateTime dateOfBirth)
        {
            return dateOfBirth < DateTime.Now && DateTime.Now.Year - dateOfBirth.Year < 130;
        }

        /// <summary>
        /// Visas the records are valid.
        /// </summary>
        /// <param name="visaRecords">The visa records.</param>
        /// <returns>
        /// True if parameter is valid; otherwise - false.
        /// </returns>
        public bool VisaRecordsAreValid(VisaRecord[] visaRecords)
        {
            bool result = true;
            if (visaRecords != null)
            {
                foreach (var visa in visaRecords)
                {
                    if (!VisaRecordIsValid(visa))
                    {
                        result = false;
                        break;
                    }
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
