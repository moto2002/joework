namespace ICSharpCode.SharpZipLib.Tar
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class InvalidHeaderException : TarException
    {
        public InvalidHeaderException()
        {
        }

        public InvalidHeaderException(string message) : base(message)
        {
        }

        protected InvalidHeaderException(SerializationInfo information, StreamingContext context) : base(information, context)
        {
        }

        public InvalidHeaderException(string message, Exception exception) : base(message, exception)
        {
        }
    }
}

