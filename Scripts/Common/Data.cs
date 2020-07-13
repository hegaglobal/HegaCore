namespace HegaCore
{
    public readonly struct Data : IData
    {
        public int Id { get; }

        public string ToString(string format)
            => format;

        public static Data None { get; } = default;
    }
}