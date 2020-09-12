using System;
using System.Collections.Generic;

namespace HegaCore
{
    public readonly struct GridCell<T> : IEquatableReadOnlyStruct<GridCell<T>>
    {
        public readonly GridIndex Index;
        public readonly T Cell;

        public GridCell(in GridIndex index, T cell)
        {
            this.Index = index;
            this.Cell = cell;
        }

        public override bool Equals(object obj)
            => obj is GridCell<T> other &&
               this.Index.Equals(in other.Index) &&
               EqualityComparer<T>.Default.Equals(this.Cell, other.Cell);

        public bool Equals(in GridCell<T> other)
            => this.Index.Equals(in other.Index) &&
               EqualityComparer<T>.Default.Equals(this.Cell, other.Cell);

        public bool Equals(GridCell<T> other)
            => this.Index.Equals(in other.Index) &&
               EqualityComparer<T>.Default.Equals(this.Cell, other.Cell);

        public override int GetHashCode()
        {
            var hashCode = 1774931160;
            hashCode = hashCode * -1521134295 + this.Index.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<T>.Default.GetHashCode(this.Cell);
            return hashCode;
        }
    }
}