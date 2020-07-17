namespace HegaCore
{
    public readonly struct Language
    {
        public readonly string Key;
        public readonly string Name;

        public Language(string key, string name)
        {
            this.Key = key;
            this.Name = name;
        }

        public override bool Equals(object obj)
            => obj is Language other && string.Equals(this.Key, other.Key);

        public override int GetHashCode()
            => this.Key.Or(string.Empty).GetHashCode();

        public override string ToString()
            => this.Name;
    }
}