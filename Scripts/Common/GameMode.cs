namespace HegaCore
{
    public enum GameMode
    {
        Normal,
        Easy,
        Hard
    }

    public static class GameModeExtensions
    {
        public static class SimpleName
        {
            public const string Normal = "N";
            public const string Easy = "E";
            public const string Hard = "H";
        }

        public static string ToSimpleName(this GameMode self)
        {
            switch (self)
            {
                case GameMode.Easy: return SimpleName.Easy;
                case GameMode.Hard: return SimpleName.Hard;
                default: return SimpleName.Normal;
            }
        }
    }
}