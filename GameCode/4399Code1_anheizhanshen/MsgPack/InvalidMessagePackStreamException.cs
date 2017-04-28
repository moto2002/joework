namespace MsgPack
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public sealed class InvalidMessagePackStreamException : Exception
    {
        public InvalidMessagePackStreamException() : this(null)
        {
        }

        public InvalidMessagePackStreamException(string message) : this(message, null)
        {
        }

        private InvalidMessagePackStreamException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public InvalidMessagePackStreamException(string message, Exception inner) : base(message ?? "Stream is not valid as serialized Message Pack object.", inner)
        {
        }
    }
}

