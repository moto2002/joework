namespace MsgPack.Serialization
{
    using MsgPack;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;

    internal abstract class SerializerBuilder<TObject>
    {
        private readonly SerializationContext _context;

        protected SerializerBuilder(SerializationContext context)
        {
            Contract.Requires(context != null);
            this._context = context;
        }

        public abstract MessagePackSerializer<TObject> CreateArraySerializer();
        public abstract MessagePackSerializer<TObject> CreateMapSerializer();
        public MessagePackSerializer<TObject> CreateSerializer()
        {
            SerializingMember[] source = (from item in SerializerBuilder<TObject>.GetTargetMembers()
                orderby item.Contract.Id
                select item).ToArray<SerializingMember>();
            if (source.Length == 0)
            {
                throw SerializationExceptions.NewNoSerializableFieldsException(typeof(TObject));
            }
            if (source.All<SerializingMember>(item => item.Contract.Id == -1))
            {
                return this.CreateSerializer((from item in source
                    orderby item.Contract.Name
                    select item).ToArray<SerializingMember>());
            }
            Contract.Assert(source[0].Contract.Id >= 0);
            if (this.Context.CompatibilityOptions.OneBoundDataMemberOrder && (source[0].Contract.Id == 0))
            {
                throw new NotSupportedException("Cannot specify order value 0 on DataMemberAttribute when SerializationContext.CompatibilityOptions.OneBoundDataMemberOrder is set to true.");
            }
            List<SerializingMember> list = new List<SerializingMember>(source.Max<SerializingMember>(item => item.Contract.Id) + 1);
            int index = 0;
            for (int i = this.Context.CompatibilityOptions.OneBoundDataMemberOrder ? 1 : 0; index < source.Length; i++)
            {
                Contract.Assert(source[index].Contract.Id >= 0);
                if (source[index].Contract.Id < i)
                {
                    throw new SerializationException(string.Format(CultureInfo.CurrentCulture, "The member ID '{0}' is duplicated in the '{1}' type.", new object[] { source[index].Contract.Id, typeof(TObject) }));
                }
                while (source[index].Contract.Id > i)
                {
                    SerializingMember member = new SerializingMember();
                    list.Add(member);
                    i++;
                }
                list.Add(source[index]);
                index++;
            }
            return this.CreateSerializer(list.ToArray());
        }

        protected abstract MessagePackSerializer<TObject> CreateSerializer(SerializingMember[] entries);
        public abstract MessagePackSerializer<TObject> CreateTupleSerializer();
        private static IEnumerable<SerializingMember> GetTargetMembers()
        {
            MemberInfo[] infoArray = typeof(TObject).FindMembers(MemberTypes.Property | MemberTypes.Field, BindingFlags.Public | BindingFlags.Instance, (member, criteria) => true, null);
            MemberInfo[] infoArray2 = (from item in infoArray
                where Attribute.IsDefined(item, typeof(MessagePackMemberAttribute))
                select item).ToArray<MemberInfo>();
            if (infoArray2.Length > 0)
            {
                return (from member in infoArray2 select new SerializingMember(member, new DataMemberContract(member, member.GetCustomAttribute<MessagePackMemberAttribute>())));
            }
            if (typeof(TObject).IsDefined(typeof(DataContractAttribute)))
            {
                return (from member in infoArray
                    where member.IsDefined(typeof(DataMemberAttribute))
                    select new SerializingMember(member, new DataMemberContract(member, member.GetCustomAttribute<DataMemberAttribute>())));
            }
            return (from member in infoArray
                where !Attribute.IsDefined(member, typeof(NonSerializedAttribute))
                select new SerializingMember(member, new DataMemberContract(member)));
        }

        public SerializationContext Context
        {
            get
            {
                return this._context;
            }
        }
    }
}

