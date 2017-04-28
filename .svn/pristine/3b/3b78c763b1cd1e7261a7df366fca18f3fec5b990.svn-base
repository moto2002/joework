namespace MsgPack
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class MessageTypeException : Exception
    {
        public MessageTypeException() : this(null)
        {
        }

        public MessageTypeException(string message) : this(message, null)
        {
        }

        protected MessageTypeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public MessageTypeException(string message, Exception inner) : base(message ?? "Invalid message type.", inner)
        {
        }
    }
}

