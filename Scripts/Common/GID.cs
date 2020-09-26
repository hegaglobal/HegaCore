using System;

namespace HegaCore
{
    public readonly struct Gid : IEquatableReadOnlyStruct<Gid>, IComparableReadOnlyStruct<Gid>,
                                 IEquatable<uint>, IComparable<uint>
    {
        private readonly uint value;

        private Gid(uint id)
            => this.value = id;

        public override bool Equals(object obj)
        {
            switch (obj)
            {
                case Gid gid: return this.value == gid.value;
                case uint @uint: return this.value == @uint;
                default: return false;
            }
        }

        public bool Equals(uint other)
            => this.value == other;

        public bool Equals(in Gid other)
            => this.value == other.value;

        public bool Equals(Gid other)
            => this.value == other.value;

        public override int GetHashCode()
            => 2108858624 + this.value.GetHashCode();

        public int CompareTo(in Gid other)
            => this.value.CompareTo(other.value);

        public int CompareTo(Gid other)
            => this.value.CompareTo(other.value);

        public int CompareTo(uint other)
            => this.value.CompareTo(other);

        private const uint _None = 0;
        private const uint _First = 1;
        private static uint _Gid = _First;

        public static Gid None { get; } = new Gid(_None);

        public static Gid Get()
            => new Gid(_Gid++);

        public static void Reset()
            => _Gid = _First;

        public static implicit operator uint(in Gid value)
            => value.value;

        public static explicit operator Gid(uint value)
            => new Gid(value);

        public static bool operator ==(in Gid lhs, in Gid rhs)
            => lhs.value == rhs.value;

        public static bool operator !=(in Gid lhs, in Gid rhs)
            => lhs.value != rhs.value;

        public static bool operator >(in Gid lhs, in Gid rhs)
            => lhs.value > rhs.value;

        public static bool operator <(in Gid lhs, in Gid rhs)
            => lhs.value < rhs.value;

        public static bool operator ==(in Gid lhs, uint rhs)
            => lhs.value == rhs;

        public static bool operator !=(in Gid lhs, uint rhs)
            => lhs.value != rhs;

        public static bool operator >(in Gid lhs, uint rhs)
            => lhs.value > rhs;

        public static bool operator <(in Gid lhs, uint rhs)
            => lhs.value < rhs;

        public static bool operator ==(uint lhs, in Gid rhs)
            => lhs == rhs.value;

        public static bool operator !=(uint lhs, in Gid rhs)
            => lhs != rhs.value;

        public static bool operator >(uint lhs, in Gid rhs)
            => lhs > rhs.value;

        public static bool operator <(uint lhs, in Gid rhs)
            => lhs < rhs.value;
    }
}
