namespace HegaCore
{
    public interface ITooltip
    {
        void SetData(string l10nKey, IData data = null);

        void ResetData();
    }
}