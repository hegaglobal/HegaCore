namespace HegaCore
{
    public static class ArrayExtensions
    {
        public static T[] OrEmpty<T>(this T[] self)
            => self ?? Array<T>.Empty;

        private static class Array<T>
        {
            public static T[] Empty { get; } = new T[0];
        }
    }
}