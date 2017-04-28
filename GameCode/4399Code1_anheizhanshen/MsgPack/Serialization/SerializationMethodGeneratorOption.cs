namespace MsgPack.Serialization
{
    using System;
    using System.ComponentModel;

    public enum SerializationMethodGeneratorOption
    {
        CanCollect = 1,
        [EditorBrowsable(EditorBrowsableState.Never)]
        CanDump = 0,
        Fast = 2
    }
}

