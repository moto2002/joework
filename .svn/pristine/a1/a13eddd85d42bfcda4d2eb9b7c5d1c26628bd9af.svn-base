namespace MsgPack.Serialization
{
    using System;
    using System.Globalization;
    using System.Linq.Expressions;
    using System.Reflection;

    internal static class FromExpression
    {
        private static void ThrowNotValidExpressionTypeException(Expression source)
        {
            throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Specified expression '{0}' is too complex. Simple member reference expression is only supported. ", new object[] { source }));
        }

        public static ConstructorInfo ToConstructor<TResult>(Expression<Func<TResult>> source)
        {
            return ToConstructorCore<Func<TResult>>(source);
        }

        public static ConstructorInfo ToConstructor<T, TResult>(Expression<Func<T, TResult>> source)
        {
            return ToConstructorCore<Func<T, TResult>>(source);
        }

        public static ConstructorInfo ToConstructor<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> source)
        {
            return ToConstructorCore<Func<T1, T2, TResult>>(source);
        }

        public static ConstructorInfo ToConstructor<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, TResult>> source)
        {
            return ToConstructorCore<Func<T1, T2, T3, TResult>>(source);
        }

        public static ConstructorInfo ToConstructor<T1, T2, T3, T4, TResult>(Expression<Func<T1, T2, T3, T4, TResult>> source)
        {
            return ToConstructorCore<Func<T1, T2, T3, T4, TResult>>(source);
        }

        private static ConstructorInfo ToConstructorCore<T>(Expression<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            NewExpression body = source.Body as NewExpression;
            if (body == null)
            {
                ThrowNotValidExpressionTypeException(source);
            }
            return body.Constructor;
        }

        public static MethodInfo ToMethod<T>(Expression<Action<T>> source)
        {
            return ToMethodCore<Action<T>>(source);
        }

        public static MethodInfo ToMethod(Expression<Action> source)
        {
            return ToMethodCore<Action>(source);
        }

        public static MethodInfo ToMethod<T1, T2>(Expression<Action<T1, T2>> source)
        {
            return ToMethodCore<Action<T1, T2>>(source);
        }

        public static MethodInfo ToMethod<T1, T2, T3>(Expression<Action<T1, T2, T3>> source)
        {
            return ToMethodCore<Action<T1, T2, T3>>(source);
        }

        public static MethodInfo ToMethod<T1, T2, T3, T4>(Expression<Action<T1, T2, T3, T4>> source)
        {
            return ToMethodCore<Action<T1, T2, T3, T4>>(source);
        }

        public static MethodInfo ToMethod<TResult>(Expression<Func<TResult>> source)
        {
            return ToMethodCore<Func<TResult>>(source);
        }

        public static MethodInfo ToMethod<T, TResult>(Expression<Func<T, TResult>> source)
        {
            return ToMethodCore<Func<T, TResult>>(source);
        }

        public static MethodInfo ToMethod<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> source)
        {
            return ToMethodCore<Func<T1, T2, TResult>>(source);
        }

        public static MethodInfo ToMethod<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, TResult>> source)
        {
            return ToMethodCore<Func<T1, T2, T3, TResult>>(source);
        }

        public static MethodInfo ToMethod<T1, T2, T3, T4, TResult>(Expression<Func<T1, T2, T3, T4, TResult>> source)
        {
            return ToMethodCore<Func<T1, T2, T3, T4, TResult>>(source);
        }

        private static MethodInfo ToMethodCore<T>(Expression<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            MethodCallExpression body = source.Body as MethodCallExpression;
            if (body == null)
            {
                ThrowNotValidExpressionTypeException(source);
            }
            return body.Method;
        }

        public static MethodInfo ToOperator<T, TResult>(Expression<Func<T, TResult>> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            UnaryExpression body = source.Body as UnaryExpression;
            if (body == null)
            {
                ThrowNotValidExpressionTypeException(source);
            }
            return body.Method;
        }

        public static MethodInfo ToOperator<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            BinaryExpression body = source.Body as BinaryExpression;
            if (body == null)
            {
                ThrowNotValidExpressionTypeException(source);
            }
            return body.Method;
        }

        public static PropertyInfo ToProperty<T>(Expression<Func<T>> source)
        {
            return ToPropertyCore<Func<T>>(source);
        }

        public static PropertyInfo ToProperty<TSource, T>(Expression<Func<TSource, T>> source)
        {
            return ToPropertyCore<Func<TSource, T>>(source);
        }

        private static PropertyInfo ToPropertyCore<T>(Expression<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            MemberExpression body = source.Body as MemberExpression;
            if (body == null)
            {
                ThrowNotValidExpressionTypeException(source);
            }
            PropertyInfo member = body.Member as PropertyInfo;
            if (member == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Member '{0}' is not property.", new object[] { body.Member }), "source");
            }
            return member;
        }
    }
}

