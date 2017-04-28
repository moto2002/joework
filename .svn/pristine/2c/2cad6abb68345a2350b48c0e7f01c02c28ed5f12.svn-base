namespace MsgPack.Serialization
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;

    [StructLayout(LayoutKind.Sequential)]
    internal struct DataMemberContract
    {
        internal const int UnspecifiedId = -1;
        private readonly string _name;
        private readonly int _id;
        private readonly MsgPack.Serialization.NilImplication _nilImplication;
        public string Name
        {
            get
            {
                return this._name;
            }
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
        }
        public DataMemberContract(MemberInfo member)
        {
            Contract.Requires(member != null);
            this._name = member.Name;
            this._nilImplication = MsgPack.Serialization.NilImplication.MemberDefault;
            this._id = -1;
        }

        public DataMemberContract(MemberInfo member, DataMemberAttribute attribute)
        {
            Contract.Requires(member != null);
            Contract.Requires(attribute != null);
            this._name = string.IsNullOrEmpty(attribute.Name) ? member.Name : attribute.Name;
            this._nilImplication = MsgPack.Serialization.NilImplication.MemberDefault;
            this._id = attribute.Order;
        }

        public DataMemberContract(MemberInfo member, MessagePackMemberAttribute attribute)
        {
            Contract.Requires(member != null);
            Contract.Requires(attribute != null);
            if (attribute.Id < 0)
            {
                throw new SerializationException(string.Format(CultureInfo.CurrentCulture, "The member ID cannot be negative. The member is '{0}' in the '{1}' type.", new object[] { member.Name, member.DeclaringType }));
            }
            this._name = member.Name;
            this._nilImplication = attribute.NilImplication;
            this._id = attribute.Id;
        }
    }
}

