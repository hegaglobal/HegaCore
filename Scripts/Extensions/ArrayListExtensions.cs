using System.Collections.ArrayBased;

namespace HegaCore
{
    public static class ArrayListExtensions
    {
        public static void GetUnsafe<T>(this ArrayList<T> self,
                                        out T[] values, out uint count)
        {
            values = self.UnsafeBuffer;
            count = self.Count;
        }
    }
}