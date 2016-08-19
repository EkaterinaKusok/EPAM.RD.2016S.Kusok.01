using System;

namespace UserStorage.Validator
{
    /// <summary>
    /// Provides user validation exeptions.
    /// </summary>
    /// <seealso cref="System.Exception" />
    [Serializable]
    public class UserValidationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserValidationException"/> class.
        /// </summary>
        public UserValidationException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserValidationException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public UserValidationException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserValidationException"/> class.
        /// </summary>
        /// <param name="format">The exeption format.</param>
        /// <param name="args">The arguments.</param>
        public UserValidationException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserValidationException"/> class.
        /// </summary>
        /// <param name="message">The exeption message.</param>
        /// <param name="innerExeption">The inner exeption.</param>
        public UserValidationException(string message, Exception innerExeption)
            : base(message, innerExeption)
        {
        }
    }
}
