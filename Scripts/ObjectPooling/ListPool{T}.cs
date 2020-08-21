using System.Collections.Generic;

namespace HegaCore
{
    public static class ListPool<T>
    {
        private static readonly Pool<List<T>> _pool = new Pool<List<T>>();

        public static List<T> Get()
        {
            var item = _pool.Get();
            item.Clear();

            return item;
        }

        public static void Return(List<T> item)
        {
            item?.Clear();
            _pool.Return(item);
        }

        public static void Return(params List<T>[] items)
        {
            if (items == null)
                return;

            foreach (var item in items)
            {
                item?.Clear();
                _pool.Return(item);
            }
        }

        public static void Return(IEnumerable<List<T>> items)
        {
            if (items == null)
                return;

            foreach (var item in items)
            {
                item?.Clear();
                _pool.Return(item);
            }
        }
    }
}