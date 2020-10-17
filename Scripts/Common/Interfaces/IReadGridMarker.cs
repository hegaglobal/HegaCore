using System.Collections.Generic;
using System.Grid;

namespace HegaCore
{
    public interface IReadGridMarker
    {
        bool IsMarked(in GridIndex index);

        void GetIndices(ICollection<GridIndex> output);
    }

    public interface IReadGridMarker<T> : IReadGridMarker
    {
        bool TryGetValue(in GridIndex key, out T value);

        void GetValues(ICollection<T> output);

        IEnumerator<GridValue<T>> GetEnumerator();
    }
}