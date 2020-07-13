namespace HegaCore
{
    public readonly struct TooltipData : IData
    {
        public int Id { get; }

        public readonly string Title;
        public readonly string Content;

        public TooltipData(string title, string content)
        {
            this.Id = 0;
            this.Title = title;
            this.Content = content;
        }

        public string ToString(string format)
            => $"({this.Title}, {this.Content})";
    }
}