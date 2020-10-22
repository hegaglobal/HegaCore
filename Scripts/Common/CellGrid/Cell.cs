using System;
using System.Grid;
using UnityEngine;

namespace HegaCore
{
    public readonly struct Cell : IEquatableReadOnlyStruct<Cell>
    {
        public readonly int Id;
        public readonly GridIndex Index;
        public readonly Vector3 Position;

        public int Row => this.Index.Row;

        public int Column => this.Index.Column;

        public float X => this.Position.x;

        public float Y => this.Position.y;

        public float Z => this.Position.z;

        public Cell(int id, in GridIndex index, in Vector3 position)
        {
            this.Id = id;
            this.Index = index;
            this.Position = position;
        }

        public void Deconstruct(out int id, out GridIndex index, out Vector3 position)
        {
            id = this.Id;
            index = this.Index;
            position = this.Position;
        }

        public Cell With(in int? Id = null, in GridIndex? Index = null, in Vector3? Position = null)
            => new Cell(
                Id ?? this.Id,
                Index ?? this.Index,
                Position ?? this.Position
            );

        public Cell With(in int? Id = null, in int? Row = null, in int? Column = null, in float? X = null, in float? Y = null, in float? Z = null)
            => new Cell(
                Id ?? this.Id,
                this.Index.With(Row, Column),
                this.Position.With(X, Y, Z)
            );

        public override bool Equals(object obj)
            => obj is Cell other &&
               this.Id == other.Id &&
               this.Index.Equals(in other.Index);

        public bool Equals(in Cell other)
            => this.Id == other.Id &&
               this.Index.Equals(in other.Index);

        public bool Equals(Cell other)
            => this.Id == other.Id &&
               this.Index.Equals(in other.Index);

        public override int GetHashCode()
        {
            var hashCode = 1230032429;
            hashCode = hashCode * -1521134295 + this.Id.GetHashCode();
            hashCode = hashCode * -1521134295 + this.Index.GetHashCode();
            return hashCode;
        }

        public override string ToString()
            => $"({this.Id}, {this.Index}, {this.Position})";

        public static Cell Zero { get; } = new Cell(0, GridIndex.Zero, Vector3.zero);

        public static implicit operator Cell(in (int id, GridIndex index, Vector3 position) value)
            => new Cell(value.id, value.index, value.position);

        public static implicit operator int(in Cell value)
            => value.Id;

        public static implicit operator GridIndex(in Cell value)
            => value.Index;

        public static implicit operator Vector3(in Cell value)
            => value.Position;

        public static bool operator ==(in Cell lhs, in Cell rhs)
            => lhs.Equals(in rhs);

        public static bool operator !=(in Cell lhs, in Cell rhs)
            => !lhs.Equals(in rhs);
    }
}
