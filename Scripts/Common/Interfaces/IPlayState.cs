namespace HegaCore
{
    public interface IPlayState
    {
        PlayState PlayState { get; }

        void SetPlayState(PlayState value);
    }
}