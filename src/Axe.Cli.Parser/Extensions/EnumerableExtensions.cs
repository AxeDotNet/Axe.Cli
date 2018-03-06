using System.Collections.Generic;

namespace Axe.Cli.Parser.Extensions
{
    static class EnumerableExtensions
    {
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