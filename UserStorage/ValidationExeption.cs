using System;

namespace UserStorage
{
    [Serializable]
    public class ValidationException : Exception
    {
        public ValidationException() : base() { }
        public ValidationException(string message) : base(message) { }
        public ValidationException(string format, params object[] args)
            : base(string.Format(format, args)) { }
        public ValidationException(string message, Exception innerExeption)
            : base(message,innerExeption) { }
    }
}
