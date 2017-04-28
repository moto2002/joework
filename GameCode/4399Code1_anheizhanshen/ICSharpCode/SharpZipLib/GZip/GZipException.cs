namespace ICSharpCode.SharpZipLib.GZip
{
    using ICSharpCode.SharpZipLib;
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class GZipException : SharpZipBaseException
    {
        public GZipException()
        {
        }

        public GZipException(string message) : base(message)
        {
        }

        protected GZipException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public GZipException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

