using UnityEngine;
using Sirenix.OdinInspector;

namespace HegaCore.UI
{
    public sealed class TitledTooltipModule : TooltipModule
    {
        [SerializeField, BoxGroup("L10n Key Format")]
        private string titleFormat = string.Empty;

        [SerializeField, BoxGroup("L10n Key Format")]
        private string contentFormat = string.Empty;

        public override void Set(string l10nKey, IToTemplatedString template = null)
        {
            if (TrySetDefault(template))
                return;

            var tempData = template ?? TooltipData.None;

            var titleKey = l10nKey;
            var contentKey = l10nKey;

            if (!string.IsNullOrEmpty(this.titleFormat))
                titleKey = string.Format(this.titleFormat, l10nKey);

            if (!string.IsNullOrEmpty(this.contentFormat))
                contentKey = string.Format(this.contentFormat, l10nKey);

            var title = TooltipData.None.ToTemplatedString(L10n.Localize(titleKey));
            SetTitle(title);

            var content = tempData.ToTemplatedString(L10n.Localize(contentKey));
            SetContent(content);
        }
    }
}