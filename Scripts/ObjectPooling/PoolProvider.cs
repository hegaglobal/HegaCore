using System;
using System.Collections.Generic;

namespace HegaCore
{
    using SCG = System.Collections.Generic;

    public static class PoolProvider
    {
        public static Pool<T> Pool<T>() where T : class, new()
            => SCG.Pool<T>.Default;

        public static T[] Array1<T>(int size)
            => Array1Pool<T>.Get(size);

        public static void Return<T>(T[] item)
            => Array1Pool<T>.Return(item);

        public static void Return<T>(params T[][] items)
            => Array1Pool<T>.Return(items);

        public static void Return<T>(IEnumerable<T[]> items)
            => Array1Pool<T>.Return(items);

        public static List<T> List<T>()
            => ListPool<T>.Get();

        public static void Return<T>(List<T> item)
            => ListPool<T>.Return(item);

        public static void Return<T>(params List<T>[] items)
            => ListPool<T>.Return(items);

        public static void Return<T>(IEnumerable<List<T>> items)
            => ListPool<T>.Return(items);

        public static HashSet<T> HashSet<T>()
            => HashSetPool<T>.Get();

        public static void Return<T>(HashSet<T> item)
            => HashSetPool<T>.Return(item);

        public static void Return<T>(params HashSet<T>[] items)
            => HashSetPool<T>.Return(items);

        public static void Return<T>(IEnumerable<HashSet<T>> items)
            => HashSetPool<T>.Return(items);

        public static Queue<T> Queue<T>()
            => QueuePool<T>.Get();

        public static void Return<T>(Queue<T> item)
            => QueuePool<T>.Return(item);

        public static void Return<T>(params Queue<T>[] items)
            => QueuePool<T>.Return(items);

        public static void Return<T>(IEnumerable<Queue<T>> items)
            => QueuePool<T>.Return(items);

        public static Stack<T> Stack<T>()
            => StackPool<T>.Get();

        public static void Return<T>(Stack<T> item)
            => StackPool<T>.Return(item);

        public static void Return<T>(params Stack<T>[] items)
            => StackPool<T>.Return(items);

        public static void Return<T>(IEnumerable<Stack<T>> items)
            => StackPool<T>.Return(items);

        public static Dictionary<TKey, TValue> Dictionary<TKey, TValue>()
            => DictionaryPool<TKey, TValue>.Get();

        public static void Return<TKey, TValue>(Dictionary<TKey, TValue> item)
            => DictionaryPool<TKey, TValue>.Return(item);

        public static void Return<TKey, TValue>(params Dictionary<TKey, TValue>[] items)
            => DictionaryPool<TKey, TValue>.Return(items);

        public static void Return<TKey, TValue>(IEnumerable<Dictionary<TKey, TValue>> items)
            => DictionaryPool<TKey, TValue>.Return(items);
    }
}