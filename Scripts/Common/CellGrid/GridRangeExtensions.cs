using System.Grid;
using UnityEngine;

namespace HegaCore
{
    public static class GridRangeExtensions
    {
        public static GridIndex MinRowMinColumn(in this GridIndexRange self)
            => new GridIndex(
                Mathf.Min(self.Start.Row, self.End.Row),
                Mathf.Min(self.Start.Column, self.End.Column)
            );

        public static GridIndex MinRowMaxColumn(in this GridIndexRange self)
            => new GridIndex(
                Mathf.Min(self.Start.Row, self.End.Row),
                Mathf.Max(self.Start.Column, self.End.Column)
            );

        public static GridIndex MaxRowMinColumn(in this GridIndexRange self)
            => new GridIndex(
                Mathf.Max(self.Start.Row, self.End.Row),
                Mathf.Min(self.Start.Column, self.End.Column)
            );

        public static GridIndex MaxRowMaxColumn(in this GridIndexRange self)
            => new GridIndex(
                Mathf.Max(self.Start.Row, self.End.Row),
                Mathf.Max(self.Start.Column, self.End.Column)
            );
    }
}
