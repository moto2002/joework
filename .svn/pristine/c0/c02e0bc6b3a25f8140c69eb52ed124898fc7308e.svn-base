namespace MsgPack.Serialization
{
    using MsgPack.Serialization.EmittingSerializers;
    using System;
    using System.Reflection;
    using System.Reflection.Emit;

    public class SerializerGenerator
    {
        private readonly System.Reflection.AssemblyName _assemblyName;
        private SerializationMethod _method;
        private readonly Type _rootType;

        public SerializerGenerator(Type rootType, System.Reflection.AssemblyName assemblyName)
        {
            if (rootType == null)
            {
                throw new ArgumentNullException("rootType");
            }
            if (assemblyName == null)
            {
                throw new ArgumentNullException("assemblyName");
            }
            this._rootType = rootType;
            this._assemblyName = assemblyName;
            this._method = SerializationMethod.Array;
        }

        public void GenerateAssemblyFile()
        {
            SerializationContext context = new SerializationContext {
                EmitterFlavor = EmitterFlavor.FieldBased,
                GeneratorOption = SerializationMethodGeneratorOption.CanDump,
                SerializationMethod = this._method
            };
            AssemblyBuilder dedicatedAssemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(this._assemblyName, AssemblyBuilderAccess.RunAndSave);
            DefaultSerializationMethodGeneratorManager.SetUpAssemblyBuilderAttributes(dedicatedAssemblyBuilder, false);
            (Activator.CreateInstance(typeof(Builder).MakeGenericType(new Type[] { this._rootType })) as Builder).GenerateAssembly(context, dedicatedAssemblyBuilder);
        }

        public System.Reflection.AssemblyName AssemblyName
        {
            get
            {
                return this._assemblyName;
            }
        }

        public SerializationMethod Method
        {
            get
            {
                return this._method;
            }
            set
            {
                if ((value != SerializationMethod.Array) && (value != SerializationMethod.Map))
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                this._method = value;
            }
        }

        public Type RootType
        {
            get
            {
                return this._rootType;
            }
        }

        private abstract class Builder
        {
            protected Builder()
            {
            }

            public abstract void GenerateAssembly(SerializationContext context, AssemblyBuilder assemblyBuilder);
        }

        private class Builder<T> : SerializerGenerator.Builder
        {
            public override void GenerateAssembly(SerializationContext context, AssemblyBuilder assemblyBuilder)
            {
                EmittingSerializerBuilder<T> builder = (context.SerializationMethod == SerializationMethod.Array) ? ((EmittingSerializerBuilder<T>) new ArrayEmittingSerializerBuilder<T>(context)) : ((EmittingSerializerBuilder<T>) new MapEmittingSerializerBuilder<T>(context));
                builder.GeneratorManager = SerializationMethodGeneratorManager.Get(assemblyBuilder);
                builder.CreateSerializer();
                assemblyBuilder.Save(assemblyBuilder.GetName().Name + ".dll");
            }
        }
    }
}

