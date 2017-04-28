namespace MsgPack
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public sealed class MessagePackObjectEqualityComparer : EqualityComparer<MessagePackObject>
    {
        private static readonly MessagePackObjectEqualityComparer _instance = new MessagePackObjectEqualityComparer();

        public sealed override bool Equals(MessagePackObject x, MessagePackObject y)
        {
            return x.Equals(y);
        }

        public sealed override int GetHashCode(MessagePackObject obj)
        {
            return obj.GetHashCode();
        }

        internal static MessagePackObjectEqualityComparer Instance
        {
            get
            {
                return _instance;
            }
        }
    }
}

