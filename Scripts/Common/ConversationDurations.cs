namespace HegaCore
{
    public readonly struct ConversationDurations
    {
        public readonly float Show;
        public readonly float Hide;
        public readonly float Color;

        public ConversationDurations(float show, float hide, float color)
        {
            this.Show = show;
            this.Hide = hide;
            this.Color = color;
        }

        public void Deconstruct(out float show, out float hide, out float color)
        {
            show = this.Show;
            hide = this.Hide;
            color = this.Color;
        }

        public ConversationDurations With(float? Show = null, float? Hide = null, float? Color = null)
            => new ConversationDurations(
                Show ?? this.Show,
                Hide ?? this.Hide,
                Color ?? this.Color
            );

        public static implicit operator ConversationDurations(in (float show, float hide, float color) value)
            => new ConversationDurations(value.show, value.hide, value.color);
    }
}