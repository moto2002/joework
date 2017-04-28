namespace MsgPack.Serialization.EmittingSerializers
{
    using MsgPack.Serialization;
    using MsgPack.Serialization.Metadata;
    using MsgPack.Serialization.Reflection;
    using System;

    internal sealed class ArrayEmittingSerializerBuilder<TObject> : EmittingSerializerBuilder<TObject>
    {
        public ArrayEmittingSerializerBuilder(SerializationContext context) : base(context)
        {
        }

        protected override void EmitPackMembers(SerializerEmitter emitter, TracingILGenerator packerIL, SerializingMember[] entries)
        {
            LocalVariableHolder localHolder = new LocalVariableHolder(packerIL);
            packerIL.EmitAnyLdarg(1);
            packerIL.EmitAnyLdc_I4(entries.Length);
            packerIL.EmitAnyCall(_Packer.PackArrayHeader);
            packerIL.EmitPop();
            SerializingMember[] memberArray = entries;
            for (int i = 0; i < memberArray.Length; i++)
            {
                Action<TracingILGenerator> loadValueEmitter = null;
                SerializingMember member = memberArray[i];
                if (member.Member == null)
                {
                    packerIL.EmitAnyLdarg(1);
                    packerIL.EmitAnyCall(_Packer.PackNull);
                    packerIL.EmitPop();
                }
                else
                {
                    if (loadValueEmitter == null)
                    {
                        loadValueEmitter = delegate (TracingILGenerator il) {
                            if (typeof(TObject).IsValueType)
                            {
                                il.EmitAnyLdarga(2);
                            }
                            else
                            {
                                il.EmitAnyLdarg(2);
                            }
                            Emittion.EmitLoadValue(il, member.Member);
                        };
                    }
                    Emittion.EmitSerializeValue(emitter, packerIL, 1, member.Member.GetMemberValueType(), member.Member.Name, member.Contract.NilImplication, loadValueEmitter, localHolder);
                }
            }
            packerIL.EmitRet();
        }
    }
}

