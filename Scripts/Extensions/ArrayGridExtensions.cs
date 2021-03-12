using System.Collections.ArrayBased;
using System.Grid;
using System.Grid.ArrayBased;

namespace HegaCore
{
    public static class ArrayGridExtensions
    {
        public static void GetUnsafe<T>(this ArrayGrid<T> self,
                                        out ArrayDictionary<GridIndex, T>.Node[] indices,
                                        out T[] values, out uint count)
        {
            indices = self.UnsafeIndices;
            values = self.UnsafeValues;
            count = self.Count;
        }

        public static void GetUnsafeIndices<T>(this ArrayGrid<T> self,
                                               out ArrayDictionary<GridIndex, T>.Node[] indices,
                                               out uint count)
        {
            indices = self.UnsafeIndices;
            count = self.Count;
        }

        public static void GetUnsafeValues<T>(this ArrayGrid<T> self,
                                              out T[] values, out uint count)
        {
            values = self.UnsafeValues;
            count = self.Count;
        }
    }
}