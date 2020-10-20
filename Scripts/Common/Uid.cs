using System;

namespace HegaCore
{
    public readonly struct Uid : IEquatableReadOnlyStruct<Uid>, IComparableReadOnlyStruct<Uid>,
                                 IEquatable<uint>, IComparable<uint>
    {
        private readonly uint value;

        private Uid(uint id)
            => this.value = id;

        public override bool Equals(object obj)
        {
            switch (obj)
            {
                case Uid gid: return this.value == gid.value;
                case uint @uint: return this.value == @uint;
                default: return false;
            }
        }

        public bool Equals(uint other)
            => this.value == other;

        public bool Equals(in Uid other)
            => this.value == other.value;

        public bool Equals(Uid other)
            => this.value == other.value;

        public override int GetHashCode()
            => 2108858624 + this.value.GetHashCode();

        public override string ToString()
            => this.value.ToString();

        public int CompareTo(in Uid other)
            => this.value.CompareTo(other.value);

        public int CompareTo(Uid other)
            => this.value.CompareTo(other.value);

        public int CompareTo(uint other)
            => this.value.CompareTo(other);

        private const uint _None = 0;
        private const uint _First = 1;
        private static uint _Gid = _First;

        public static Uid None { get; } = new Uid(_None);

        public static Uid Get()
            => new Uid(_Gid++);

        public static void Reset()
            => _Gid = _First;

        public static implicit operator uint(in Uid value)
            => value.value;

        public static explicit operator Uid(uint value)
            => new Uid(value);

        public static bool operator ==(in Uid lhs, in Uid rhs)
            => lhs.value == rhs.value;

        public static bool operator !=(in Uid lhs, in Uid rhs)
            => lhs.value != rhs.value;

        public static bool operator >(in Uid lhs, in Uid rhs)
            => lhs.value > rhs.value;

        public static bool operator <(in Uid lhs, in Uid rhs)
            => lhs.value < rhs.value;

        public static bool operator ==(in Uid lhs, uint rhs)
            => lhs.value == rhs;

        public static bool operator !=(in Uid lhs, uint rhs)
            => lhs.value != rhs;

        public static bool operator >(in Uid lhs, uint rhs)
            => lhs.value > rhs;

        public static bool operator <(in Uid lhs, uint rhs)
            => lhs.value < rhs;

        public static bool operator ==(uint lhs, in Uid rhs)
            => lhs == rhs.value;

        public static bool operator !=(uint lhs, in Uid rhs)
            => lhs != rhs.value;

        public static bool operator >(uint lhs, in Uid rhs)
            => lhs > rhs.value;

        public static bool operator <(uint lhs, in Uid rhs)
            => lhs < rhs.value;
    }
}
