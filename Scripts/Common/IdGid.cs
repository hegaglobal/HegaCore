using System;

namespace HegaCore
{
    [Serializable]
    public readonly struct IdGid : IEquatableReadOnlyStruct<IdGid>
    {
        public readonly int Id;
        public readonly Gid Gid;

        public IdGid(int id)
        {
            this.Id = id;
            this.Gid = Gid.Get();
        }

        internal IdGid(int id, in Gid gid)
        {
            this.Id = id;
            this.Gid = gid;
        }

        public override bool Equals(object obj)
            => obj is IdGid other &&
               this.Id == other.Id && this.Gid == other.Gid;

        public bool Equals(in IdGid other)
            => this.Id == other.Id && this.Gid == other.Gid;

        public bool Equals(IdGid other)
            => this.Id == other.Id && this.Gid == other.Gid;

        public override int GetHashCode()
        {
            var hashCode = -2096442702;
            hashCode = hashCode * -1521134295 + this.Id.GetHashCode();
            hashCode = hashCode * -1521134295 + this.Gid.GetHashCode();
            return hashCode;
        }

        public override string ToString()
            => $"{this.Id}-{this.Gid}";

        public static IdGid None { get; } = new IdGid(0, Gid.None);

        public static bool operator ==(in IdGid lhs, in IdGid rhs)
            => lhs.Gid == rhs.Gid && lhs.Id == rhs.Id;

        public static bool operator !=(in IdGid lhs, in IdGid rhs)
            => lhs.Gid != rhs.Gid || lhs.Id != rhs.Id;
    }
}