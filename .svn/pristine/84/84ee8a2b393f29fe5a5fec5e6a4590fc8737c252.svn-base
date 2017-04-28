namespace MsgPack.Serialization.DefaultSerializers
{
    using System;
    using System.Reflection;

    internal static class NullableMessagePackSerializer
    {
        public static readonly PropertyInfo MessagePackObject_IsNilProperty = FromExpression.ToProperty<MessagePackObject, bool>(value => value.IsNil);
        public static readonly PropertyInfo Nullable_MessagePackObject_ValueProperty = FromExpression.ToProperty<MessagePackObject?, MessagePackObject>(value => value.Value);
        public static readonly MethodInfo PackerPackNull = FromExpression.ToMethod<Packer, Packer>(packer => packer.PackNull());
        public static readonly PropertyInfo UnpackerDataProperty = FromExpression.ToProperty<Unpacker, MessagePackObject?>(unpacker => unpacker.Data);
    }
}

