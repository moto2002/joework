namespace MsgPack.Serialization.EmittingSerializers
{
    using MsgPack;
    using MsgPack.Serialization;
    using MsgPack.Serialization.Metadata;
    using MsgPack.Serialization.Reflection;
    using System;
    using System.Diagnostics.Contracts;
    using System.Reflection;
    using System.Reflection.Emit;

    internal sealed class ContextBasedSerializerEmitter : SerializerEmitter
    {
        private readonly DynamicMethod _packToMethod;
        private readonly Type _targetType;
        private readonly DynamicMethod _unpackFromMethod;
        private DynamicMethod _unpackToMethod;

        public ContextBasedSerializerEmitter(Type targetType)
        {
            Contract.Requires(targetType != null);
            this._targetType = targetType;
            this._packToMethod = new DynamicMethod("PackToCore", null, new Type[] { typeof(SerializationContext), typeof(Packer), targetType });
            this._unpackFromMethod = new DynamicMethod("UnpackFromCore", targetType, new Type[] { typeof(SerializationContext), typeof(Unpacker) });
        }

        public override MessagePackSerializer<T> CreateInstance<T>(SerializationContext context)
        {
            Action<SerializationContext, Packer, T> packToCore = this._packToMethod.CreateDelegate(typeof(Action<SerializationContext, Packer, T>)) as Action<SerializationContext, Packer, T>;
            Func<SerializationContext, Unpacker, T> unpackFromCore = this._unpackFromMethod.CreateDelegate(typeof(Func<SerializationContext, Unpacker, T>)) as Func<SerializationContext, Unpacker, T>;
            Action<SerializationContext, Unpacker, T> unpackToCore = null;
            if (this._unpackToMethod != null)
            {
                unpackToCore = this._unpackToMethod.CreateDelegate(typeof(Action<SerializationContext, Unpacker, T>)) as Action<SerializationContext, Unpacker, T>;
            }
            return new CallbackMessagePackSerializer<T>(context, packToCore, unpackFromCore, unpackToCore);
        }

        public override TracingILGenerator GetPackToMethodILGenerator()
        {
            if (SerializerEmitter.IsTraceEnabled)
            {
                base.Trace.WriteLine();
                base.Trace.WriteLine("{0}::{1}", MethodBase.GetCurrentMethod(), this._packToMethod);
            }
            return new TracingILGenerator(this._packToMethod, base.Trace);
        }

        public override TracingILGenerator GetUnpackFromMethodILGenerator()
        {
            if (SerializerEmitter.IsTraceEnabled)
            {
                base.Trace.WriteLine();
                base.Trace.WriteLine("{0}::{1}", MethodBase.GetCurrentMethod(), this._unpackFromMethod);
            }
            return new TracingILGenerator(this._unpackFromMethod, base.Trace);
        }

        public override TracingILGenerator GetUnpackToMethodILGenerator()
        {
            if (this._unpackToMethod == null)
            {
                this._unpackToMethod = new DynamicMethod("UnpackToCore", null, new Type[] { typeof(SerializationContext), typeof(Unpacker), this._targetType });
            }
            if (SerializerEmitter.IsTraceEnabled)
            {
                base.Trace.WriteLine();
                base.Trace.WriteLine("{0}::{1}", MethodBase.GetCurrentMethod(), this._unpackToMethod);
            }
            return new TracingILGenerator(this._unpackToMethod, base.Trace);
        }

        public override Action<TracingILGenerator, int> RegisterSerializer(Type targetType)
        {
            return delegate (TracingILGenerator il, int contextIndex) {
                il.EmitAnyLdarg(contextIndex);
                il.EmitAnyCall(_SerializationContext.GetSerializer1_Method.MakeGenericMethod(new Type[] { targetType }));
            };
        }
    }
}

