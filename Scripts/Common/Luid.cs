using System;

namespace HegaCore
{
    public readonly struct Luid : IEquatableReadOnlyStruct<Luid>
    {
        public readonly int Id;
        public readonly Uid Uid;

        public Luid(int id)
        {
            this.Id = id;
            this.Uid = Uid.New();
        }

        internal Luid(int id, in Uid gid)
        {
            this.Id = id;
            this.Uid = gid;
        }

        public override bool Equals(object obj)
            => obj is Luid other &&
               this.Id == other.Id && this.Uid == other.Uid;

        public bool Equals(in Luid other)
            => this.Id == other.Id && this.Uid == other.Uid;

        public bool Equals(Luid other)
            => this.Id == other.Id && this.Uid == other.Uid;

        public override int GetHashCode()
        {
            var hashCode = -2096442702;
            hashCode = hashCode * -1521134295 + this.Id.GetHashCode();
            hashCode = hashCode * -1521134295 + this.Uid.GetHashCode();
            return hashCode;
        }

        public override string ToString()
            => $"{this.Id}-{this.Uid}";

        public static Luid None { get; } = new Luid(0, Uid.None);

        public static bool operator ==(in Luid lhs, in Luid rhs)
            => lhs.Uid == rhs.Uid && lhs.Id == rhs.Id;

        public static bool operator !=(in Luid lhs, in Luid rhs)
            => lhs.Uid != rhs.Uid || lhs.Id != rhs.Id;
    }
}