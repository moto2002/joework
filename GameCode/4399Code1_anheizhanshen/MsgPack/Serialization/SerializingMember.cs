namespace MsgPack.Serialization
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct SerializingMember
    {
        public readonly MemberInfo Member;
        public readonly DataMemberContract Contract;
        public SerializingMember(MemberInfo member, DataMemberContract contract)
        {
            this.Member = member;
            this.Contract = contract;
        }
    }
}

