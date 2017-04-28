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
    using System.Reflection;
    using System.Reflection.Emit;

    internal static class EmittingSerializerBuilderLogics
    {
        private static readonly Type[] _delegateConstructorParameterTypes = new Type[] { typeof(object), typeof(IntPtr) };

        public static SerializerEmitter CreateArraySerializerCore(Type targetType, EmitterFlavor emitterFlavor)
        {
            Contract.Requires(targetType != null);
            SerializerEmitter emitter = SerializationMethodGeneratorManager.Get().CreateEmitter(targetType, emitterFlavor);
            CollectionTraits collectionTraits = targetType.GetCollectionTraits();
            CreatePackArrayProceduresCore(targetType, emitter, collectionTraits);
            CreateUnpackArrayProceduresCore(targetType, emitter, collectionTraits);
            return emitter;
        }

        private static void CreateArrayUnpackFrom(Type targetType, SerializerEmitter emitter, CollectionTraits traits)
        {
            Action<TracingILGenerator> initialCountLoadingEmitter = null;
            TracingILGenerator unpackFromMethodILGenerator = emitter.GetUnpackFromMethodILGenerator();
            LocalVariableHolder localHolder = new LocalVariableHolder(unpackFromMethodILGenerator);
            try
            {
                LocalBuilder collection;
                if (targetType.IsInterface || targetType.IsAbstract)
                {
                    unpackFromMethodILGenerator.EmitTypeOf(targetType);
                    unpackFromMethodILGenerator.EmitAnyCall(SerializationExceptions.NewNotSupportedBecauseCannotInstanciateAbstractTypeMethod);
                    unpackFromMethodILGenerator.EmitThrow();
                }
                else
                {
                    unpackFromMethodILGenerator.EmitAnyLdarg(1);
                    unpackFromMethodILGenerator.EmitGetProperty(_Unpacker.IsArrayHeader);
                    Label target = unpackFromMethodILGenerator.DefineLabel("END_IF");
                    unpackFromMethodILGenerator.EmitBrtrue_S(target);
                    unpackFromMethodILGenerator.EmitAnyCall(SerializationExceptions.NewIsNotArrayHeaderMethod);
                    unpackFromMethodILGenerator.EmitThrow();
                    unpackFromMethodILGenerator.MarkLabel(target);
                    collection = localHolder.GetDeserializingCollection(targetType);
                    if (initialCountLoadingEmitter == null)
                    {
                        initialCountLoadingEmitter = il0 => Emittion.EmitGetUnpackerItemsCountAsInt32(il0, 1, localHolder);
                    }
                    Emittion.EmitConstruction(unpackFromMethodILGenerator, collection, initialCountLoadingEmitter);
                    EmitInvokeArrayUnpackToHelper(targetType, emitter, traits, unpackFromMethodILGenerator, 1, il0 => il0.EmitAnyLdloc(collection));
                    unpackFromMethodILGenerator.EmitAnyLdloc(collection);
                    unpackFromMethodILGenerator.EmitRet();
                }
            }
            finally
            {
                unpackFromMethodILGenerator.FlushTrace();
            }
        }

        private static void CreateArrayUnpackTo(Type targetType, SerializerEmitter emitter, CollectionTraits traits)
        {
            TracingILGenerator unpackToMethodILGenerator = emitter.GetUnpackToMethodILGenerator();
            try
            {
                EmitInvokeArrayUnpackToHelper(targetType, emitter, traits, unpackToMethodILGenerator, 1, il0 => il0.EmitAnyLdarg(2));
                unpackToMethodILGenerator.EmitRet();
            }
            finally
            {
                unpackToMethodILGenerator.FlushTrace();
            }
        }

        private static void CreateMapPack(Type targetType, SerializerEmitter emiter, CollectionTraits traits)
        {
            TracingILGenerator packToMethodILGenerator = emiter.GetPackToMethodILGenerator();
            LocalVariableHolder localHolder = new LocalVariableHolder(packToMethodILGenerator);
            try
            {
                <>c__DisplayClass20 class3;
                LocalBuilder serializingCollection = localHolder.GetSerializingCollection(targetType);
                LocalBuilder item = localHolder.GetSerializingCollectionItem(traits.ElementType);
                PropertyInfo keyProperty = traits.ElementType.GetProperty("Key");
                PropertyInfo valueProperty = traits.ElementType.GetProperty("Value");
                packToMethodILGenerator.EmitAnyLdarg(2);
                packToMethodILGenerator.EmitAnyStloc(serializingCollection);
                LocalBuilder packingCollectionCount = localHolder.PackingCollectionCount;
                EmitLoadTarget(targetType, packToMethodILGenerator, serializingCollection);
                packToMethodILGenerator.EmitGetProperty(traits.CountProperty);
                packToMethodILGenerator.EmitAnyStloc(packingCollectionCount);
                packToMethodILGenerator.EmitAnyLdarg(1);
                packToMethodILGenerator.EmitAnyLdloc(packingCollectionCount);
                packToMethodILGenerator.EmitAnyCall(_Packer.PackMapHeader);
                packToMethodILGenerator.EmitPop();
                Emittion.EmitForEach(packToMethodILGenerator, traits, serializingCollection, delegate (TracingILGenerator il0, Action getCurrentEmitter) {
                    Action<TracingILGenerator> loadValueEmitter = null;
                    Action<TracingILGenerator> action2 = null;
                    Action<TracingILGenerator> action3 = null;
                    Action<TracingILGenerator> action4 = null;
                    <>c__DisplayClass20 class1 = class3;
                    if (traits.ElementType.IsGenericType)
                    {
                        Contract.Assert(traits.ElementType.GetGenericTypeDefinition() == typeof(KeyValuePair<,>));
                        getCurrentEmitter();
                        il0.EmitAnyStloc(item);
                        if (loadValueEmitter == null)
                        {
                            loadValueEmitter = delegate (TracingILGenerator il1) {
                                il1.EmitAnyLdloca(item);
                                il1.EmitGetProperty(keyProperty);
                            };
                        }
                        Emittion.EmitSerializeValue(emiter, il0, 1, traits.ElementType.GetGenericArguments()[0], null, NilImplication.MemberDefault, loadValueEmitter, localHolder);
                        if (action2 == null)
                        {
                            action2 = delegate (TracingILGenerator il1) {
                                il1.EmitAnyLdloca(item);
                                il1.EmitGetProperty(valueProperty);
                            };
                        }
                        Emittion.EmitSerializeValue(emiter, il0, 1, traits.ElementType.GetGenericArguments()[1], null, NilImplication.MemberDefault, action2, localHolder);
                    }
                    else
                    {
                        Contract.Assert(traits.ElementType == typeof(DictionaryEntry));
                        getCurrentEmitter();
                        il0.EmitAnyStloc(item);
                        if (action3 == null)
                        {
                            action3 = delegate (TracingILGenerator il1) {
                                il0.EmitAnyLdloca(item);
                                il0.EmitGetProperty(_DictionaryEntry.Key);
                                il0.EmitUnbox_Any(typeof(MessagePackObject));
                            };
                        }
                        Emittion.EmitSerializeValue(emiter, il0, 1, typeof(MessagePackObject), null, NilImplication.MemberDefault, action3, localHolder);
                        if (action4 == null)
                        {
                            action4 = delegate (TracingILGenerator il1) {
                                il0.EmitAnyLdloca(item);
                                il0.EmitGetProperty(_DictionaryEntry.Value);
                                il0.EmitUnbox_Any(typeof(MessagePackObject));
                            };
                        }
                        Emittion.EmitSerializeValue(emiter, il0, 1, typeof(MessagePackObject), null, NilImplication.MemberDefault, action4, localHolder);
                    }
                });
                packToMethodILGenerator.EmitRet();
            }
            finally
            {
                packToMethodILGenerator.FlushTrace();
            }
        }

        public static SerializerEmitter CreateMapSerializerCore(Type targetType, EmitterFlavor emitterFlavor)
        {
            Contract.Requires(targetType != null);
            SerializerEmitter emiter = SerializationMethodGeneratorManager.Get().CreateEmitter(targetType, emitterFlavor);
            CollectionTraits collectionTraits = targetType.GetCollectionTraits();
            CreateMapPack(targetType, emiter, collectionTraits);
            CreateMapUnpack(targetType, emiter, collectionTraits);
            return emiter;
        }

        private static void CreateMapUnpack(Type targetType, SerializerEmitter emitter, CollectionTraits traits)
        {
            CreateMapUnpackFrom(targetType, emitter, traits);
            CreateMapUnpackTo(targetType, emitter, traits);
        }

        private static void CreateMapUnpackFrom(Type targetType, SerializerEmitter emitter, CollectionTraits traits)
        {
            Action<TracingILGenerator> initialCountLoadingEmitter = null;
            TracingILGenerator unpackFromMethodILGenerator = emitter.GetUnpackFromMethodILGenerator();
            LocalVariableHolder localHolder = new LocalVariableHolder(unpackFromMethodILGenerator);
            try
            {
                LocalBuilder collection;
                if (targetType.IsInterface || targetType.IsAbstract)
                {
                    unpackFromMethodILGenerator.EmitTypeOf(targetType);
                    unpackFromMethodILGenerator.EmitAnyCall(SerializationExceptions.NewNotSupportedBecauseCannotInstanciateAbstractTypeMethod);
                    unpackFromMethodILGenerator.EmitThrow();
                }
                else
                {
                    unpackFromMethodILGenerator.EmitAnyLdarg(1);
                    unpackFromMethodILGenerator.EmitGetProperty(_Unpacker.IsMapHeader);
                    Label target = unpackFromMethodILGenerator.DefineLabel("END_IF");
                    unpackFromMethodILGenerator.EmitBrtrue_S(target);
                    unpackFromMethodILGenerator.EmitAnyCall(SerializationExceptions.NewIsNotMapHeaderMethod);
                    unpackFromMethodILGenerator.EmitThrow();
                    unpackFromMethodILGenerator.MarkLabel(target);
                    collection = localHolder.GetDeserializingCollection(targetType);
                    if (initialCountLoadingEmitter == null)
                    {
                        initialCountLoadingEmitter = il0 => Emittion.EmitGetUnpackerItemsCountAsInt32(il0, 1, localHolder);
                    }
                    Emittion.EmitConstruction(unpackFromMethodILGenerator, collection, initialCountLoadingEmitter);
                    EmitInvokeMapUnpackToHelper(targetType, emitter, traits, unpackFromMethodILGenerator, 1, il0 => il0.EmitAnyLdloc(collection));
                    unpackFromMethodILGenerator.EmitAnyLdloc(collection);
                    unpackFromMethodILGenerator.EmitRet();
                }
            }
            finally
            {
                unpackFromMethodILGenerator.FlushTrace();
            }
        }

        private static void CreateMapUnpackTo(Type targetType, SerializerEmitter emitter, CollectionTraits traits)
        {
            TracingILGenerator unpackToMethodILGenerator = emitter.GetUnpackToMethodILGenerator();
            try
            {
                EmitInvokeMapUnpackToHelper(targetType, emitter, traits, unpackToMethodILGenerator, 1, il0 => il0.EmitAnyLdarg(2));
                unpackToMethodILGenerator.EmitRet();
            }
            finally
            {
                unpackToMethodILGenerator.FlushTrace();
            }
        }

        private static void CreatePackArrayProceduresCore(Type targetType, SerializerEmitter emitter, CollectionTraits traits)
        {
            Action<TracingILGenerator, LocalBuilder> bodyEmitter = null;
            Action<TracingILGenerator, Action> action2 = null;
            TracingILGenerator packToMethodILGenerator = emitter.GetPackToMethodILGenerator();
            LocalVariableHolder localHolder = new LocalVariableHolder(packToMethodILGenerator);
            try
            {
                LocalBuilder packingCollectionCount;
                if (targetType.IsArray)
                {
                    packingCollectionCount = localHolder.PackingCollectionCount;
                    packToMethodILGenerator.EmitAnyLdarg(2);
                    packToMethodILGenerator.EmitLdlen();
                    packToMethodILGenerator.EmitAnyStloc(packingCollectionCount);
                    packToMethodILGenerator.EmitAnyLdarg(1);
                    packToMethodILGenerator.EmitAnyLdloc(packingCollectionCount);
                    packToMethodILGenerator.EmitAnyCall(_Packer.PackArrayHeader);
                    packToMethodILGenerator.EmitPop();
                    if (bodyEmitter == null)
                    {
                        bodyEmitter = (il0, i) => Emittion.EmitSerializeValue(emitter, il0, 1, traits.ElementType, null, NilImplication.MemberDefault, delegate (TracingILGenerator il1) {
                            il1.EmitAnyLdarg(2);
                            il1.EmitAnyLdloc(i);
                            il1.EmitLdelem(traits.ElementType);
                        }, localHolder);
                    }
                    Emittion.EmitFor(packToMethodILGenerator, packingCollectionCount, bodyEmitter);
                }
                else if (traits.CountProperty == null)
                {
                    <>c__DisplayClass8 class2;
                    LocalBuilder array = localHolder.GetSerializingCollection(traits.ElementType.MakeArrayType());
                    EmitLoadTarget(targetType, packToMethodILGenerator, 2);
                    packToMethodILGenerator.EmitAnyCall(_Enumerable.ToArray1Method.MakeGenericMethod(new Type[] { traits.ElementType }));
                    packToMethodILGenerator.EmitAnyStloc(array);
                    packingCollectionCount = localHolder.PackingCollectionCount;
                    packToMethodILGenerator.EmitAnyLdloc(array);
                    packToMethodILGenerator.EmitLdlen();
                    packToMethodILGenerator.EmitAnyStloc(packingCollectionCount);
                    packToMethodILGenerator.EmitAnyLdarg(1);
                    packToMethodILGenerator.EmitAnyLdloc(packingCollectionCount);
                    packToMethodILGenerator.EmitAnyCall(_Packer.PackArrayHeader);
                    packToMethodILGenerator.EmitPop();
                    Emittion.EmitFor(packToMethodILGenerator, packingCollectionCount, delegate (TracingILGenerator il0, LocalBuilder i) {
                        <>c__DisplayClass8 class1 = class2;
                        Emittion.EmitSerializeValue(emitter, il0, 1, traits.ElementType, null, NilImplication.MemberDefault, delegate (TracingILGenerator il1) {
                            il1.EmitAnyLdloc(array);
                            il1.EmitAnyLdloc(i);
                            il1.EmitLdelem(class1.traits.ElementType);
                        }, localHolder);
                    });
                }
                else
                {
                    LocalBuilder serializingCollection = localHolder.GetSerializingCollection(targetType);
                    packToMethodILGenerator.EmitAnyLdarg(2);
                    packToMethodILGenerator.EmitAnyStloc(serializingCollection);
                    LocalBuilder local = localHolder.PackingCollectionCount;
                    EmitLoadTarget(targetType, packToMethodILGenerator, 2);
                    packToMethodILGenerator.EmitGetProperty(traits.CountProperty);
                    packToMethodILGenerator.EmitAnyStloc(local);
                    packToMethodILGenerator.EmitAnyLdarg(1);
                    packToMethodILGenerator.EmitAnyLdloc(local);
                    packToMethodILGenerator.EmitAnyCall(_Packer.PackArrayHeader);
                    packToMethodILGenerator.EmitPop();
                    if (action2 == null)
                    {
                        action2 = (il0, getCurrentEmitter) => Emittion.EmitSerializeValue(emitter, il0, 1, traits.ElementType, null, NilImplication.MemberDefault, _ => getCurrentEmitter(), localHolder);
                    }
                    Emittion.EmitForEach(packToMethodILGenerator, traits, serializingCollection, action2);
                }
                packToMethodILGenerator.EmitRet();
            }
            finally
            {
                packToMethodILGenerator.FlushTrace();
            }
        }

        private static void CreateUnpackArrayProceduresCore(Type targetType, SerializerEmitter emitter, CollectionTraits traits)
        {
            CreateArrayUnpackFrom(targetType, emitter, traits);
            CreateArrayUnpackTo(targetType, emitter, traits);
        }

        private static void EmitInvokeArrayUnpackToHelper(Type targetType, SerializerEmitter emitter, CollectionTraits traits, TracingILGenerator il, int unpackerArgumentIndex, Action<TracingILGenerator> loadCollectionEmitting)
        {
            il.EmitAnyLdarg(unpackerArgumentIndex);
            Action<TracingILGenerator, int> action = emitter.RegisterSerializer(traits.ElementType);
            if (targetType.IsArray)
            {
                action(il, 0);
                loadCollectionEmitting(il);
                il.EmitAnyCall(_UnpackHelpers.UnpackArrayTo_1.MakeGenericMethod(new Type[] { traits.ElementType }));
            }
            else
            {
                Type returnType;
                if (targetType.IsGenericType)
                {
                    Type parameterType;
                    action(il, 0);
                    loadCollectionEmitting(il);
                    if (targetType.IsValueType)
                    {
                        il.EmitBox(targetType);
                    }
                    if ((traits.AddMethod.ReturnType == null) || (traits.AddMethod.ReturnType == typeof(void)))
                    {
                        parameterType = traits.AddMethod.GetParameters()[0].ParameterType;
                        EmitNewDelegate(il, targetType, traits.AddMethod, loadCollectionEmitting, typeof(Action<>).MakeGenericType(new Type[] { parameterType }));
                        il.EmitAnyCall(_UnpackHelpers.UnpackCollectionTo_1.MakeGenericMethod(new Type[] { parameterType }));
                    }
                    else
                    {
                        parameterType = traits.AddMethod.GetParameters()[0].ParameterType;
                        returnType = traits.AddMethod.ReturnType;
                        EmitNewDelegate(il, targetType, traits.AddMethod, loadCollectionEmitting, typeof(Func<,>).MakeGenericType(new Type[] { parameterType, returnType }));
                        il.EmitAnyCall(_UnpackHelpers.UnpackCollectionTo_2.MakeGenericMethod(new Type[] { parameterType, returnType }));
                    }
                }
                else
                {
                    loadCollectionEmitting(il);
                    if (targetType.IsValueType)
                    {
                        il.EmitBox(targetType);
                    }
                    if ((traits.AddMethod.ReturnType == null) || (traits.AddMethod.ReturnType == typeof(void)))
                    {
                        EmitNewDelegate(il, targetType, traits.AddMethod, loadCollectionEmitting, typeof(Action<object>));
                        il.EmitAnyCall(_UnpackHelpers.UnpackNonGenericCollectionTo);
                    }
                    else
                    {
                        returnType = traits.AddMethod.ReturnType;
                        EmitNewDelegate(il, targetType, traits.AddMethod, loadCollectionEmitting, typeof(Func<,>).MakeGenericType(new Type[] { typeof(object), returnType }));
                        il.EmitAnyCall(_UnpackHelpers.UnpackNonGenericCollectionTo_1.MakeGenericMethod(new Type[] { returnType }));
                    }
                }
            }
        }

        private static void EmitInvokeMapUnpackToHelper(Type targetType, SerializerEmitter emitter, CollectionTraits traits, TracingILGenerator il, int unpackerArgumentIndex, Action<TracingILGenerator> loadCollectionEmitting)
        {
            il.EmitAnyLdarg(unpackerArgumentIndex);
            if (traits.ElementType.IsGenericType)
            {
                Type type = traits.ElementType.GetGenericArguments()[0];
                Type type2 = traits.ElementType.GetGenericArguments()[1];
                Action<TracingILGenerator, int> action = emitter.RegisterSerializer(type);
                Action<TracingILGenerator, int> action2 = emitter.RegisterSerializer(type2);
                action(il, 0);
                action2(il, 0);
                loadCollectionEmitting(il);
                if (targetType.IsValueType)
                {
                    il.EmitBox(targetType);
                }
                il.EmitAnyCall(_UnpackHelpers.UnpackMapTo_2.MakeGenericMethod(new Type[] { type, type2 }));
            }
            else
            {
                loadCollectionEmitting(il);
                if (targetType.IsValueType)
                {
                    il.EmitBox(targetType);
                }
                il.EmitAnyCall(_UnpackHelpers.UnpackNonGenericMapTo);
            }
        }

        private static void EmitLoadTarget(Type targetType, TracingILGenerator il, int parameterIndex)
        {
            if (targetType.IsValueType)
            {
                il.EmitAnyLdarga(parameterIndex);
            }
            else
            {
                il.EmitAnyLdarg(parameterIndex);
            }
        }

        private static void EmitLoadTarget(Type targetType, TracingILGenerator il, LocalBuilder local)
        {
            if (targetType.IsValueType)
            {
                il.EmitAnyLdloca(local);
            }
            else
            {
                il.EmitAnyLdloc(local);
            }
        }

        private static void EmitNewDelegate(TracingILGenerator il, Type targetType, MethodInfo method, Action<TracingILGenerator> loadTargetEmitting, Type delegateType)
        {
            loadTargetEmitting(il);
            if (targetType.IsValueType)
            {
                il.EmitBox(targetType);
            }
            if (!((!method.IsStatic && !method.IsFinal) && method.IsVirtual))
            {
                il.EmitLdftn(method);
            }
            else
            {
                il.EmitDup();
                il.EmitLdvirtftn(method);
            }
            il.EmitNewobj(delegateType.GetConstructor(_delegateConstructorParameterTypes));
        }
    }
}

