using System.Collections.ArrayBased;

namespace HegaCore
{
    public static class ArrayDictionaryExtensions
    {
        public static void GetUnsafe<TKey, TValue>(this ArrayDictionary<TKey, TValue> self,
                                                   out ArrayDictionary<TKey, TValue>.Node[] keys,
                                                   out TValue[] values, out uint count)
        {
            keys = self.UnsafeKeys;
            values = self.UnsafeValues;
            count = self.Count;
        }

        public static void GetUnsafeKeys<TKey, TValue>(this ArrayDictionary<TKey, TValue> self,
                                                       out ArrayDictionary<TKey, TValue>.Node[] keys,
                                                       out uint count)
        {
            keys = self.UnsafeKeys;
            count = self.Count;
        }

        public static void GetUnsafeValues<TKey, TValue>(this ArrayDictionary<TKey, TValue> self,
                                                         out TValue[] values, out uint count)
        {
            values = self.UnsafeValues;
            count = self.Count;
        }
    }
}