namespace HegaCore
{
    public static class GameTime
    {
        public static ITimeProvider Provider { get; private set; }

        static GameTime()
        {
            Provider = new TimeProvider();
        }

        public static void Use(ITimeProvider provider)
            => Provider = provider ?? new TimeProvider();

        public static void Use<T>() where T : ITimeProvider, new()
            => Provider = new T();
    }
}
