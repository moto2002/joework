namespace MsgPack
{
    using MsgPack.Serialization;
    using System;
    using System.Linq.Expressions;
    using System.Runtime.CompilerServices;

    public static class PackerUnpackerExtensions
    {
        private static readonly Type[] _messagePackSerializer_Create_ParameterTypes = new Type[] { typeof(SerializationContext) };

        public static void Pack<T>(this Packer source, T value)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            PackCore<T>(source, value, new SerializationContext());
        }

        public static void Pack<T>(this Packer source, T value, SerializationContext context)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            PackCore<T>(source, value, context);
        }

        private static void PackCore<T>(Packer source, T value, SerializationContext context)
        {
            if (value == null)
            {
                source.PackNull();
            }
            else if ((typeof(T) != typeof(MessagePackObject)) && typeof(IPackable).IsAssignableFrom(typeof(T)))
            {
                (value as IPackable).PackToMessage(source, new PackingOptions());
            }
            else
            {
                MessagePackSerializer.Create<T>(context).PackTo(source, value);
            }
        }

        public static void PackObject(this Packer source, object value)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            PackObjectCore(source, value, new SerializationContext());
        }

        public static void PackObject(this Packer source, object value, SerializationContext context)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            PackObjectCore(source, value, context);
        }

        private static void PackObjectCore(Packer source, object value, SerializationContext context)
        {
            if (value == null)
            {
                source.PackNull();
            }
            else
            {
                ParameterExpression expression;
                ParameterExpression expression2;
                ParameterExpression expression3;
                Type type = value.GetType();
                Expression.Lambda<Action<SerializationContext, Packer, object>>(Expression.Call(Expression.Call(typeof(MessagePackSerializer).GetMethod("Create", _messagePackSerializer_Create_ParameterTypes).MakeGenericMethod(new Type[] { type }), new Expression[] { expression = Expression.Parameter(typeof(SerializationContext), "context") }), typeof(MessagePackSerializer<>).MakeGenericType(new Type[] { type }).GetMethod("PackTo"), new Expression[] { expression2 = Expression.Parameter(typeof(Packer), "packer"), Expression.Convert(expression3 = Expression.Parameter(typeof(object), "value"), type) }), new ParameterExpression[] { expression, expression2, expression3 }).Compile()(context, source, value);
            }
        }

        public static T Unpack<T>(this Unpacker source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return UnpackCore<T>(source, new SerializationContext());
        }

        public static T Unpack<T>(this Unpacker source, SerializationContext context)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            return UnpackCore<T>(source, context);
        }

        private static T UnpackCore<T>(Unpacker source, SerializationContext context)
        {
            return MessagePackSerializer.Create<T>(context).UnpackFrom(source);
        }
    }
}

