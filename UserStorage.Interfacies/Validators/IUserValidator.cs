using System;
using UserStorage.Interfacies.UserEntities;

namespace UserStorage.Interfacies.Validators
{
    /// <summary>
    /// Provides a way for validation users.
    /// </summary>
    public interface IUserValidator
    {
        /// <summary>
        /// Validates the user first name.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <returns>True if parameter is valid; otherwise - false.</returns>
        bool FirstNameIsValid(string firstName);

        /// <summary>
        /// Validates the user last name.
        /// </summary>
        /// <param name="lastName">The first name.</param>
        /// <returns>True if parameter is valid; otherwise - false.</returns>
        bool LastNameIsValid(string lastName);

        /// <summary>
        /// The personal identifier is valid.
        /// </summary>
        /// <param name="personalId">The personal identifier.</param>
        /// <returns>True if parameter is valid; otherwise - false.</returns>
        bool PersonalIdIsValid(string personalId);

        /// <summary>
        /// Date the of birth is valid.
        /// </summary>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <returns>True if parameter is valid; otherwise - false.</returns>
        bool DateOfBirthIsValid(DateTime dateOfBirth);

        /// <summary>
        /// Visas the records are valid.
        /// </summary>
        /// <param name="visaRecords">The visa records.</param>
        /// <returns>True if parameter is valid; otherwise - false.</returns>
        bool VisaRecordsAreValid(VisaRecord[] visaRecords);
    }
}