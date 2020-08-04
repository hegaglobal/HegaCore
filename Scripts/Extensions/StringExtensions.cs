namespace HegaCore
{
    public static class StringExtensions
    {
        public static string OrDarkLord(this string self, bool darkLord)
            => string.IsNullOrEmpty(self) ? string.Empty : (darkLord ? $"{self}-{Strings.DarkLord}" : self);
    }
}