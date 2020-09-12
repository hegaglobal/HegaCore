using System;
using UnityEngine;

namespace HegaCore
{
    public readonly struct Position5 : IPosition5, IEquatableReadOnlyStruct<Position5>
    {
        public readonly Vector3 Above;
        public readonly Vector2 Below;
        public readonly Vector3 Ahead;
        public readonly Vector3 Behind;
        public readonly Vector3 Center;

        public Position5(in Vector3 above, in Vector3 below, in Vector3 ahead, in Vector3 behind, in Vector3 center)
        {
            this.Above = above;
            this.Below = below;
            this.Ahead = ahead;
            this.Behind = behind;
            this.Center = center;
        }

        public void Deconstruct(out Vector3 above, out Vector3 below, out Vector3 ahead, out Vector3 behind, out Vector3 center)
        {
            above = this.Above;
            below = this.Below;
            ahead = this.Ahead;
            behind = this.Behind;
            center = this.Center;
        }

        public Position5 With(in Vector3? Above = null, in Vector3? Below = null, in Vector3? Ahead = null,
                              in Vector3? Behind = null, in Vector3? Center = null)
            => new Position5(
                Above ?? this.Above,
                Below ?? this.Below,
                Ahead ?? this.Ahead,
                Behind ?? this.Behind,
                Center ?? this.Center
            );

        public override bool Equals(object obj)
            => obj is Position5 other && Equals(in other);

        public bool Equals(in Position5 other)
            => this.Above.Equals(other.Above) &&
               this.Below.Equals(other.Below) &&
               this.Ahead.Equals(other.Ahead) &&
               this.Behind.Equals(other.Behind) &&
               this.Center.Equals(other.Center);

        public bool Equals(Position5 other)
            => this.Above.Equals(other.Above) &&
               this.Below.Equals(other.Below) &&
               this.Ahead.Equals(other.Ahead) &&
               this.Behind.Equals(other.Behind) &&
               this.Center.Equals(other.Center);

        public override int GetHashCode()
        {
            var hashCode = -2007588305;
            hashCode = hashCode * -1521134295 + this.Above.GetHashCode();
            hashCode = hashCode * -1521134295 + this.Below.GetHashCode();
            hashCode = hashCode * -1521134295 + this.Ahead.GetHashCode();
            hashCode = hashCode * -1521134295 + this.Behind.GetHashCode();
            hashCode = hashCode * -1521134295 + this.Center.GetHashCode();
            return hashCode;
        }

        Vector3 IPosition5.Above => this.Above;

        Vector3 IPosition5.Below => this.Below;

        Vector3 IPosition5.Ahead => this.Ahead;

        Vector3 IPosition5.Behind => this.Behind;

        Vector3 IPosition5.Center => this.Center;

        public static Position5 Zero { get; } = new Position5();
    }
}