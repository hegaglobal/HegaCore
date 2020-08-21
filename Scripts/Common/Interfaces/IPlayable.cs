namespace HegaCore
{
    public interface IPlayable
    {
        PlayState State { get; }

        void SetState(PlayState value);
    }
}