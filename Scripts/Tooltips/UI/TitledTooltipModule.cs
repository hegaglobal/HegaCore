namespace HegaCore.UI
{
    public sealed class TitledTooltipModule : TooltipModule
    {
        public override void Set(string l10nKey, IToTemplatedString template = null)
        {
            if (TrySetDefault(template))
                return;

            var tempData = template ?? TooltipData.None;

            var title = TooltipData.None.ToTemplatedString(L10n.Localize($"{l10nKey}-title"));
            SetTitle(title);

            var content = tempData.ToTemplatedString(L10n.Localize(l10nKey));
            SetContent(content);
        }
    }
}