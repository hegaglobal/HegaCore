using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace HegaCore
{
    [InlineProperty]
    [Serializable]
    public struct GridVector : IEquatable<GridVector>
    {
        [HorizontalGroup, LabelText("R"), LabelWidth(12), Tooltip(nameof(Row)), MinValue(0)]
        [SerializeField, Min(0)]
        private int row;

        public int Row
        {
            get => this.row;
            set => this.row = Mathf.Max(value, 0);
        }

        [HorizontalGroup, LabelText("C"), LabelWidth(12), Tooltip(nameof(Column)), MinValue(0)]
        [SerializeField, Min(0)]
        private int column;

        public int Column
        {
            get => this.column;
            set => this.column = Mathf.Max(value, 0);
        }

        public GridVector(int row, int column)
        {
            this.row = Mathf.Max(row, 0);
            this.column = Mathf.Max(column, 0);
        }

        public void Deconstruct(out int row, out int column)
        {
            row = this.row;
            column = this.column;
        }

        public override bool Equals(object obj)
            => obj is GridVector other && this.row == other.row && this.column == other.column;

        public bool Equals(GridVector other)
            => this.row == other.row && this.column == other.column;

        public bool Equals(in GridVector other)
            => this.row == other.row && this.column == other.column;

        public override int GetHashCode()
        {
            var hashCode = -1663278630;
            hashCode = hashCode * -1521134295 + this.row.GetHashCode();
            hashCode = hashCode * -1521134295 + this.column.GetHashCode();
            return hashCode;
        }

        public override string ToString()
            => $"({this.row}, {this.column})";

        public static implicit operator GridVector(in (int row, int column) value)
            => new GridVector(value.row, value.column);

        public static bool operator ==(in GridVector lhs, in GridVector rhs)
            => lhs.row == rhs.row && lhs.column == rhs.column;

        public static bool operator !=(in GridVector lhs, in GridVector rhs)
            => lhs.row != rhs.row || lhs.column != rhs.column;
    }
}