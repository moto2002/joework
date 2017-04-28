namespace MsgPack
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Runtime.InteropServices;

    internal static class SetOperation
    {
        public static bool IsProperSubsetOf<T>(ICollection<T> set, IEnumerable<T> other)
        {
            int num;
            Contract.Assert(set != null);
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }
            ICollection<T> is2 = other as ICollection<T>;
            if (is2 != null)
            {
                if (set.Count == 0)
                {
                    return (0 < is2.Count);
                }
                if (is2.Count <= set.Count)
                {
                    return false;
                }
            }
            if (!IsSubsetOfCore<T>(set, other, out num))
            {
                return false;
            }
            return (set.Count < num);
        }

        public static bool IsProperSupersetOf<T>(ICollection<T> set, IEnumerable<T> other)
        {
            int num;
            Contract.Assert(set != null);
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }
            ICollection<T> is2 = other as ICollection<T>;
            if ((is2 != null) && (is2.Count == 0))
            {
                return (0 < set.Count);
            }
            if (!IsSupersetOfCore<T>(set, other, out num))
            {
                return false;
            }
            return (num < set.Count);
        }

        public static bool IsSubsetOf<T>(ICollection<T> set, IEnumerable<T> other)
        {
            int num;
            Contract.Assert(set != null);
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }
            if (set.Count == 0)
            {
                return true;
            }
            ICollection<T> is2 = other as ICollection<T>;
            if ((is2 != null) && (is2.Count < set.Count))
            {
                return false;
            }
            return IsSubsetOfCore<T>(set, other, out num);
        }

        private static bool IsSubsetOfCore<T>(ICollection<T> set, IEnumerable<T> other, out int otherCount)
        {
            otherCount = 0;
            HashSet<T> set2 = other as HashSet<T>;
            if (set2 == null)
            {
                set2 = new HashSet<T>(other);
            }
            int num = 0;
            foreach (T local in set2)
            {
                otherCount++;
                if (set.Contains(local))
                {
                    num++;
                }
            }
            return (set.Count <= num);
        }

        public static bool IsSupersetOf<T>(ICollection<T> set, IEnumerable<T> other)
        {
            int num;
            Contract.Assert(set != null);
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }
            ICollection<T> is2 = other as ICollection<T>;
            if ((is2 != null) && (is2.Count < set.Count))
            {
                if (is2.Count == 0)
                {
                    return true;
                }
                if (set.Count <= is2.Count)
                {
                    return false;
                }
            }
            return IsSupersetOfCore<T>(set, other, out num);
        }

        private static bool IsSupersetOfCore<T>(ICollection<T> set, IEnumerable<T> other, out int otherCount)
        {
            otherCount = 0;
            HashSet<T> set2 = other as HashSet<T>;
            if (set2 == null)
            {
                set2 = new HashSet<T>(other);
            }
            foreach (T local in set2)
            {
                otherCount++;
                if (!set.Contains(local))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool Overlaps<T>(ICollection<T> set, IEnumerable<T> other)
        {
            Contract.Assert(set != null);
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }
            if (set.Count == 0)
            {
                return false;
            }
            return other.Any<T>(item => set.Contains(item));
        }

        public static bool SetEquals<T>(ICollection<T> set, IEnumerable<T> other)
        {
            Contract.Assert(set != null);
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }
            if (set.Count == 0)
            {
                ICollection<T> is2 = other as ICollection<T>;
                if (is2 != null)
                {
                    return (is2.Count == 0);
                }
            }
            HashSet<T> set2 = (other as HashSet<T>) ?? new HashSet<T>(other);
            int num = 0;
            foreach (T local in set2)
            {
                if (!set.Contains(local))
                {
                    return false;
                }
                num++;
            }
            return (num == set.Count);
        }
    }
}

