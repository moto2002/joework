namespace MsgPack.Serialization
{
    using System;
    using System.Runtime.CompilerServices;

    public sealed class SerializationCompatibilityOptions
    {
        internal SerializationCompatibilityOptions()
        {
        }

        public bool OneBoundDataMemberOrder { get; set; }
    }
}

