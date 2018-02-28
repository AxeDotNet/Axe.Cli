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
    }
}