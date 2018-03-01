using System;
using System.Collections.Generic;
using System.Linq;

namespace Axe.Cli.Parser.Extensions
{
    static class EnumerableExtensions
    {
        public static Dictionary<TKey, TValue> MergeToDictionary<T, TKey, TValue>(
            this IEnumerable<T> enumerable,
            Func<T, TKey> keySelector,
            Func<T, TValue> valueSelector)
        {
            return enumerable.Aggregate(
                new Dictionary<TKey, TValue>(),
                (acc, item) =>
                {
                    TKey key = keySelector(item);
                    if (!acc.ContainsKey(key))
                    {
                        acc.Add(key, valueSelector(item));
                    }

                    return acc;
                });
        }

        public static bool HasDuplication<T>(this IEnumerable<T> source)
        {
            return HasDuplication(source, EqualityComparer<T>.Default);
        }

        public static bool HasDuplication<T>(this IEnumerable<T> source, IEqualityComparer<T> comparer)
        {
            var set = new HashSet<T>(comparer);
            foreach (T o in source)
            {
                if (!set.Add(o)) { return true; }
            }

            return false;
        }
    }
}