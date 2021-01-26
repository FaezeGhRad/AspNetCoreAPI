using System;

namespace Rangle.Abstractions.Exceptions
{
    [Serializable]
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string key) : base("Entity not found - id:" + key) { }
        public EntityNotFoundException(string key, Exception inner) : base("Entity not found - id:" + key, inner) { }

        protected EntityNotFoundException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}