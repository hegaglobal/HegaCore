using System.Collections.Generic;
using System.Grid;

namespace HegaCore
{
    public readonly struct GridIndex1Comparer : IComparer<GridIndex>
    {
        private readonly GridSize size;
        private readonly bool reversed;

        public GridIndex1Comparer(in GridSize size) : this(size, false)
        {
        }

        public GridIndex1Comparer(in GridSize size, bool reversed)
        {
            this.size = size;
            this.reversed = reversed;
        }

        public int Compare(GridIndex x, GridIndex y)
            => this.reversed
               ? this.size.Index1Of(y).CompareTo(this.size.Index1Of(x))
               : this.size.Index1Of(x).CompareTo(this.size.Index1Of(y));
    }
}
