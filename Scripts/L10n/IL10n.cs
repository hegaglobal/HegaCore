namespace HegaCore
{
    public interface IL10n
    {
        string Key { get; }

        void SetKey(string value);

        void Localize();
    }
}
