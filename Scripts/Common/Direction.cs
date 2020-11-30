using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace HegaCore
{
    public enum Horizontal { Left, Center, Right }

    public enum Vertical { Top, Middle, Bottom }

    [InlineProperty]
    [Serializable]
    public struct Direction : IEquatable<Direction>
    {
        [HorizontalGroup(PaddingLeft = 6), LabelText("H"), LabelWidth(12), Tooltip(nameof(Horizontal))]
        public Horizontal Horizontal;

        [HorizontalGroup, LabelText("V"), LabelWidth(12), Tooltip(nameof(Vertical))]
        public Vertical Vertical;

        public Direction(Horizontal horizontal, Vertical vertical)
        {
            this.Horizontal = horizontal;
            this.Vertical = vertical;
        }

        public void Deconstruct(out Horizontal horizontal, out Vertical vertical)
        {
            horizontal = this.Horizontal;
            vertical = this.Vertical;
        }

        public Direction With(Horizontal? Horizontal = null, Vertical? Vertical = null)
            => new Direction(
                Horizontal ?? this.Horizontal,
                Vertical ?? this.Vertical
            );

        public override bool Equals(object obj)
            => obj is Direction other &&
               this.Horizontal == other.Horizontal &&
               this.Vertical == other.Vertical;

        public bool Equals(Direction other)
            => this.Horizontal == other.Horizontal &&
               this.Vertical == other.Vertical;

        public bool Equals(in Direction other)
            => this.Horizontal == other.Horizontal &&
               this.Vertical == other.Vertical;

        public override int GetHashCode()
        {
            var hashCode = 1238135884;
            hashCode = hashCode * -1521134295 + this.Horizontal.GetHashCode();
            hashCode = hashCode * -1521134295 + this.Vertical.GetHashCode();
            return hashCode;
        }

        public override string ToString()
            => $"({this.Horizontal}, {this.Vertical})";

        public static bool operator ==(in Direction lhs, in Direction rhs)
            => lhs.Horizontal == rhs.Horizontal && lhs.Vertical == rhs.Vertical;

        public static bool operator !=(in Direction lhs, in Direction rhs)
            => lhs.Horizontal != rhs.Horizontal || lhs.Vertical != rhs.Vertical;
    }
}