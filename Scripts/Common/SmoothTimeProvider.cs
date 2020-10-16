namespace HegaCore
{
    public readonly struct SmoothTimeProvider : ITimeProvider
    {
        public float Time => UnityEngine.Time.time;

        public float DeltaTime => UnityEngine.Time.smoothDeltaTime;

        public float UnscaledDeltaTime => UnityEngine.Time.unscaledDeltaTime;
    }
}