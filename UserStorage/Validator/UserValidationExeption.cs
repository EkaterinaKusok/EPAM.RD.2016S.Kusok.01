using System;

namespace UserStorage.Validator
{
    [Serializable]
    public class UserValidationException : Exception
    {
        public UserValidationException() : base() { }
        public UserValidationException(string message) : base(message) { }
        public UserValidationException(string format, params object[] args)
            : base(string.Format(format, args)) { }
        public UserValidationException(string message, Exception innerExeption)
            : base(message,innerExeption) { }
    }
}
