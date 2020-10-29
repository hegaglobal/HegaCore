namespace HegaCore
{
    public readonly struct TooltipData : IToTemplatedString
    {
        public readonly string Title;
        public readonly string Content;

        public TooltipData(string title, string content)
        {
            this.Title = title;
            this.Content = content;
        }

        public string ToTemplatedString(string format)
            => $"({this.Title}, {this.Content})";

        public static NoneData None { get; } = new NoneData();

        public readonly struct NoneData : IToTemplatedString
        {
            public string ToTemplatedString(string template)
                => template;
        }
    }
}