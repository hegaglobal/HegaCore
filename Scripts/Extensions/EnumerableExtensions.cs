using System;
using System.Collections.Generic;
using System.Collections.Pooling;

namespace HegaCore
{
    public static class EnumerableExtensions
    {
        public static void Exclude<T>(this IEnumerable<T> source, ICollection<T> dest, params T[] excludes)
        {
            var set = Pool.Provider.HashSet<T>();
            set.AddRange(excludes);

            foreach (var type in source)
            {
                if (set.Contains(type))
                    continue;

                dest.Add(type);
            }

            Pool.Provider.Return(set);
        }

        public static IEnumerable<T> Exclude<T>(this IEnumerable<T> source, params T[] excludes)
        {
            var set = Pool.Provider.HashSet<T>();
            set.AddRange(excludes);

            foreach (var type in source)
            {
                if (set.Contains(type))
                    continue;

                yield return type;
            }

            Pool.Provider.Return(set);
        }

        public static void Dispose(this IEnumerable<IDisposable> self)
        {
            foreach (var disposable in self)
            {
                disposable?.Dispose();
            }
        }
    }
}