namespace MsgPack.Serialization.DefaultSerializers
{
    using MsgPack;
    using MsgPack.Serialization;
    using MsgPack.Serialization.EmittingSerializers;
    using MsgPack.Serialization.Reflection;
    using System;
    using System.Globalization;
    using System.Reflection;
    using System.Reflection.Emit;

    internal sealed class NullableMessagePackSerializer<T> : MessagePackSerializer<T>
    {
        private static readonly PropertyInfo _nullableTHasValueProperty;
        private static readonly MethodInfo _nullableTImplicitOperator;
        private static readonly PropertyInfo _nullableTValueProperty;
        private readonly MessagePackSerializer<T> _underlying;

        static NullableMessagePackSerializer()
        {
            NullableMessagePackSerializer<T>._nullableTHasValueProperty = NullableMessagePackSerializer<T>.GetOnlyWhenNullable<PropertyInfo>(typeof(Nullable<>).MakeGenericType(new Type[] { Nullable.GetUnderlyingType(typeof(T)) }).GetProperty("HasValue"));
            NullableMessagePackSerializer<T>._nullableTValueProperty = NullableMessagePackSerializer<T>.GetOnlyWhenNullable<PropertyInfo>(typeof(Nullable<>).MakeGenericType(new Type[] { Nullable.GetUnderlyingType(typeof(T)) }).GetProperty("Value"));
            NullableMessagePackSerializer<T>._nullableTImplicitOperator = NullableMessagePackSerializer<T>.GetOnlyWhenNullable<MethodInfo>(typeof(Nullable<>).MakeGenericType(new Type[] { Nullable.GetUnderlyingType(typeof(T)) }).GetMethod("op_Implicit", new Type[] { Nullable.GetUnderlyingType(typeof(T)) }));
        }

        public NullableMessagePackSerializer(SerializationContext context) : this(context, context.EmitterFlavor)
        {
        }

        internal NullableMessagePackSerializer(SerializationContext context, EmitterFlavor emitterFlavor)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (NullableMessagePackSerializer<T>._nullableTImplicitOperator == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "'{0}' is not nullable type.", new object[] { typeof(T) }));
            }
            SerializerEmitter emitter = SerializationMethodGeneratorManager.Get().CreateEmitter(typeof(T), emitterFlavor);
            NullableMessagePackSerializer<T>.CreatePacking(emitter);
            NullableMessagePackSerializer<T>.CreateUnpacking(emitter);
            this._underlying = emitter.CreateInstance<T>(context);
        }

        private static void CreatePacking(SerializerEmitter emitter)
        {
            Action<TracingILGenerator> loadValueEmitter = null;
            TracingILGenerator il = emitter.GetPackToMethodILGenerator();
            try
            {
                Label target = il.DefineLabel("END_IF");
                Label label2 = il.DefineLabel("END_METHOD");
                il.EmitAnyLdarga(2);
                il.EmitGetProperty(NullableMessagePackSerializer<T>._nullableTHasValueProperty);
                il.EmitBrtrue_S(target);
                il.EmitAnyLdarg(1);
                il.EmitAnyCall(NullableMessagePackSerializer.PackerPackNull);
                il.EmitPop();
                il.EmitBr_S(label2);
                il.MarkLabel(target);
                if (loadValueEmitter == null)
                {
                    loadValueEmitter = delegate (TracingILGenerator il0) {
                        il0.EmitAnyLdarga(2);
                        il.EmitGetProperty(NullableMessagePackSerializer<T>._nullableTValueProperty);
                    };
                }
                Emittion.EmitSerializeValue(emitter, il, 1, NullableMessagePackSerializer<T>._nullableTValueProperty.PropertyType, null, NilImplication.MemberDefault, loadValueEmitter, new LocalVariableHolder(il));
                il.MarkLabel(label2);
                il.EmitRet();
            }
            finally
            {
                il.FlushTrace();
                emitter.FlushTrace();
            }
        }

        private static void CreateUnpacking(SerializerEmitter emitter)
        {
            TracingILGenerator unpackFromMethodILGenerator = emitter.GetUnpackFromMethodILGenerator();
            try
            {
                LocalBuilder local = unpackFromMethodILGenerator.DeclareLocal(typeof(MessagePackObject?), "mayBeNullData");
                LocalBuilder builder2 = unpackFromMethodILGenerator.DeclareLocal(typeof(MessagePackObject), "data");
                LocalBuilder builder3 = unpackFromMethodILGenerator.DeclareLocal(typeof(T), "result");
                LocalBuilder result = unpackFromMethodILGenerator.DeclareLocal(NullableMessagePackSerializer<T>._nullableTValueProperty.PropertyType, "value");
                Label target = unpackFromMethodILGenerator.DefineLabel("END_IF");
                Label label2 = unpackFromMethodILGenerator.DefineLabel("END_METHOD");
                unpackFromMethodILGenerator.EmitAnyLdarg(1);
                unpackFromMethodILGenerator.EmitGetProperty(NullableMessagePackSerializer.UnpackerDataProperty);
                unpackFromMethodILGenerator.EmitAnyStloc(local);
                unpackFromMethodILGenerator.EmitAnyLdloca(local);
                unpackFromMethodILGenerator.EmitGetProperty(NullableMessagePackSerializer.Nullable_MessagePackObject_ValueProperty);
                unpackFromMethodILGenerator.EmitAnyStloc(builder2);
                unpackFromMethodILGenerator.EmitAnyLdloca(builder2);
                unpackFromMethodILGenerator.EmitGetProperty(NullableMessagePackSerializer.MessagePackObject_IsNilProperty);
                unpackFromMethodILGenerator.EmitBrfalse_S(target);
                unpackFromMethodILGenerator.EmitAnyLdloca(builder3);
                unpackFromMethodILGenerator.EmitInitobj(builder3.LocalType);
                unpackFromMethodILGenerator.EmitBr_S(label2);
                unpackFromMethodILGenerator.MarkLabel(target);
                Emittion.EmitUnpackFrom(emitter, unpackFromMethodILGenerator, result, 1);
                unpackFromMethodILGenerator.EmitAnyLdloc(result);
                unpackFromMethodILGenerator.EmitAnyCall(NullableMessagePackSerializer<T>._nullableTImplicitOperator);
                unpackFromMethodILGenerator.EmitAnyStloc(builder3);
                unpackFromMethodILGenerator.MarkLabel(label2);
                unpackFromMethodILGenerator.EmitAnyLdloc(builder3);
                unpackFromMethodILGenerator.EmitRet();
            }
            finally
            {
                unpackFromMethodILGenerator.FlushTrace();
                emitter.FlushTrace();
            }
        }

        private static TValue GetOnlyWhenNullable<TValue>(TValue value)
        {
            if (typeof(T).GetIsGenericType() && (typeof(T).GetGenericTypeDefinition() == typeof(Nullable<>)))
            {
                return value;
            }
            return default(TValue);
        }

        protected internal sealed override void PackToCore(Packer packer, T value)
        {
            this._underlying.PackTo(packer, value);
        }

        protected internal sealed override T UnpackFromCore(Unpacker unpacker)
        {
            return this._underlying.UnpackFrom(unpacker);
        }
    }
}

