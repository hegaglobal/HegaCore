namespace HegaCore
{
    public readonly struct Data : IData, IToTemplatedString
    {
        public int Id { get; }

        public string ToString(string format)
            => format;

        public string ToTemplatedString(string template)
            => template;

        public static Data None { get; } = default;
    }
}