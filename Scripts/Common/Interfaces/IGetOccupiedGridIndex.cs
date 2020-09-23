using System.Collections.Generic;
using System.Grid;

namespace HegaCore
{
    public interface IGetOccupiedGridIndex
    {
        bool IsOccupied(in GridIndex index);

        void GetOccupiedGridIndex(ICollection<GridIndex> output);
    }
}