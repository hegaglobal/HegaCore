namespace HegaCore
{
    public static class StringExtensions
    {
        public static string OrDarkLord(this string self, bool darkLord)
            => string.IsNullOrEmpty(self) ? string.Empty : (darkLord ? $"{self}-{Strings.DarkLord}" : self);

        public static bool NotNull(this string self, out string value)
        {
            value = self;
            return self != null;
        }

        public static bool NotNullOrEmpty(this string self, out string value)
        {
            value = self;
            return !string.IsNullOrEmpty(self);
        }

        public static bool NotNullOrWhiteSpace(this string self, out string value)
        {
            value = self;
            return !string.IsNullOrWhiteSpace(self);
        }
    }
}