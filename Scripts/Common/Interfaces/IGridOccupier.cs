using System.Collections.Generic;
using System.Grid;

namespace HegaCore
{
    public interface IGridOccupier : IReadGridOccupier
    {
        void Mark(in GridIndex index);

        void Mark(in GridIndexRange range);

        void Mark(IEnumerable<GridIndex> indices);

        void Mark(in GridIndex index, in GridIndex valueIndex);

        void Mark(in GridIndexRange range, in GridIndex valueIndex);

        void Mark(IEnumerable<GridIndex> indices, in GridIndex valueIndex);

        void Unmark(in GridIndex index);

        void Unmark(in GridIndexRange range);

        void Unmark(IEnumerable<GridIndex> indices);

        void Clear();
    }

    public interface IGridOccupier<T> : IGridOccupier, IReadGridOccupier<T>
    {
        void Mark(in GridIndex index, T value);

        void Mark(in GridIndexRange range, T value);

        void Mark(IEnumerable<GridIndex> indices, T value);

        void Unmark(T value);
    }
}