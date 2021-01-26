using System;

namespace Rangle.Abstractions.Exceptions
{
    [Serializable]
    public class InvalidEntityException : Exception
    {
        public InvalidEntityException(string message) : base(message) { }
        public InvalidEntityException(string message, Exception inner) : base(message, inner) { }

        protected InvalidEntityException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}