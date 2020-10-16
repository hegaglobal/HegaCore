namespace HegaCore
{
    public interface ITimeProvider
    {
        float Time { get; }

        float DeltaTime { get; }

        float UnscaledDeltaTime { get; }
    }
}