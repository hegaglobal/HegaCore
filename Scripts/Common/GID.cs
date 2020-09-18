using System;

namespace HegaCore
{
    public readonly struct GID : IEquatableReadOnlyStruct<GID>, IComparableReadOnlyStruct<GID>
    {
        public readonly uint Id;

        private GID(uint id)
            => this.Id = id;

        public override bool Equals(object obj)
            => obj is GID other &&
               this.Id == other.Id;

        public bool Equals(in GID other)
            => this.Id == other.Id;

        public bool Equals(GID other)
            => this.Id == other.Id;

        public override int GetHashCode()
            => 2108858624 + this.Id.GetHashCode();

        public int CompareTo(in GID other)
            => this.Id.CompareTo(other.Id);

        public int CompareTo(GID other)
            => this.Id.CompareTo(other.Id);

        private static uint _GID = 0;

        public static GID Get()
            => new GID(_GID++);

        public static void Reset()
            => _GID = 0;

        public static implicit operator uint(in GID value)
            => value.Id;

        public static bool operator ==(in GID lhs, in GID rhs)
            => lhs.Id == rhs.Id;

        public static bool operator !=(in GID lhs, in GID rhs)
            => lhs.Id != rhs.Id;

        public static bool operator >(in GID lhs, in GID rhs)
            => lhs.Id > rhs.Id;

        public static bool operator <(in GID lhs, in GID rhs)
            => lhs.Id < rhs.Id;
    }
}
