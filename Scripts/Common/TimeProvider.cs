namespace HegaCore
{
    public readonly struct TimeProvider : ITimeProvider
    {
        public float Time => UnityEngine.Time.time;

        public float DeltaTime => UnityEngine.Time.deltaTime;

        public float UnscaledDeltaTime => UnityEngine.Time.unscaledDeltaTime;
    }
}