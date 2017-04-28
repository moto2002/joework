namespace MsgPack.Serialization
{
    using System;

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple=false, Inherited=true)]
    public sealed class MessagePackMemberAttribute : Attribute
    {
        private readonly int _id;
        private MsgPack.Serialization.NilImplication _nilImplication;

        public MessagePackMemberAttribute(int id)
        {
            this._id = id;
        }

        public int Id
        {
            get
            {
                return this._id;
            }
        }

        public MsgPack.Serialization.NilImplication NilImplication
        {
            get
            {
                return this._nilImplication;
            }
            set
            {
                switch (value)
                {
                    case MsgPack.Serialization.NilImplication.MemberDefault:
                    case MsgPack.Serialization.NilImplication.Null:
                    case MsgPack.Serialization.NilImplication.Prohibit:
                        this._nilImplication = value;
                        return;
                }
                throw new ArgumentOutOfRangeException("value");
            }
        }
    }
}

