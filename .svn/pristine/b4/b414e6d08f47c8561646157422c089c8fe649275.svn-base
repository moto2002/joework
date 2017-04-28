namespace MsgPack
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public sealed class UnpackException : Exception
    {
        public UnpackException() : this(null)
        {
        }

        public UnpackException(string message) : this(message, null)
        {
        }

        private UnpackException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public UnpackException(string message, Exception inner) : base(message ?? "Failed to unpacking.", inner)
        {
        }
    }
}

