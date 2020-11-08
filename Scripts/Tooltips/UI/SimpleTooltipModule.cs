using UnityEngine;
using Sirenix.OdinInspector;

namespace HegaCore.UI
{
    public sealed class SimpleTooltipModule : TooltipModule
    {
        [SerializeField, BoxGroup("L10n Key Format")]
        private string contentFormat = string.Empty;

        public override void Set(string l10nKey, IToTemplatedString template = null)
        {
            if (TrySetDefault(template))
                return;

            if (!string.IsNullOrEmpty(this.contentFormat))
                l10nKey = string.Format(this.contentFormat, l10nKey);

            var content = (template ?? TooltipData.None).ToTemplatedString(L10n.Localize(l10nKey));
            SetContent(content);
        }
    }
}