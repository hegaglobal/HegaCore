using System;

namespace HegaCore
{
    [Serializable]
    public readonly struct Digid : IEquatableReadOnlyStruct<Digid>
    {
        public readonly int Id;
        public readonly Gid Gid;

        public Digid(int id)
        {
            this.Id = id;
            this.Gid = Gid.Get();
        }

        public Digid(int id, in Gid gid)
        {
            this.Id = id;
            this.Gid = gid;
        }

        public override bool Equals(object obj)
            => obj is Digid other &&
               this.Id == other.Id && this.Gid == other.Gid;

        public bool Equals(in Digid other)
            => this.Id == other.Id && this.Gid == other.Gid;

        public bool Equals(Digid other)
            => this.Id == other.Id && this.Gid == other.Gid;

        public override int GetHashCode()
        {
            var hashCode = -2096442702;
            hashCode = hashCode * -1521134295 + this.Id.GetHashCode();
            hashCode = hashCode * -1521134295 + this.Gid.GetHashCode();
            return hashCode;
        }

        public static Digid None { get; } = new Digid(0, Gid.None);

        public static bool operator ==(in Digid lhs, in Digid rhs)
            => lhs.Gid == rhs.Gid && lhs.Id == rhs.Id;

        public static bool operator !=(in Digid lhs, in Digid rhs)
            => lhs.Gid != rhs.Gid || lhs.Id != rhs.Id;
    }
}