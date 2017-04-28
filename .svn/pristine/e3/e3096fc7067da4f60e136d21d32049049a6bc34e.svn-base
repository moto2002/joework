namespace MsgPack.Serialization.Metadata
{
    using System;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    internal static class _UnpackHelpers
    {
        public static readonly MethodInfo ConvertWithEnsuringNotNull_1Method = typeof(UnpackHelpers).GetMethod("ConvertWithEnsuringNotNull");
        public static readonly MethodInfo InvokeUnpackFrom_1Method = typeof(UnpackHelpers).GetMethod("InvokeUnpackFrom");
        public static readonly MethodInfo UnpackArrayTo_1 = typeof(UnpackHelpers).GetMethod("UnpackArrayTo");
        public static readonly MethodInfo UnpackCollectionTo_1 = typeof(UnpackHelpers).GetMethods().Single<MethodInfo>(item => (((item.Name == "UnpackCollectionTo") && (item.GetParameters().Length == 4)) && (item.GetGenericArguments().Length == 1)));
        public static readonly MethodInfo UnpackCollectionTo_2 = typeof(UnpackHelpers).GetMethods().Single<MethodInfo>(item => (((item.Name == "UnpackCollectionTo") && (item.GetParameters().Length == 4)) && (item.GetGenericArguments().Length == 2)));
        public static readonly MethodInfo UnpackMapTo_2 = typeof(UnpackHelpers).GetMethods().Single<MethodInfo>(item => ((item.Name == "UnpackMapTo") && (item.GetGenericArguments().Length == 2)));
        public static readonly MethodInfo UnpackNonGenericCollectionTo = FromExpression.ToMethod<Unpacker, IList, Action<object>>((unpacker, collection, addition) => UnpackHelpers.UnpackCollectionTo(unpacker, collection, addition));
        public static readonly MethodInfo UnpackNonGenericCollectionTo_1 = typeof(UnpackHelpers).GetMethods().Single<MethodInfo>(item => (((item.Name == "UnpackCollectionTo") && (item.GetParameters().Length == 3)) && (item.GetGenericArguments().Length == 1)));
        public static readonly MethodInfo UnpackNonGenericMapTo = FromExpression.ToMethod<Unpacker, IDictionary>((unpacker, dictionary) => UnpackHelpers.UnpackMapTo(unpacker, dictionary));

        [CompilerGenerated]
        private static bool <.cctor>b__0(MethodInfo item)
        {
            return (((item.Name == "UnpackCollectionTo") && (item.GetParameters().Length == 4)) && (item.GetGenericArguments().Length == 1));
        }

        [CompilerGenerated]
        private static bool <.cctor>b__1(MethodInfo item)
        {
            return (((item.Name == "UnpackCollectionTo") && (item.GetParameters().Length == 4)) && (item.GetGenericArguments().Length == 2));
        }

        [CompilerGenerated]
        private static bool <.cctor>b__2(MethodInfo item)
        {
            return ((item.Name == "UnpackMapTo") && (item.GetGenericArguments().Length == 2));
        }

        [CompilerGenerated]
        private static bool <.cctor>b__3(MethodInfo item)
        {
            return (((item.Name == "UnpackCollectionTo") && (item.GetParameters().Length == 3)) && (item.GetGenericArguments().Length == 1));
        }
    }
}

