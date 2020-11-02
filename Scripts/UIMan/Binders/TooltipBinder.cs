using UnityEngine;
using UnuGames.MVVM;

namespace HegaCore.MVVM
{
    using UI;

    [RequireComponent(typeof(Tooltip))]
    public class TooltipBinder : BinderBase
    {
        protected Tooltip tooltip;

        [HideInInspector]
        public BindingField l10nKeyField = new BindingField("L10n Key");

        [HideInInspector]
        public BindingField templatedStringField = new BindingField(nameof(IToTemplatedString));

        [HideInInspector]
        public StringConverter l10nKeyConverter = new StringConverter("L10n Key");

        [HideInInspector]
        public IToTemplatedStringConverter templatedStringConverter = new IToTemplatedStringConverter(nameof(IToTemplatedString));

        public override void Initialize(bool forceInit = false)
        {
            if (!CheckInitialize(forceInit))
                return;

            this.tooltip = GetComponent<Tooltip>();

            SubscribeOnChangedEvent(this.l10nKeyField, OnUpdateL10nKey);
            SubscribeOnChangedEvent(this.templatedStringField, OnUpdateTemplatedString);
        }

        public void OnUpdateL10nKey(object val)
        {
            var value = this.l10nKeyConverter.Convert(val, this);
            this.tooltip.Set(value);
        }

        public void OnUpdateTemplatedString(object val)
        {
            var template = this.templatedStringConverter.Convert(val, this);
            this.tooltip.Set(template);
        }
    }
}