namespace MsgPack.Serialization
{
    using MsgPack;
    using System;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Reflection;
    using System.Runtime.Serialization;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class SerializationExceptions
    {
        internal static readonly MethodInfo NewFailedToDeserializeMemberMethod = FromExpression.ToMethod<Type, string, Exception, Exception>((targetType, memberName, inner) => NewFailedToDeserializeMember(targetType, memberName, inner));
        internal static readonly MethodInfo NewIsIncorrectStreamMethod = FromExpression.ToMethod<Exception, Exception>(innerException => NewIsIncorrectStream(innerException));
        internal static readonly MethodInfo NewIsNotArrayHeaderMethod = FromExpression.ToMethod<Exception>(Expression.Lambda<Func<Exception>>(Expression.Call(null, (MethodInfo) methodof(SerializationExceptions.NewIsNotArrayHeader), new Expression[0]), new ParameterExpression[0]));
        internal static readonly MethodInfo NewIsNotMapHeaderMethod = FromExpression.ToMethod<Exception>(Expression.Lambda<Func<Exception>>(Expression.Call(null, (MethodInfo) methodof(SerializationExceptions.NewIsNotMapHeader), new Expression[0]), new ParameterExpression[0]));
        internal static readonly MethodInfo NewIsTooLargeCollectionMethod = FromExpression.ToMethod<Exception>(Expression.Lambda<Func<Exception>>(Expression.Call(null, (MethodInfo) methodof(SerializationExceptions.NewIsTooLargeCollection), new Expression[0]), new ParameterExpression[0]));
        internal static readonly MethodInfo NewMissingItemMethod = FromExpression.ToMethod<int, Exception>(index => NewMissingItem(index));
        internal static readonly MethodInfo NewMissingPropertyMethod = FromExpression.ToMethod<string, Exception>(name => NewMissingProperty(name));
        internal static readonly MethodInfo NewNotSupportedBecauseCannotInstanciateAbstractTypeMethod = FromExpression.ToMethod<Type, Exception>(type => NewNotSupportedBecauseCannotInstanciateAbstractType(type));
        internal static readonly MethodInfo NewNullIsProhibitedMethod = FromExpression.ToMethod<string, Exception>(memberName => NewNullIsProhibited(memberName));
        internal static readonly MethodInfo NewReadOnlyMemberItemsMustNotBeNullMethod = FromExpression.ToMethod<string, Exception>(memberName => NewReadOnlyMemberItemsMustNotBeNull(memberName));
        internal static readonly MethodInfo NewStreamDoesNotContainCollectionForMemberMethod = FromExpression.ToMethod<string, Exception>(memberName => NewStreamDoesNotContainCollectionForMember(memberName));
        internal static readonly MethodInfo NewTupleCardinarityIsNotMatchMethod = FromExpression.ToMethod<int, int, Exception>((expected, actual) => NewTupleCardinarityIsNotMatch(expected, actual));
        internal static readonly MethodInfo NewTypeCannotDeserialize3Method = FromExpression.ToMethod<Type, string, Exception, Exception>((type, memberName, inner) => NewTypeCannotDeserialize(type, memberName, inner));
        internal static readonly MethodInfo NewTypeCannotDeserializeMethod = FromExpression.ToMethod<Type, Exception>(type => NewTypeCannotDeserialize(type));
        internal static readonly MethodInfo NewTypeCannotSerializeMethod = FromExpression.ToMethod<Type, Exception>(type => NewTypeCannotSerialize(type));
        internal static readonly MethodInfo NewUnexpectedArrayLengthMethod = FromExpression.ToMethod<int, int, Exception>((expectedLength, actualLength) => NewUnexpectedArrayLength(expectedLength, actualLength));
        internal static readonly MethodInfo NewUnexpectedEndOfStreamMethod = FromExpression.ToMethod<Exception>(Expression.Lambda<Func<Exception>>(Expression.Call(null, (MethodInfo) methodof(SerializationExceptions.NewUnexpectedEndOfStream), new Expression[0]), new ParameterExpression[0]));
        internal static readonly MethodInfo NewValueTypeCannotBeNull3Method = FromExpression.ToMethod<string, Type, Type, Exception>((name, memberType, declaringType) => NewValueTypeCannotBeNull(name, memberType, declaringType));

        internal static Exception NewEmptyOrUnstartedUnpacker()
        {
            return new SerializationException("The unpacker did not read any data yet. The unpacker might never read or underlying stream is empty.");
        }

        public static Exception NewFailedToDeserializeMember(Type targetType, string memberName, Exception inner)
        {
            Contract.Requires(targetType != null);
            Contract.Requires(!string.IsNullOrEmpty(memberName));
            Contract.Requires(inner != null);
            return new SerializationException(string.Format(CultureInfo.CurrentCulture, "Cannot deserialize member '{0}' of type '{1}'.", new object[] { memberName, targetType }), inner);
        }

        public static Exception NewIsIncorrectStream(Exception innerException)
        {
            return new SerializationException("Failed to unpack items count of the collection.", innerException);
        }

        public static Exception NewIsNotArrayHeader()
        {
            return new SerializationException("Unpacker is not in the array header. The stream may not be array.");
        }

        public static Exception NewIsNotMapHeader()
        {
            return new SerializationException("Unpacker is not in the map header. The stream may not be map.");
        }

        public static Exception NewIsTooLargeCollection()
        {
            return new MessageNotSupportedException("The collection which has more than Int32.MaxValue items is not supported.");
        }

        internal static Exception NewMissingAddMethod(Type type)
        {
            Contract.Requires(type != null);
            return new SerializationException(string.Format(CultureInfo.CurrentCulture, "Type '{0}' does not have appropriate Add method.", new object[] { type }));
        }

        public static Exception NewMissingItem(int index)
        {
            Contract.Requires(index >= 0);
            return new InvalidMessagePackStreamException(string.Format(CultureInfo.CurrentCulture, "Items at index '{0}' is missing.", new object[] { index }));
        }

        public static Exception NewMissingProperty(string name)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));
            return new SerializationException(string.Format(CultureInfo.CurrentCulture, "Property '{0}' is missing.", new object[] { name }));
        }

        internal static Exception NewNoSerializableFieldsException(Type type)
        {
            Contract.Requires(type != null);
            return new SerializationException(string.Format(CultureInfo.CurrentCulture, "Cannot serialize type '{0}' because it does not have any serializable fields nor properties.", new object[] { type }));
        }

        public static Exception NewNotSupportedBecauseCannotInstanciateAbstractType(Type type)
        {
            Contract.Requires(type != null);
            return new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "This operation is not supported because '{0}' cannot be instanciated.", new object[] { type }));
        }

        public static Exception NewNullIsProhibited(string memberName)
        {
            Contract.Requires(!string.IsNullOrEmpty(memberName));
            return new SerializationException(string.Format(CultureInfo.CurrentCulture, "The member '{0}' cannot be nil.", new object[] { memberName }));
        }

        public static Exception NewReadOnlyMemberItemsMustNotBeNull(string memberName)
        {
            Contract.Requires(!string.IsNullOrEmpty(memberName));
            return new SerializationException(string.Format(CultureInfo.CurrentCulture, "The member '{0}' cannot be nil because it is read only member.", new object[] { memberName }));
        }

        public static Exception NewStreamDoesNotContainCollectionForMember(string memberName)
        {
            Contract.Requires(!string.IsNullOrEmpty(memberName));
            return new SerializationException(string.Format(CultureInfo.CurrentCulture, "Cannot deserialize member '{0}' because the underlying stream does not contain collection.", new object[] { memberName }));
        }

        internal static Exception NewTargetDoesNotHavePublicDefaultConstructor(Type type)
        {
            Contract.Requires(type != null);
            return new SerializationException(string.Format(CultureInfo.CurrentCulture, "Type '{0}' does not have default (parameterless) public constructor.", new object[] { type }));
        }

        internal static Exception NewTargetDoesNotHavePublicDefaultConstructorNorInitialCapacity(Type type)
        {
            Contract.Requires(type != null);
            return new SerializationException(string.Format(CultureInfo.CurrentCulture, "Type '{0}' does not have both of default (parameterless) public constructor and  public constructor with an Int32 parameter.", new object[] { type }));
        }

        public static Exception NewTupleCardinarityIsNotMatch(int expectedTupleCardinality, int actualArrayLength)
        {
            Contract.Requires(expectedTupleCardinality > 0);
            return new SerializationException(string.Format(CultureInfo.CurrentCulture, "The length of array ({0}) does not match to tuple cardinality ({1}).", new object[] { actualArrayLength, expectedTupleCardinality }));
        }

        public static Exception NewTypeCannotDeserialize(Type type)
        {
            Contract.Requires(type != null);
            return new SerializationException(string.Format(CultureInfo.CurrentCulture, "Cannot deserialize '{0}' type.", new object[] { type }));
        }

        public static Exception NewTypeCannotDeserialize(Type type, string memberName, Exception inner)
        {
            Contract.Requires(type != null);
            Contract.Requires(!string.IsNullOrEmpty(memberName));
            Contract.Requires(inner != null);
            return new SerializationException(string.Format(CultureInfo.CurrentCulture, "Cannot deserialize member '{1}' of type '{0}'.", new object[] { type, memberName }), inner);
        }

        public static Exception NewTypeCannotSerialize(Type type)
        {
            Contract.Requires(type != null);
            return new SerializationException(string.Format(CultureInfo.CurrentCulture, "Cannot serialize '{0}' type.", new object[] { type }));
        }

        public static Exception NewUnexpectedArrayLength(int expectedLength, int actualLength)
        {
            Contract.Requires(expectedLength >= 0);
            Contract.Requires(actualLength >= 0);
            return new SerializationException(string.Format(CultureInfo.CurrentCulture, "The MessagePack stream is invalid. Expected array length is {0}, but actual is {1}.", new object[] { expectedLength, actualLength }));
        }

        public static Exception NewUnexpectedEndOfStream()
        {
            return new SerializationException("Stream unexpectedly ends.");
        }

        public static Exception NewValueTypeCannotBeNull(Type type)
        {
            Contract.Requires(type != null);
            return new SerializationException(string.Format(CultureInfo.CurrentCulture, "Cannot be null '{0}' type value.", new object[] { type }));
        }

        public static Exception NewValueTypeCannotBeNull(string name, Type memberType, Type declaringType)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));
            Contract.Requires(memberType != null);
            Contract.Requires(declaringType != null);
            return new SerializationException(string.Format(CultureInfo.CurrentCulture, "Member '{0}' of type '{1}' cannot be null because it is value type('{2}').", new object[] { name, declaringType, memberType }));
        }
    }
}

