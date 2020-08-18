namespace HegaCore.UI
{
    public sealed class TitledTooltipModule : TooltipModule
    {
        public override void Set(string l10nKey, IData data = null)
        {
            if (TrySetDefaultData(data))
                return;

            var tempData = data ?? Data.None;

            var title = Data.None.ToString(L10n.Localize($"{l10nKey}-title"));
            SetTitle(title);

            var content = tempData.ToString(L10n.Localize(l10nKey));
            SetContent(content);
        }
    }
}