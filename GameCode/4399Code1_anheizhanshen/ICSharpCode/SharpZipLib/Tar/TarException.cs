namespace ICSharpCode.SharpZipLib.Tar
{
    using ICSharpCode.SharpZipLib;
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class TarException : SharpZipBaseException
    {
        public TarException()
        {
        }

        public TarException(string message) : base(message)
        {
        }

        protected TarException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public TarException(string message, Exception exception) : base(message, exception)
        {
        }
    }
}

