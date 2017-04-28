namespace MsgPack.Serialization.EmittingSerializers
{
    using MsgPack;
    using MsgPack.Serialization;
    using MsgPack.Serialization.Metadata;
    using MsgPack.Serialization.Reflection;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Runtime.Serialization;

    internal static class Emittion
    {
        private static readonly Type[] _ctor_Int32_ParameterTypes = new Type[] { typeof(int) };

        private static void EmitCompareNull(TracingILGenerator il, LocalBuilder value, Label targetIfNotNull)
        {
            if (value.LocalType == typeof(MessagePackObject))
            {
                il.EmitAnyLdloca(value);
                il.EmitGetProperty(_MessagePackObject.IsNil);
                il.EmitBrfalse_S(targetIfNotNull);
            }
            else if (value.LocalType.GetIsValueType())
            {
                Contract.Assert(Nullable.GetUnderlyingType(value.LocalType) != null, value.LocalType.FullName);
                il.EmitAnyLdloca(value);
                il.EmitGetProperty(value.LocalType.GetProperty("HasValue"));
                il.EmitBrtrue_S(targetIfNotNull);
            }
            else
            {
                il.EmitAnyLdloc(value);
                il.EmitLdnull();
                il.EmitBne_Un_S(targetIfNotNull);
            }
        }

        public static void EmitConstruction(TracingILGenerator il, LocalBuilder target, Action<TracingILGenerator> initialCountLoadingEmitter)
        {
            Contract.Requires(il != null);
            Contract.Requires(target != null);
            if (target.LocalType.IsArray)
            {
                Contract.Assert(initialCountLoadingEmitter != null);
                initialCountLoadingEmitter(il);
                il.EmitNewarr(target.LocalType.GetElementType());
                il.EmitAnyStloc(target);
            }
            else
            {
                ConstructorInfo constructor = target.LocalType.GetConstructor(_ctor_Int32_ParameterTypes);
                if (((constructor != null) && (initialCountLoadingEmitter != null)) && typeof(IEnumerable).IsAssignableFrom(target.LocalType))
                {
                    if (target.LocalType.IsValueType)
                    {
                        LocalBuilder local = il.DeclareLocal(typeof(int), "capacity");
                        initialCountLoadingEmitter(il);
                        il.EmitAnyStloc(local);
                        il.EmitAnyLdloca(target);
                        il.EmitAnyLdloc(local);
                        il.EmitCallConstructor(constructor);
                    }
                    else
                    {
                        initialCountLoadingEmitter(il);
                        il.EmitNewobj(constructor);
                        il.EmitAnyStloc(target);
                    }
                }
                else if (!target.LocalType.IsValueType)
                {
                    constructor = target.LocalType.GetConstructor(Type.EmptyTypes);
                    if (constructor == null)
                    {
                        throw SerializationExceptions.NewTargetDoesNotHavePublicDefaultConstructorNorInitialCapacity(target.LocalType);
                    }
                    il.EmitNewobj(constructor);
                    il.EmitAnyStloc(target);
                }
            }
        }

        public static void EmitDeserializeCollectionValue(SerializerEmitter emitter, TracingILGenerator il, int unpackerArgumentIndex, LocalBuilder target, MemberInfo member, Type memberType, NilImplication nilImplication, LocalVariableHolder localHolder)
        {
            LocalBuilder unpackedData;
            LocalBuilder unpackedDataValue;
            Label label2;
            Label label3;
            Contract.Requires(emitter != null);
            Contract.Requires(il != null);
            Contract.Requires(unpackerArgumentIndex >= 0);
            Contract.Requires(target != null);
            Contract.Requires(member != null);
            Contract.Requires(memberType != null);
            Contract.Requires(localHolder != null);
            Label label = il.DefineLabel("END_OF_DESERIALIZATION");
            EmitGeneralRead(il, unpackerArgumentIndex);
            switch (nilImplication)
            {
                case NilImplication.MemberDefault:
                    il.EmitAnyLdarg(unpackerArgumentIndex);
                    il.EmitGetProperty(_Unpacker.Data);
                    unpackedData = localHolder.UnpackedData;
                    il.EmitAnyStloc(unpackedData);
                    il.EmitAnyLdloca(unpackedData);
                    il.EmitGetProperty(_Nullable<MessagePackObject>.Value);
                    unpackedDataValue = localHolder.UnpackedDataValue;
                    il.EmitAnyStloc(unpackedDataValue);
                    il.EmitAnyLdloca(unpackedDataValue);
                    il.EmitGetProperty(_MessagePackObject.IsNil);
                    il.EmitBrtrue(label);
                    goto Label_01B5;

                case NilImplication.Null:
                case NilImplication.Prohibit:
                    il.EmitAnyLdarg(unpackerArgumentIndex);
                    il.EmitGetProperty(_Unpacker.Data);
                    unpackedData = localHolder.UnpackedData;
                    il.EmitAnyStloc(unpackedData);
                    il.EmitAnyLdloca(unpackedData);
                    il.EmitGetProperty(_Nullable<MessagePackObject>.Value);
                    unpackedDataValue = localHolder.UnpackedDataValue;
                    il.EmitAnyStloc(unpackedDataValue);
                    il.EmitAnyLdloca(unpackedDataValue);
                    il.EmitGetProperty(_MessagePackObject.IsNil);
                    label2 = il.DefineLabel("END_IF0");
                    il.EmitBrfalse_S(label2);
                    il.EmitLdstr(member.Name);
                    if (nilImplication != NilImplication.Prohibit)
                    {
                        il.EmitAnyCall(SerializationExceptions.NewReadOnlyMemberItemsMustNotBeNullMethod);
                        break;
                    }
                    il.EmitAnyCall(SerializationExceptions.NewNullIsProhibitedMethod);
                    break;

                default:
                    goto Label_01B5;
            }
            il.EmitThrow();
            il.MarkLabel(label2);
        Label_01B5:
            label3 = il.DefineLabel("THEN");
            Action<TracingILGenerator, int> action = emitter.RegisterSerializer(memberType);
            il.EmitAnyLdarg(unpackerArgumentIndex);
            il.EmitGetProperty(_Unpacker.IsArrayHeader);
            il.EmitBrtrue_S(label3);
            il.EmitAnyLdarg(unpackerArgumentIndex);
            il.EmitGetProperty(_Unpacker.IsMapHeader);
            il.EmitBrtrue_S(label3);
            il.EmitLdstr(member.Name);
            il.EmitAnyCall(SerializationExceptions.NewStreamDoesNotContainCollectionForMemberMethod);
            il.EmitThrow();
            LocalBuilder subtreeUnpacker = localHolder.SubtreeUnpacker;
            il.MarkLabel(label3);
            EmitUnpackerBeginReadSubtree(il, unpackerArgumentIndex, subtreeUnpacker);
            action(il, 0);
            il.EmitAnyLdloc(subtreeUnpacker);
            il.EmitAnyLdloc(target);
            EmitLoadValue(il, member);
            il.EmitAnyCall(typeof(MessagePackSerializer<>).MakeGenericType(new Type[] { memberType }).GetMethod("UnpackTo", new Type[] { typeof(Unpacker), memberType }));
            EmitUnpackerEndReadSubtree(il, subtreeUnpacker);
            il.MarkLabel(label);
        }

        public static void EmitDeserializeValue(SerializerEmitter emitter, TracingILGenerator il, int unpackerArgumentIndex, LocalBuilder result, SerializingMember member, LocalVariableHolder localHolder)
        {
            LocalBuilder deserializedValue;
            Contract.Requires(emitter != null);
            Contract.Requires(il != null);
            Contract.Requires(unpackerArgumentIndex >= 0);
            Contract.Requires(result != null);
            Label endOfDeserialization = il.DefineLabel("END_OF_DESERIALIZATION");
            bool flag = false;
            if (member.Member.GetMemberValueType().GetIsValueType() && (Nullable.GetUnderlyingType(member.Member.GetMemberValueType()) == null))
            {
                deserializedValue = localHolder.GetDeserializedValue(typeof(Nullable<>).MakeGenericType(new Type[] { member.Member.GetMemberValueType() }));
                flag = true;
            }
            else
            {
                deserializedValue = localHolder.GetDeserializedValue(member.Member.GetMemberValueType());
            }
            if (deserializedValue.LocalType.GetIsValueType())
            {
                il.EmitAnyLdloca(deserializedValue);
                il.EmitInitobj(deserializedValue.LocalType);
            }
            else
            {
                il.EmitLdnull();
                il.EmitAnyStloc(deserializedValue);
            }
            EmitDeserializeValueCore(emitter, il, unpackerArgumentIndex, deserializedValue, result.LocalType, new SerializingMember?(member), member.Contract.Name, endOfDeserialization, localHolder);
            if (result.LocalType.IsValueType)
            {
                il.EmitAnyLdloca(result);
            }
            else
            {
                il.EmitAnyLdloc(result);
            }
            if (flag)
            {
                il.EmitAnyLdloca(deserializedValue);
                il.EmitGetProperty(typeof(Nullable<>).MakeGenericType(new Type[] { member.Member.GetMemberValueType() }).GetProperty("Value"));
            }
            else
            {
                il.EmitAnyLdloc(deserializedValue);
            }
            EmitStoreValue(il, member.Member);
            il.MarkLabel(endOfDeserialization);
        }

        private static void EmitDeserializeValueCore(SerializerEmitter emitter, TracingILGenerator il, int unpackerArgumentIndex, LocalBuilder value, Type targetType, SerializingMember? member, string memberName, Label endOfDeserialization, LocalVariableHolder localHolder)
        {
            MethodInfo directReadMethod = _Unpacker.GetDirectReadMethod(value.LocalType);
            if ((directReadMethod != null) && (!member.HasValue || !UnpackHelpers.IsReadOnlyAppendableCollectionMember(member.Value.Member)))
            {
                LocalBuilder isDeserializationSucceeded = localHolder.IsDeserializationSucceeded;
                il.EmitLdc_I4_0();
                il.EmitAnyStloc(isDeserializationSucceeded);
                il.BeginExceptionBlock();
                il.EmitAnyLdarg(unpackerArgumentIndex);
                il.EmitAnyLdloca(value);
                il.EmitAnyCall(directReadMethod);
                il.EmitAnyStloc(isDeserializationSucceeded);
                il.BeginCatchBlock(typeof(MessageTypeException));
                LocalBuilder catchedException = localHolder.GetCatchedException(typeof(MessageTypeException));
                il.EmitAnyStloc(catchedException);
                il.EmitTypeOf(targetType);
                il.EmitLdstr(memberName);
                il.EmitAnyLdloc(catchedException);
                il.EmitAnyCall(SerializationExceptions.NewFailedToDeserializeMemberMethod);
                il.EmitThrow();
                il.EndExceptionBlock();
                Label target = il.DefineLabel("END_IF");
                il.EmitAnyLdloc(isDeserializationSucceeded);
                il.EmitBrtrue_S(target);
                il.EmitAnyCall(SerializationExceptions.NewUnexpectedEndOfStreamMethod);
                il.EmitThrow();
                il.MarkLabel(target);
                if (member.HasValue)
                {
                    EmitNilImplicationForPrimitive(il, member.Value, value, endOfDeserialization);
                }
            }
            else
            {
                EmitGeneralRead(il, unpackerArgumentIndex);
                if (member.HasValue)
                {
                    EmitNilImplication(il, unpackerArgumentIndex, member.Value.Contract.Name, member.Value.Contract.NilImplication, endOfDeserialization, localHolder);
                }
                Label label2 = il.DefineLabel("THEN_IF_COLLECTION");
                Label label3 = il.DefineLabel("END_IF_COLLECTION");
                il.EmitAnyLdarg(unpackerArgumentIndex);
                il.EmitGetProperty(_Unpacker.IsArrayHeader);
                il.EmitAnyLdarg(unpackerArgumentIndex);
                il.EmitGetProperty(_Unpacker.IsMapHeader);
                il.EmitOr();
                il.EmitBrtrue_S(label2);
                EmitUnpackFrom(emitter, il, value, unpackerArgumentIndex);
                il.EmitBr_S(label3);
                LocalBuilder subtreeUnpacker = localHolder.SubtreeUnpacker;
                il.MarkLabel(label2);
                EmitUnpackerBeginReadSubtree(il, unpackerArgumentIndex, subtreeUnpacker);
                EmitUnpackFrom(emitter, il, value, subtreeUnpacker);
                EmitUnpackerEndReadSubtree(il, subtreeUnpacker);
                il.MarkLabel(label3);
            }
        }

        public static void EmitDeserializeValueWithoutNilImplication(SerializerEmitter emitter, TracingILGenerator il, int unpackerArgumentIndex, LocalBuilder value, Type targetType, string memberName, LocalVariableHolder localHolder)
        {
            Contract.Requires(emitter != null);
            Contract.Requires(il != null);
            Contract.Requires(unpackerArgumentIndex >= 0);
            Contract.Requires(value != null);
            Label endOfDeserialization = il.DefineLabel("END_OF_DESERIALIZATION");
            EmitDeserializeValueCore(emitter, il, unpackerArgumentIndex, value, targetType, null, memberName, endOfDeserialization, localHolder);
            il.MarkLabel(endOfDeserialization);
        }

        public static void EmitFor(TracingILGenerator il, LocalBuilder count, Action<TracingILGenerator, LocalBuilder> bodyEmitter)
        {
            Contract.Requires(il != null);
            Contract.Requires(count != null);
            Contract.Requires(bodyEmitter != null);
            LocalBuilder local = il.DeclareLocal(typeof(int), "i");
            il.EmitLdc_I4_0();
            il.EmitAnyStloc(local);
            Label target = il.DefineLabel("FOR_COND");
            il.EmitBr(target);
            Label label = il.DefineLabel("BODY");
            il.MarkLabel(label);
            bodyEmitter(il, local);
            il.EmitAnyLdloc(local);
            il.EmitLdc_I4_1();
            il.EmitAdd();
            il.EmitAnyStloc(local);
            il.MarkLabel(target);
            il.EmitAnyLdloc(local);
            il.EmitAnyLdloc(count);
            il.EmitBlt(label);
        }

        public static void EmitForEach(TracingILGenerator il, CollectionTraits traits, LocalBuilder collection, Action<TracingILGenerator, Action> bodyEmitter)
        {
            Contract.Requires(il != null);
            Contract.Requires(collection != null);
            Contract.Requires(bodyEmitter != null);
            LocalBuilder enumerator = il.DeclareLocal(traits.GetEnumeratorMethod.ReturnType, "enumerator");
            if (collection.LocalType.IsValueType)
            {
                il.EmitAnyLdloca(collection);
            }
            else
            {
                il.EmitAnyLdloc(collection);
            }
            il.EmitAnyCall(traits.GetEnumeratorMethod);
            il.EmitAnyStloc(enumerator);
            if (typeof(IDisposable).IsAssignableFrom(traits.GetEnumeratorMethod.ReturnType))
            {
                il.BeginExceptionBlock();
            }
            Label label = il.DefineLabel("START_LOOP");
            il.MarkLabel(label);
            Label target = il.DefineLabel("END_LOOP");
            Type returnType = traits.GetEnumeratorMethod.ReturnType;
            MethodInfo method = returnType.GetMethod("MoveNext", Type.EmptyTypes);
            PropertyInfo currentProperty = traits.GetEnumeratorMethod.ReturnType.GetProperty("Current");
            if (method == null)
            {
                method = _IEnumerator.MoveNext;
            }
            if (currentProperty == null)
            {
                if (returnType == typeof(IDictionaryEnumerator))
                {
                    currentProperty = _IDictionaryEnumerator.Current;
                }
                else if (returnType.IsInterface)
                {
                    if (returnType.IsGenericType && (returnType.GetGenericTypeDefinition() == typeof(IEnumerator<>)))
                    {
                        currentProperty = typeof(IEnumerator<>).MakeGenericType(new Type[] { traits.ElementType }).GetProperty("Current");
                    }
                    else
                    {
                        currentProperty = _IEnumerator.Current;
                    }
                }
            }
            Contract.Assert(currentProperty != null, returnType.ToString());
            if (traits.GetEnumeratorMethod.ReturnType.IsValueType)
            {
                il.EmitAnyLdloca(enumerator);
            }
            else
            {
                il.EmitAnyLdloc(enumerator);
            }
            il.EmitAnyCall(method);
            il.EmitBrfalse(target);
            bodyEmitter(il, delegate {
                if (traits.GetEnumeratorMethod.ReturnType.IsValueType)
                {
                    il.EmitAnyLdloca(enumerator);
                }
                else
                {
                    il.EmitAnyLdloc(enumerator);
                }
                il.EmitGetProperty(currentProperty);
            });
            il.EmitBr(label);
            il.MarkLabel(target);
            if (typeof(IDisposable).IsAssignableFrom(traits.GetEnumeratorMethod.ReturnType))
            {
                il.BeginFinallyBlock();
                if (traits.GetEnumeratorMethod.ReturnType.IsValueType)
                {
                    MethodInfo info2 = traits.GetEnumeratorMethod.ReturnType.GetMethod("Dispose");
                    if (((info2 != null) && (info2.GetParameters().Length == 0)) && (info2.ReturnType == typeof(void)))
                    {
                        il.EmitAnyLdloca(enumerator);
                        il.EmitAnyCall(info2);
                    }
                    else
                    {
                        il.EmitAnyLdloc(enumerator);
                        il.EmitBox(traits.GetEnumeratorMethod.ReturnType);
                        il.EmitAnyCall(_IDisposable.Dispose);
                    }
                }
                else
                {
                    il.EmitAnyLdloc(enumerator);
                    il.EmitAnyCall(_IDisposable.Dispose);
                }
                il.EndExceptionBlock();
            }
        }

        public static void EmitGeneralRead(TracingILGenerator il, int unpackerArgumentIndex)
        {
            il.EmitAnyLdarg(unpackerArgumentIndex);
            il.EmitAnyCall(_Unpacker.Read);
            Label target = il.DefineLabel("END_IF");
            il.EmitBrtrue_S(target);
            il.EmitAnyCall(SerializationExceptions.NewUnexpectedEndOfStreamMethod);
            il.EmitThrow();
            il.MarkLabel(target);
        }

        public static void EmitGetUnpackerItemsCountAsInt32(TracingILGenerator il, int unpackerArgumentIndex, LocalVariableHolder localHolder)
        {
            Contract.Requires(il != null);
            Contract.Requires(unpackerArgumentIndex >= 0);
            il.EmitAnyLdloca(localHolder.RawItemsCount);
            il.EmitInitobj(typeof(long));
            il.BeginExceptionBlock();
            il.EmitAnyLdarg(unpackerArgumentIndex);
            il.EmitGetProperty(_Unpacker.ItemsCount);
            il.EmitAnyStloc(localHolder.RawItemsCount);
            il.BeginCatchBlock(typeof(InvalidOperationException));
            il.EmitAnyCall(SerializationExceptions.NewIsIncorrectStreamMethod);
            il.EmitThrow();
            il.EndExceptionBlock();
            il.EmitAnyLdloc(localHolder.RawItemsCount);
            il.EmitLdc_I8(0x7fffffffL);
            Label target = il.DefineLabel();
            il.EmitBle_S(target);
            il.EmitAnyCall(SerializationExceptions.NewIsTooLargeCollectionMethod);
            il.EmitThrow();
            il.MarkLabel(target);
            il.EmitAnyLdloc(localHolder.RawItemsCount);
            il.EmitConv_I4();
        }

        public static void EmitLoadValue(TracingILGenerator il, MemberInfo member)
        {
            Contract.Requires(il != null);
            Contract.Requires(member != null);
            PropertyInfo property = member as PropertyInfo;
            if (property != null)
            {
                il.EmitGetProperty(property);
            }
            else
            {
                Contract.Assert(member is FieldInfo, member.ToString() + ":" + member.MemberType);
                il.EmitLdfld(member as FieldInfo);
            }
        }

        public static void EmitNilImplication(TracingILGenerator il, int unpackerArgumentIndex, string memberName, NilImplication nilImplication, Label endOfDeserialization, LocalVariableHolder localHolder)
        {
            LocalBuilder unpackedData;
            LocalBuilder unpackedDataValue;
            switch (nilImplication)
            {
                case NilImplication.MemberDefault:
                    il.EmitAnyLdarg(unpackerArgumentIndex);
                    il.EmitGetProperty(_Unpacker.Data);
                    unpackedData = localHolder.UnpackedData;
                    il.EmitAnyStloc(unpackedData);
                    il.EmitAnyLdloca(unpackedData);
                    il.EmitGetProperty(_Nullable<MessagePackObject>.Value);
                    unpackedDataValue = localHolder.UnpackedDataValue;
                    il.EmitAnyStloc(unpackedDataValue);
                    il.EmitAnyLdloca(unpackedDataValue);
                    il.EmitGetProperty(_MessagePackObject.IsNil);
                    il.EmitBrtrue(endOfDeserialization);
                    break;

                case NilImplication.Prohibit:
                {
                    il.EmitAnyLdarg(unpackerArgumentIndex);
                    il.EmitGetProperty(_Unpacker.Data);
                    unpackedData = localHolder.UnpackedData;
                    il.EmitAnyStloc(unpackedData);
                    il.EmitAnyLdloca(unpackedData);
                    il.EmitGetProperty(_Nullable<MessagePackObject>.Value);
                    unpackedDataValue = localHolder.UnpackedDataValue;
                    il.EmitAnyStloc(unpackedDataValue);
                    il.EmitAnyLdloca(unpackedDataValue);
                    il.EmitGetProperty(_MessagePackObject.IsNil);
                    Label target = il.DefineLabel("END_IF0");
                    il.EmitBrfalse_S(target);
                    il.EmitLdstr(memberName);
                    il.EmitAnyCall(SerializationExceptions.NewNullIsProhibitedMethod);
                    il.EmitThrow();
                    il.MarkLabel(target);
                    break;
                }
            }
        }

        private static void EmitNilImplicationForPrimitive(TracingILGenerator il, SerializingMember member, LocalBuilder value, Label endOfDeserialization)
        {
            Label targetIfNotNull = il.DefineLabel("END_IF_NULL");
            EmitCompareNull(il, value, targetIfNotNull);
            switch (member.Contract.NilImplication)
            {
                case NilImplication.MemberDefault:
                    il.EmitBr(endOfDeserialization);
                    break;

                case NilImplication.Null:
                    if (member.Member.GetMemberValueType().GetIsValueType() && (Nullable.GetUnderlyingType(member.Member.GetMemberValueType()) == null))
                    {
                        il.EmitLdstr(member.Contract.Name);
                        il.EmitLdtoken(member.Member.GetMemberValueType());
                        il.EmitAnyCall(_Type.GetTypeFromHandle);
                        il.EmitLdtoken(member.Member.DeclaringType);
                        il.EmitAnyCall(_Type.GetTypeFromHandle);
                        il.EmitAnyCall(SerializationExceptions.NewValueTypeCannotBeNull3Method);
                        il.EmitThrow();
                    }
                    break;

                case NilImplication.Prohibit:
                    il.EmitLdstr(member.Contract.Name);
                    il.EmitAnyCall(SerializationExceptions.NewNullIsProhibitedMethod);
                    il.EmitThrow();
                    break;
            }
            il.MarkLabel(targetIfNotNull);
        }

        public static void EmitSerializeValue(SerializerEmitter emitter, TracingILGenerator il, int packerArgumentIndex, Type valueType, string memberName, NilImplication nilImplication, Action<TracingILGenerator> loadValueEmitter, LocalVariableHolder localHolder)
        {
            Contract.Requires(emitter != null);
            Contract.Requires(il != null);
            Contract.Requires(packerArgumentIndex >= 0);
            Contract.Requires(valueType != null);
            Contract.Requires(loadValueEmitter != null);
            LocalBuilder serializingValue = localHolder.GetSerializingValue(valueType);
            loadValueEmitter(il);
            il.EmitAnyStloc(serializingValue);
            if ((memberName != null) && (nilImplication == NilImplication.Prohibit))
            {
                Label label;
                if (!valueType.IsValueType)
                {
                    il.EmitAnyLdloc(serializingValue);
                    label = il.DefineLabel("END_IF");
                    il.EmitBrtrue_S(label);
                    il.EmitLdstr(memberName);
                    il.EmitAnyCall(SerializationExceptions.NewNullIsProhibitedMethod);
                    il.EmitThrow();
                    il.MarkLabel(label);
                }
                else if (Nullable.GetUnderlyingType(valueType) != null)
                {
                    il.EmitAnyLdloca(serializingValue);
                    il.EmitGetProperty(typeof(Nullable<>).MakeGenericType(new Type[] { Nullable.GetUnderlyingType(valueType) }).GetProperty("HasValue"));
                    label = il.DefineLabel("END_IF");
                    il.EmitBrtrue_S(label);
                    il.EmitLdstr(memberName);
                    il.EmitAnyCall(SerializationExceptions.NewNullIsProhibitedMethod);
                    il.EmitThrow();
                    il.MarkLabel(label);
                }
            }
            emitter.RegisterSerializer(valueType)(il, 0);
            il.EmitAnyLdarg(packerArgumentIndex);
            il.EmitAnyLdloc(serializingValue);
            il.EmitAnyCall(typeof(MessagePackSerializer<>).MakeGenericType(new Type[] { valueType }).GetMethod("PackTo"));
        }

        public static void EmitStoreValue(TracingILGenerator il, MemberInfo member)
        {
            Contract.Requires(il != null);
            Contract.Requires(member != null);
            PropertyInfo property = member as PropertyInfo;
            if (property != null)
            {
                if (!property.CanWrite)
                {
                    throw new SerializationException(string.Format(CultureInfo.CurrentCulture, "Cannot set value to '{0}.{1}' property.", new object[] { property.DeclaringType, property.Name }));
                }
                il.EmitSetProperty(property);
            }
            else
            {
                Contract.Assert(member is FieldInfo, member.ToString() + ":" + member.MemberType);
                FieldInfo field = member as FieldInfo;
                if (field.IsInitOnly)
                {
                    throw new SerializationException(string.Format(CultureInfo.CurrentCulture, "Cannot set value to '{0}.{1}' field.", new object[] { field.DeclaringType, field.Name }));
                }
                il.EmitStfld(field);
            }
        }

        public static void EmitUnpackerBeginReadSubtree(TracingILGenerator il, int unpackerArgumentIndex, LocalBuilder subtreeUnpacker)
        {
            Contract.Requires(il != null);
            Contract.Requires(unpackerArgumentIndex >= 0);
            Contract.Requires(subtreeUnpacker != null);
            il.EmitAnyLdarg(unpackerArgumentIndex);
            il.EmitAnyCall(_Unpacker.ReadSubtree);
            il.EmitAnyStloc(subtreeUnpacker);
            il.BeginExceptionBlock();
        }

        public static void EmitUnpackerEndReadSubtree(TracingILGenerator il, LocalBuilder subtreeUnpacker)
        {
            Contract.Requires(il != null);
            Contract.Requires(subtreeUnpacker != null);
            il.BeginFinallyBlock();
            il.EmitAnyLdloc(subtreeUnpacker);
            Label target = il.DefineLabel("END_IF");
            il.EmitBrfalse_S(target);
            il.EmitAnyLdloc(subtreeUnpacker);
            il.EmitAnyCall(_IDisposable.Dispose);
            il.MarkLabel(target);
            il.EndExceptionBlock();
        }

        public static void EmitUnpackFrom(SerializerEmitter emitter, TracingILGenerator il, LocalBuilder result, int unpackerIndex)
        {
            emitter.RegisterSerializer(result.LocalType)(il, 0);
            il.EmitAnyLdarg(unpackerIndex);
            il.EmitAnyCall(_UnpackHelpers.InvokeUnpackFrom_1Method.MakeGenericMethod(new Type[] { result.LocalType }));
            il.EmitAnyStloc(result);
        }

        public static void EmitUnpackFrom(SerializerEmitter emitter, TracingILGenerator il, LocalBuilder result, LocalBuilder unpacker)
        {
            emitter.RegisterSerializer(result.LocalType)(il, 0);
            il.EmitAnyLdloc(unpacker);
            il.EmitAnyCall(_UnpackHelpers.InvokeUnpackFrom_1Method.MakeGenericMethod(new Type[] { result.LocalType }));
            il.EmitAnyStloc(result);
        }
    }
}

