namespace HegaCore
{
    public readonly struct ActorDuration
    {
        public readonly float Show;
        public readonly float Hide;
        public readonly float Color;

        public ActorDuration(float show, float hide, float color)
        {
            this.Show = show;
            this.Hide = hide;
            this.Color = color;
        }

        public static implicit operator ActorDuration(in (float show, float hide, float color) value)
            => new ActorDuration(value.show, value.hide, value.color);
    }
}