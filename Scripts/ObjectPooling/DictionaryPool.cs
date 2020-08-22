using System.Collections.Concurrent;
using System.Collections.Generic;

namespace HegaCore
{
    public static class DictionaryPool<TKey, TValue>
    {
        private static readonly ConcurrentQueue<Dictionary<TKey, TValue>> _pool = new ConcurrentQueue<Dictionary<TKey, TValue>>();

        public static Dictionary<TKey, TValue> Get()
        {
            if (_pool.TryDequeue(out var item))
                return item;

            return new Dictionary<TKey, TValue>();
        }

        public static void Return(Dictionary<TKey, TValue> item)
        {
            item?.Clear();
            _pool.Enqueue(item);
        }

        public static void Return(params Dictionary<TKey, TValue>[] items)
        {
            if (items == null)
                return;

            foreach (var item in items)
            {
                item?.Clear();
                _pool.Enqueue(item);
            }
        }

        public static void Return(IEnumerable<Dictionary<TKey, TValue>> items)
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