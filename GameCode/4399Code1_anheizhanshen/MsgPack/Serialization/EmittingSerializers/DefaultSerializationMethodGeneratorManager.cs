namespace MsgPack.Serialization.EmittingSerializers
{
    using MsgPack.Serialization;
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Runtime.CompilerServices;
    using System.Threading;

    internal sealed class DefaultSerializationMethodGeneratorManager : SerializationMethodGeneratorManager
    {
        private readonly AssemblyBuilder _assembly;
        private static int _assemblySequence = -1;
        private static DefaultSerializationMethodGeneratorManager _canCollect = new DefaultSerializationMethodGeneratorManager(false, true, null);
        private static DefaultSerializationMethodGeneratorManager _canDump = new DefaultSerializationMethodGeneratorManager(true, false, null);
        private static readonly ConstructorInfo _debuggableAttributeCtor = typeof(DebuggableAttribute).GetConstructor(new Type[] { typeof(bool), typeof(bool) });
        private static readonly object[] _debuggableAttributeCtorArguments = new object[] { true, true };
        private static DefaultSerializationMethodGeneratorManager _fast = new DefaultSerializationMethodGeneratorManager(false, false, null);
        private readonly bool _isDebuggable;
        private readonly bool _isExternalAssemblyBuilder;
        private readonly ModuleBuilder _module;
        private readonly string _moduleFileName;
        private int _typeSequence;

        private DefaultSerializationMethodGeneratorManager(bool isDebuggable, bool isCollectable, AssemblyBuilder assemblyBuilder)
        {
            string name;
            this._typeSequence = -1;
            this._isDebuggable = isDebuggable;
            this._isExternalAssemblyBuilder = assemblyBuilder != null;
            if (assemblyBuilder != null)
            {
                name = assemblyBuilder.GetName(false).Name;
                this._assembly = assemblyBuilder;
            }
            else
            {
                name = typeof(DefaultSerializationMethodGeneratorManager).Namespace + ".GeneratedSerealizers" + Interlocked.Increment(ref _assemblySequence);
                AssemblyBuilder dedicatedAssemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName(name), isDebuggable ? AssemblyBuilderAccess.RunAndSave : AssemblyBuilderAccess.Run);
                SetUpAssemblyBuilderAttributes(dedicatedAssemblyBuilder, isDebuggable);
                this._assembly = dedicatedAssemblyBuilder;
            }
            this._moduleFileName = name + ".dll";
            if (isDebuggable)
            {
                this._module = this._assembly.DefineDynamicModule(name, this._moduleFileName, true);
            }
            else
            {
                this._module = this._assembly.DefineDynamicModule(name, true);
            }
        }

        public static SerializationMethodGeneratorManager Create(AssemblyBuilder assemblyBuilder)
        {
            return new DefaultSerializationMethodGeneratorManager(true, false, assemblyBuilder);
        }

        protected sealed override SerializerEmitter CreateEmitterCore(Type targetType, EmitterFlavor emitterFlavor)
        {
            switch (emitterFlavor)
            {
                case EmitterFlavor.FieldBased:
                    return new FieldBasedSerializerEmitter(this._module, this._isExternalAssemblyBuilder ? null : new int?(Interlocked.Increment(ref this._typeSequence)), targetType, this._isDebuggable);
            }
            return new ContextBasedSerializerEmitter(targetType);
        }

        public static void DumpTo()
        {
            _canDump.DumpToCore();
        }

        private void DumpToCore()
        {
            this._assembly.Save(this._moduleFileName);
        }

        internal static void Refresh()
        {
            _canCollect = new DefaultSerializationMethodGeneratorManager(false, true, null);
            _canDump = new DefaultSerializationMethodGeneratorManager(true, false, null);
            _fast = new DefaultSerializationMethodGeneratorManager(false, false, null);
        }

        internal static void SetUpAssemblyBuilderAttributes(AssemblyBuilder dedicatedAssemblyBuilder, bool isDebuggable)
        {
            if (isDebuggable)
            {
                dedicatedAssemblyBuilder.SetCustomAttribute(new CustomAttributeBuilder(_debuggableAttributeCtor, _debuggableAttributeCtorArguments));
            }
            else
            {
                dedicatedAssemblyBuilder.SetCustomAttribute(new CustomAttributeBuilder(typeof(DebuggableAttribute).GetConstructor(new Type[] { typeof(DebuggableAttribute.DebuggingModes) }), new object[] { DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints }));
            }
            dedicatedAssemblyBuilder.SetCustomAttribute(new CustomAttributeBuilder(typeof(CompilationRelaxationsAttribute).GetConstructor(new Type[] { typeof(int) }), new object[] { 8 }));
        }

        public static DefaultSerializationMethodGeneratorManager CanCollect
        {
            get
            {
                return _canCollect;
            }
        }

        public static DefaultSerializationMethodGeneratorManager CanDump
        {
            get
            {
                return _canDump;
            }
        }

        public static DefaultSerializationMethodGeneratorManager Fast
        {
            get
            {
                return _fast;
            }
        }
    }
}

