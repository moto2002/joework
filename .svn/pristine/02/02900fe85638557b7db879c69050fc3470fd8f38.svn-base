namespace MsgPack.Serialization.EmittingSerializers
{
    using MsgPack;
    using MsgPack.Serialization;
    using MsgPack.Serialization.Metadata;
    using MsgPack.Serialization.Reflection;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Reflection.Emit;

    internal sealed class FieldBasedSerializerEmitter : SerializerEmitter
    {
        private readonly ConstructorBuilder _constructorBuilder;
        private static readonly Type[] _constructorParameterTypes = new Type[] { typeof(SerializationContext) };
        private readonly bool _isDebuggable;
        private readonly MethodBuilder _packMethodBuilder;
        private readonly Dictionary<RuntimeTypeHandle, FieldBuilder> _serializers;
        private readonly TypeBuilder _typeBuilder;
        private readonly MethodBuilder _unpackFromMethodBuilder;
        private MethodBuilder _unpackToMethodBuilder;

        public FieldBasedSerializerEmitter(ModuleBuilder host, int? sequence, Type targetType, bool isDebuggable)
        {
            Contract.Requires(host != null);
            Contract.Requires(targetType != null);
            string name = string.Join(Type.Delimiter.ToString(), new string[] { typeof(SerializerEmitter).Namespace, "Generated", IdentifierUtility.EscapeTypeName(targetType) + "Serializer" + sequence });
            Tracer.Emit.TraceEvent(TraceEventType.Verbose, 0x66, "Create {0}", new object[] { name });
            this._typeBuilder = host.DefineType(name, TypeAttributes.BeforeFieldInit | TypeAttributes.UnicodeClass | TypeAttributes.Sealed | TypeAttributes.Public, typeof(MessagePackSerializer<>).MakeGenericType(new Type[] { targetType }));
            this._constructorBuilder = this._typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, _constructorParameterTypes);
            this._packMethodBuilder = this._typeBuilder.DefineMethod("PackToCore", MethodAttributes.HideBySig | MethodAttributes.Virtual | MethodAttributes.Final | MethodAttributes.Family, CallingConventions.HasThis, typeof(void), new Type[] { typeof(Packer), targetType });
            this._unpackFromMethodBuilder = this._typeBuilder.DefineMethod("UnpackFromCore", MethodAttributes.HideBySig | MethodAttributes.Virtual | MethodAttributes.Final | MethodAttributes.Family, CallingConventions.HasThis, targetType, SerializerEmitter.UnpackFromCoreParameterTypes);
            this._typeBuilder.DefineMethodOverride(this._packMethodBuilder, this._typeBuilder.BaseType.GetMethod(this._packMethodBuilder.Name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance));
            this._typeBuilder.DefineMethodOverride(this._unpackFromMethodBuilder, this._typeBuilder.BaseType.GetMethod(this._unpackFromMethodBuilder.Name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance));
            this._serializers = new Dictionary<RuntimeTypeHandle, FieldBuilder>();
            this._isDebuggable = isDebuggable;
        }

        private ConstructorInfo Create()
        {
            if (!this._typeBuilder.IsCreated())
            {
                ILGenerator iLGenerator = this._constructorBuilder.GetILGenerator();
                iLGenerator.Emit(OpCodes.Ldarg_0);
                iLGenerator.Emit(OpCodes.Call, this._typeBuilder.BaseType.GetConstructor(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null));
                foreach (KeyValuePair<RuntimeTypeHandle, FieldBuilder> pair in this._serializers)
                {
                    Type typeFromHandle = Type.GetTypeFromHandle(pair.Key);
                    MethodInfo meth = _SerializationContext.GetSerializer1_Method.MakeGenericMethod(new Type[] { typeFromHandle });
                    iLGenerator.Emit(OpCodes.Ldarg_0);
                    iLGenerator.Emit(OpCodes.Ldarg_1);
                    iLGenerator.Emit(OpCodes.Callvirt, meth);
                    iLGenerator.Emit(OpCodes.Stfld, pair.Value);
                }
                iLGenerator.Emit(OpCodes.Ret);
            }
            return this._typeBuilder.CreateType().GetConstructor(_constructorParameterTypes);
        }

        public sealed override MessagePackSerializer<T> CreateInstance<T>(SerializationContext context)
        {
            ParameterExpression expression;
            return Expression.Lambda<Func<SerializationContext, MessagePackSerializer<T>>>(Expression.New(this.Create(), new Expression[] { expression = Expression.Parameter(typeof(SerializationContext), "context") }), new ParameterExpression[] { expression }).Compile()(context);
        }

        public sealed override TracingILGenerator GetPackToMethodILGenerator()
        {
            if (SerializerEmitter.IsTraceEnabled)
            {
                base.Trace.WriteLine();
                base.Trace.WriteLine("{0}->{1}::{2}", MethodBase.GetCurrentMethod(), this._typeBuilder.Name, this._packMethodBuilder);
            }
            return new TracingILGenerator(this._packMethodBuilder, base.Trace, this._isDebuggable);
        }

        public sealed override TracingILGenerator GetUnpackFromMethodILGenerator()
        {
            if (SerializerEmitter.IsTraceEnabled)
            {
                base.Trace.WriteLine();
                base.Trace.WriteLine("{0}->{1}::{2}", MethodBase.GetCurrentMethod(), this._typeBuilder.Name, this._unpackFromMethodBuilder);
            }
            return new TracingILGenerator(this._unpackFromMethodBuilder, base.Trace, this._isDebuggable);
        }

        public sealed override TracingILGenerator GetUnpackToMethodILGenerator()
        {
            if (SerializerEmitter.IsTraceEnabled)
            {
                base.Trace.WriteLine();
                base.Trace.WriteLine("{0}->{1}::{2}", MethodBase.GetCurrentMethod(), this._typeBuilder.Name, this._unpackToMethodBuilder);
            }
            if (this._unpackToMethodBuilder == null)
            {
                this._unpackToMethodBuilder = this._typeBuilder.DefineMethod("UnpackToCore", MethodAttributes.Virtual | MethodAttributes.Final | MethodAttributes.Family, CallingConventions.HasThis, null, new Type[] { typeof(Unpacker), this._unpackFromMethodBuilder.ReturnType });
                this._typeBuilder.DefineMethodOverride(this._unpackToMethodBuilder, this._typeBuilder.BaseType.GetMethod(this._unpackToMethodBuilder.Name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance));
            }
            return new TracingILGenerator(this._unpackToMethodBuilder, base.Trace, this._isDebuggable);
        }

        public sealed override Action<TracingILGenerator, int> RegisterSerializer(Type targetType)
        {
            FieldBuilder result;
            if (this._typeBuilder.IsCreated())
            {
                throw new InvalidOperationException("Type is already built.");
            }
            if (!this._serializers.TryGetValue(targetType.TypeHandle, out result))
            {
                result = this._typeBuilder.DefineField("_serializer" + this._serializers.Count, typeof(MessagePackSerializer<>).MakeGenericType(new Type[] { targetType }), FieldAttributes.InitOnly | FieldAttributes.Private);
                this._serializers.Add(targetType.TypeHandle, result);
            }
            return delegate (TracingILGenerator il, int thisIndex) {
                il.EmitAnyLdarg(thisIndex);
                il.EmitLdfld(result);
            };
        }
    }
}

