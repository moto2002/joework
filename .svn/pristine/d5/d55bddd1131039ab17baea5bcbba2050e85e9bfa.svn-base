namespace MsgPack
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public sealed class UnassignedMessageTypeException : MessageTypeException
    {
        public UnassignedMessageTypeException() : this(null)
        {
        }

        public UnassignedMessageTypeException(string message) : this(message, null)
        {
        }

        private UnassignedMessageTypeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public UnassignedMessageTypeException(string message, Exception inner) : base(message ?? "Invalid message type.", inner)
        {
        }
    }
}

