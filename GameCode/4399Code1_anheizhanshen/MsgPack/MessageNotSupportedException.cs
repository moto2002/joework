namespace MsgPack
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public sealed class MessageNotSupportedException : Exception
    {
        public MessageNotSupportedException() : this(null)
        {
        }

        public MessageNotSupportedException(string message) : this(message, null)
        {
        }

        private MessageNotSupportedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public MessageNotSupportedException(string message, Exception inner) : base(message ?? "Specified object is not supported.", inner)
        {
        }
    }
}

