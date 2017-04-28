namespace MsgPack.Serialization.DefaultSerializers
{
    using MsgPack;
    using MsgPack.Serialization;
    using System;
    using System.Globalization;

    internal sealed class EnumMessagePackSerializer<T> : MessagePackSerializer<T>
    {
        private static readonly Func<Unpacker, T> _unpacking;

        static EnumMessagePackSerializer()
        {
            EnumMessagePackSerializer<T>._unpacking = EnumMessagePackSerializer<T>.InitializeUnpacking();
        }

        public EnumMessagePackSerializer()
        {
            if (!typeof(T).GetIsEnum())
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Type '{0}' is not enum.", new object[] { typeof(T) }));
            }
        }

        private static Func<Unpacker, T> InitializeUnpacking()
        {
            if (typeof(T).GetIsValueType())
            {
                return (Delegate.CreateDelegate(typeof(Func<Unpacker, T>), EnumMessagePackSerializer.Unmarshal1Method.MakeGenericMethod(new Type[] { typeof(T) })) as Func<Unpacker, T>);
            }
            return delegate (Unpacker _) {
                throw new NotSupportedException();
            };
        }

        protected internal sealed override void PackToCore(Packer packer, T value)
        {
            packer.PackString(value.ToString());
        }

        protected internal sealed override T UnpackFromCore(Unpacker unpacker)
        {
            return EnumMessagePackSerializer<T>._unpacking(unpacker);
        }
    }
}

