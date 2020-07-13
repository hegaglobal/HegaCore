namespace HegaCore
{
    public sealed class SimpleTooltipModule : TooltipModule
    {
        public override void Set(string l10nKey, IData data = null)
        {
            if (TrySetDefaultData(data))
                return;

            var content = (data ?? Data.None).ToString(L10n.Localize(l10nKey));
            SetContent(content);
        }
    }
}