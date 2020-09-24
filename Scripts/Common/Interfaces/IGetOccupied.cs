using System.Collections.Generic;
using System.Grid;

namespace HegaCore
{
    public interface IGetOccupied
    {
        bool IsOccupied(in GridIndex index);

        void GetOccupied(ICollection<GridIndex> output);
    }
}