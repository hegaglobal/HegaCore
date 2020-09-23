using System.Collections.Generic;
using System.Grid;

namespace HegaCore
{
    public interface IGridOccupier
    {
        bool IsOccupied(in GridIndex index);

        void GetOccupied(ICollection<GridIndex> output);

        void MarkAsOccupied(in GridIndex index);
    }
}