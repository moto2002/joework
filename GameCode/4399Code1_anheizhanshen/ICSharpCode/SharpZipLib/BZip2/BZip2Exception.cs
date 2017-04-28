namespace ICSharpCode.SharpZipLib.BZip2
{
    using ICSharpCode.SharpZipLib;
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class BZip2Exception : SharpZipBaseException
    {
        public BZip2Exception()
        {
        }

        public BZip2Exception(string message) : base(message)
        {
        }

        protected BZip2Exception(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public BZip2Exception(string message, Exception exception) : base(message, exception)
        {
        }
    }
}

