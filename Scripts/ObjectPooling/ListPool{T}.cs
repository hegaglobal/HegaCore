using System.Collections.Concurrent;
using System.Collections.Generic;

namespace HegaCore
{
    public static class ListPool<T>
    {
        private static readonly ConcurrentQueue<List<T>> _pool = new ConcurrentQueue<List<T>>();

        public static List<T> Get()
        {
            if (_pool.TryDequeue(out var item))
                return item;

            return new List<T>();
        }

        public static void Return(List<T> item)
        {
            item?.Clear();
            _pool.Enqueue(item);
        }

        public static void Return(params List<T>[] items)
        {
            if (items == null)
                return;

            foreach (var item in items)
            {
                item?.Clear();
                _pool.Enqueue(item);
            }
        }

        public static void Return(IEnumerable<List<T>> items)
        {
            if (items == null)
                return;

            foreach (var item in items)
            {
                item?.Clear();
                _pool.Enqueue(item);
            }
        }
    }
}