namespace MsgPack.Serialization.EmittingSerializers
{
    using MsgPack.Serialization;
    using MsgPack.Serialization.Metadata;
    using MsgPack.Serialization.Reflection;
    using System;

    internal sealed class MapEmittingSerializerBuilder<TObject> : EmittingSerializerBuilder<TObject>
    {
        public MapEmittingSerializerBuilder(SerializationContext context) : base(context)
        {
        }

        protected sealed override void EmitPackMembers(SerializerEmitter emitter, TracingILGenerator packerIL, SerializingMember[] entries)
        {
            LocalVariableHolder localHolder = new LocalVariableHolder(packerIL);
            packerIL.EmitAnyLdarg(1);
            packerIL.EmitAnyLdc_I4(entries.Length);
            packerIL.EmitAnyCall(_Packer.PackMapHeader);
            packerIL.EmitPop();
            SerializingMember[] memberArray = entries;
            for (int i = 0; i < memberArray.Length; i++)
            {
                Action<TracingILGenerator> loadValueEmitter = null;
                SerializingMember entry = memberArray[i];
                if (entry.Member != null)
                {
                    packerIL.EmitAnyLdarg(1);
                    packerIL.EmitLdstr(entry.Contract.Name);
                    packerIL.EmitAnyCall(_Packer.PackString);
                    packerIL.EmitPop();
                    if (loadValueEmitter == null)
                    {
                        loadValueEmitter = delegate (TracingILGenerator il0) {
                            if (typeof(TObject).IsValueType)
                            {
                                il0.EmitAnyLdarga(2);
                            }
                            else
                            {
                                il0.EmitAnyLdarg(2);
                            }
                            Emittion.EmitLoadValue(il0, entry.Member);
                        };
                    }
                    Emittion.EmitSerializeValue(emitter, packerIL, 1, entry.Member.GetMemberValueType(), null, NilImplication.MemberDefault, loadValueEmitter, localHolder);
                }
            }
            packerIL.EmitRet();
        }
    }
}

