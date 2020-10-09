using System;

namespace HegaCore
{
    public readonly struct CharacterId : IEquatableReadOnlyStruct<CharacterId>
    {
        public readonly int Character;
        public readonly int Variant;
        public readonly int Index;

        public CharacterId(int character, int variant)
        {
            this.Character = character;
            this.Variant = variant;
            this.Index = new Index2(character, variant).ToIndex1(CharacterCount);
        }

        public override bool Equals(object obj)
            => obj is CharacterId other &&
               this.Character == other.Character &&
               this.Variant == other.Variant;

        public bool Equals(in CharacterId other)
            => this.Character == other.Character &&
               this.Variant == other.Variant;

        public bool Equals(CharacterId other)
            => this.Character == other.Character &&
               this.Variant == other.Variant;

        public override int GetHashCode()
        {
            var hashCode = -1852605354;
            hashCode = hashCode * -1521134295 + this.Character.GetHashCode();
            hashCode = hashCode * -1521134295 + this.Variant.GetHashCode();
            return hashCode;
        }

        public static int CharacterCount { get; set; } = 0;

        public static implicit operator int(in CharacterId value)
            => value.Index;

        public static bool operator ==(in CharacterId lhs, in CharacterId rhs)
            => lhs.Character == rhs.Character && lhs.Variant == rhs.Variant;

        public static bool operator !=(in CharacterId lhs, in CharacterId rhs)
            => lhs.Character != rhs.Character || lhs.Variant != rhs.Variant;
    }
}