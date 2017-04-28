namespace MsgPack
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    internal static class CollectionOperation
    {
        public static void CopyTo<T>(IEnumerable<T> source, int sourceCount, Array array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            if ((array.Rank != 1) || (array.GetLowerBound(0) != 0))
            {
                throw new ArgumentException("array is not zero-based one dimensional array.", "array");
            }
            if (arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException("arrayIndex");
            }
            if ((array.Length - arrayIndex) < sourceCount)
            {
                throw new ArgumentException("array is too small to finish copying.", "array");
            }
            T[] localArray = array as T[];
            if (localArray != null)
            {
                CopyTo<T>(source, sourceCount, 0, localArray, arrayIndex, localArray.Length);
            }
            else
            {
                int num = 0;
                foreach (T local in source)
                {
                    try
                    {
                        array.SetValue(local, (int) (num + arrayIndex));
                        num++;
                    }
                    catch (InvalidCastException)
                    {
                        throw new ArgumentException("The type of destination array is not compatible to the type of items in the collection.", "array");
                    }
                }
            }
        }

        public static void CopyTo<T>(IEnumerable<T> source, int sourceCount, int index, T[] array, int arrayIndex, int count)
        {
            ValidateCopyToArguments<T>(sourceCount, index, array, arrayIndex, count);
            int num = 0;
            foreach (T local in source.Skip<T>(index).Take<T>(count))
            {
                array[num + arrayIndex] = local;
                num++;
            }
        }

        public static void CopyTo<TSource, TDestination>(IEnumerable<TSource> source, int sourceCount, int index, TDestination[] array, int arrayIndex, int count, Func<TSource, TDestination> converter)
        {
            ValidateCopyToArguments<TDestination>(sourceCount, index, array, arrayIndex, count);
            Contract.Assert(converter != null);
            int num = 0;
            foreach (TSource local in source.Skip<TSource>(index).Take<TSource>(count))
            {
                array[num + arrayIndex] = converter(local);
                num++;
            }
        }

        private static void ValidateCopyToArguments<T>(int sourceCount, int index, T[] array, int arrayIndex, int count)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            if ((0 < sourceCount) && (sourceCount <= index))
            {
                throw new ArgumentException("index is too large to finish copying.", "index");
            }
            if (arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException("arrayIndex");
            }
            if ((array.Length - arrayIndex) < count)
            {
                throw new ArgumentException("array is too small to finish copying.", "array");
            }
        }
    }
}

