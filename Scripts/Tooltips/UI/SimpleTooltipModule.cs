namespace HegaCore.UI
{
    public sealed class SimpleTooltipModule : TooltipModule
    {
        public override void Set(string l10nKey, IToTemplatedString template = null)
        {
            if (TrySetDefault(template))
                return;

            var content = (template ?? TooltipData.None).ToTemplatedString(L10n.Localize(l10nKey));
            SetContent(content);
        }
    }
}