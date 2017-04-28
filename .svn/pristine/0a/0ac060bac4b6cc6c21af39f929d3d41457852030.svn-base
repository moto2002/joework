namespace MsgPack.Serialization.EmittingSerializers
{
    using MsgPack;
    using MsgPack.Serialization;
    using MsgPack.Serialization.Metadata;
    using MsgPack.Serialization.Reflection;
    using System;
    using System.Linq;
    using System.Reflection.Emit;

    internal abstract class EmittingSerializerBuilder<TObject> : SerializerBuilder<TObject>
    {
        private readonly EmitterFlavor _emitterFlavor;
        private SerializationMethodGeneratorManager _generatorManager;

        protected EmittingSerializerBuilder(SerializationContext context) : base(context)
        {
            this._emitterFlavor = context.EmitterFlavor;
            this._generatorManager = SerializationMethodGeneratorManager.Get(context.GeneratorOption);
        }

        public sealed override MessagePackSerializer<TObject> CreateArraySerializer()
        {
            MessagePackSerializer<TObject> serializer;
            using (SerializerEmitter emitter = EmittingSerializerBuilderLogics.CreateArraySerializerCore(typeof(TObject), this._emitterFlavor))
            {
                try
                {
                    serializer = emitter.CreateInstance<TObject>(base.Context);
                }
                finally
                {
                    emitter.FlushTrace();
                }
            }
            return serializer;
        }

        public sealed override MessagePackSerializer<TObject> CreateMapSerializer()
        {
            MessagePackSerializer<TObject> serializer;
            using (SerializerEmitter emitter = EmittingSerializerBuilderLogics.CreateMapSerializerCore(typeof(TObject), this._emitterFlavor))
            {
                try
                {
                    serializer = emitter.CreateInstance<TObject>(base.Context);
                }
                finally
                {
                    emitter.FlushTrace();
                }
            }
            return serializer;
        }

        protected sealed override MessagePackSerializer<TObject> CreateSerializer(SerializingMember[] entries)
        {
            MessagePackSerializer<TObject> serializer;
            using (SerializerEmitter emitter = this._generatorManager.CreateEmitter(typeof(TObject), this._emitterFlavor))
            {
                try
                {
                    TracingILGenerator packToMethodILGenerator = emitter.GetPackToMethodILGenerator();
                    try
                    {
                        if (typeof(IPackable).IsAssignableFrom(typeof(TObject)))
                        {
                            if (typeof(TObject).IsValueType)
                            {
                                packToMethodILGenerator.EmitAnyLdarga(2);
                            }
                            else
                            {
                                packToMethodILGenerator.EmitAnyLdarg(2);
                            }
                            packToMethodILGenerator.EmitAnyLdarg(1);
                            packToMethodILGenerator.EmitLdnull();
                            packToMethodILGenerator.EmitCall(typeof(TObject).GetInterfaceMap(typeof(IPackable)).TargetMethods.Single<MethodInfo>());
                            packToMethodILGenerator.EmitRet();
                        }
                        else
                        {
                            this.EmitPackMembers(emitter, packToMethodILGenerator, entries);
                        }
                    }
                    finally
                    {
                        packToMethodILGenerator.FlushTrace();
                    }
                    TracingILGenerator unpackFromMethodILGenerator = emitter.GetUnpackFromMethodILGenerator();
                    try
                    {
                        LocalBuilder target = unpackFromMethodILGenerator.DeclareLocal(typeof(TObject), "result");
                        Emittion.EmitConstruction(unpackFromMethodILGenerator, target, null);
                        if (typeof(IUnpackable).IsAssignableFrom(typeof(TObject)))
                        {
                            if (typeof(TObject).GetIsValueType())
                            {
                                unpackFromMethodILGenerator.EmitAnyLdloca(target);
                            }
                            else
                            {
                                unpackFromMethodILGenerator.EmitAnyLdloc(target);
                            }
                            unpackFromMethodILGenerator.EmitAnyLdarg(1);
                            unpackFromMethodILGenerator.EmitCall(typeof(TObject).GetInterfaceMap(typeof(IUnpackable)).TargetMethods.Single<MethodInfo>());
                        }
                        else
                        {
                            EmittingSerializerBuilder<TObject>.EmitUnpackMembers(emitter, unpackFromMethodILGenerator, entries, target);
                        }
                        unpackFromMethodILGenerator.EmitAnyLdloc(target);
                        unpackFromMethodILGenerator.EmitRet();
                    }
                    finally
                    {
                        unpackFromMethodILGenerator.FlushTrace();
                    }
                    serializer = emitter.CreateInstance<TObject>(base.Context);
                }
                finally
                {
                    emitter.FlushTrace();
                }
            }
            return serializer;
        }

        public sealed override MessagePackSerializer<TObject> CreateTupleSerializer()
        {
            throw new PlatformNotSupportedException();
        }

        protected abstract void EmitPackMembers(SerializerEmitter emitter, TracingILGenerator packerIL, SerializingMember[] entries);
        private static void EmitUnpackMembers(SerializerEmitter emitter, TracingILGenerator unpackerIL, SerializingMember[] entries, LocalBuilder result)
        {
            LocalVariableHolder localHolder = new LocalVariableHolder(unpackerIL);
            unpackerIL.EmitAnyLdarg(1);
            unpackerIL.EmitGetProperty(_Unpacker.IsArrayHeader);
            Label target = unpackerIL.DefineLabel("ELSE");
            Label label2 = unpackerIL.DefineLabel("END_IF");
            unpackerIL.EmitBrfalse(target);
            EmittingSerializerBuilder<TObject>.EmitUnpackMembersFromArray(emitter, unpackerIL, entries, result, localHolder);
            unpackerIL.EmitBr(label2);
            unpackerIL.MarkLabel(target);
            EmittingSerializerBuilder<TObject>.EmitUnpackMembersFromMap(emitter, unpackerIL, entries, result, localHolder);
            unpackerIL.MarkLabel(label2);
        }

        private static void EmitUnpackMembersFromArray(SerializerEmitter emitter, TracingILGenerator unpackerIL, SerializingMember[] entries, LocalBuilder result, LocalVariableHolder localHolder)
        {
            LocalBuilder itemsCount = localHolder.ItemsCount;
            LocalBuilder local = unpackerIL.DeclareLocal(typeof(int), "unpacked");
            Emittion.EmitGetUnpackerItemsCountAsInt32(unpackerIL, 1, localHolder);
            unpackerIL.EmitAnyStloc(itemsCount);
            for (int i = 0; i < entries.Length; i++)
            {
                Label endOfDeserialization = unpackerIL.DefineLabel("END_IF");
                Label target = unpackerIL.DefineLabel("ELSE");
                unpackerIL.EmitAnyLdloc(local);
                unpackerIL.EmitAnyLdloc(itemsCount);
                unpackerIL.EmitBlt(target);
                if (entries[i].Member != null)
                {
                    Emittion.EmitNilImplication(unpackerIL, 1, entries[i].Contract.Name, entries[i].Contract.NilImplication, endOfDeserialization, localHolder);
                }
                unpackerIL.EmitBr(endOfDeserialization);
                unpackerIL.MarkLabel(target);
                if (entries[i].Member == null)
                {
                    Emittion.EmitGeneralRead(unpackerIL, 1);
                }
                else if (UnpackHelpers.IsReadOnlyAppendableCollectionMember(entries[i].Member))
                {
                    Emittion.EmitDeserializeCollectionValue(emitter, unpackerIL, 1, result, entries[i].Member, entries[i].Member.GetMemberValueType(), entries[i].Contract.NilImplication, localHolder);
                }
                else
                {
                    Emittion.EmitDeserializeValue(emitter, unpackerIL, 1, result, entries[i], localHolder);
                }
                unpackerIL.EmitAnyLdloc(local);
                unpackerIL.EmitLdc_I4_1();
                unpackerIL.EmitAdd();
                unpackerIL.EmitAnyStloc(local);
                unpackerIL.MarkLabel(endOfDeserialization);
            }
        }

        private static void EmitUnpackMembersFromMap(SerializerEmitter emitter, TracingILGenerator unpackerIL, SerializingMember[] entries, LocalBuilder result, LocalVariableHolder localHolder)
        {
            Label label = unpackerIL.DefineLabel("BEGIN_LOOP");
            Label target = unpackerIL.DefineLabel("END_LOOP");
            unpackerIL.MarkLabel(label);
            LocalBuilder memberName = localHolder.MemberName;
            unpackerIL.EmitAnyLdarg(1);
            unpackerIL.EmitAnyLdloca(memberName);
            unpackerIL.EmitAnyCall(_Unpacker.ReadString);
            unpackerIL.EmitBrfalse(target);
            LocalBuilder unpackedData = localHolder.UnpackedData;
            LocalBuilder unpackedDataValue = localHolder.UnpackedDataValue;
            for (int i = 0; i < entries.Length; i++)
            {
                if (entries[i].Contract.Name != null)
                {
                    unpackerIL.EmitAnyLdloc(memberName);
                    unpackerIL.EmitLdstr(entries[i].Contract.Name);
                    unpackerIL.EmitAnyCall(_String.op_Equality);
                    Label label3 = unpackerIL.DefineLabel("END_IF_MEMBER_" + i);
                    unpackerIL.EmitBrfalse(label3);
                    if (entries[i].Member == null)
                    {
                        Emittion.EmitGeneralRead(unpackerIL, 1);
                    }
                    else if (UnpackHelpers.IsReadOnlyAppendableCollectionMember(entries[i].Member))
                    {
                        Emittion.EmitDeserializeCollectionValue(emitter, unpackerIL, 1, result, entries[i].Member, entries[i].Member.GetMemberValueType(), entries[i].Contract.NilImplication, localHolder);
                    }
                    else
                    {
                        Emittion.EmitDeserializeValue(emitter, unpackerIL, 1, result, entries[i], localHolder);
                    }
                    unpackerIL.EmitBr(label);
                    unpackerIL.MarkLabel(label3);
                }
            }
            unpackerIL.EmitAnyLdarg(1);
            unpackerIL.EmitCallvirt(_Unpacker.Read);
            unpackerIL.EmitPop();
            unpackerIL.EmitBr(label);
            unpackerIL.MarkLabel(target);
        }

        internal SerializationMethodGeneratorManager GeneratorManager
        {
            get
            {
                return this._generatorManager;
            }
            set
            {
                this._generatorManager = value;
            }
        }
    }
}

