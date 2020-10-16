using System.Collections.Generic;
using System.Grid;

namespace HegaCore
{
    public interface IReadGridOccupier
    {
        bool IsOccupied(in GridIndex index);

        void GetIndices(ICollection<GridIndex> output);
    }

    public interface IReadGridOccupier<T> : IReadGridOccupier
    {
        bool TryGetValue(in GridIndex key, out T value);

        void GetValues(ICollection<T> output);

        IEnumerator<GridValue<T>> GetEnumerator();
    }
}