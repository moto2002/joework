namespace Mogo.RPC
{
    using System;

    [Serializable]
    public class DefineParseException : Exception
    {
        public DefineParseException(string message) : base(message)
        {
        }

        public DefineParseException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

