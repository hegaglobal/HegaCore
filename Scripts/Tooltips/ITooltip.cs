namespace HegaCore
{
    public interface ITooltip
    {
        void Set(string l10nKey, IToTemplatedString template = null);

        void Unset();
    }
}