using UnityEngine;
using TMPro;
using UnuGames.MVVM;

namespace HegaCore.MVVM
{
    [RequireComponent(typeof(TMP_Text))]
    [DisallowMultipleComponent]
    public class TMP_L10nTextBinder : BinderBase, IL10n
    {
        protected TMP_Text text;

        [HideInInspector]
        public BindingField textField = new BindingField("Text");

        [HideInInspector]
        public BindingField colorField = new BindingField("Color");

        [HideInInspector]
        public BindingField formatKeyField = new BindingField("Format Key");

        [HideInInspector]
        public StringConverter textConverter = new StringConverter("Text");

        [HideInInspector]
        public ColorConverter colorConverter = new ColorConverter("Color");

        [HideInInspector]
        public StringConverter formatKeyConverter = new StringConverter("Format Key");

        public string formatKey;
        public bool shouldLocalizeText;
        public bool silent;

        public string Key
            => this.formatKey;

        private string value = string.Empty;

        private void OnDestroy()
        {
            if (SingletonBehaviour.Quitting)
                return;

            L10n.Deregister(this);
        }

        public override void Initialize(bool forceInit)
        {
            if (!CheckInitialize(forceInit))
                return;

            this.text = GetComponent<TMP_Text>();

            L10n.Register(this);
            Localize();

            SubscribeOnChangedEvent(this.textField, OnUpdateText);
            SubscribeOnChangedEvent(this.colorField, OnUpdateColor);
            SubscribeOnChangedEvent(this.formatKeyField, OnUpdateFormatKey);
        }

        private void OnUpdateText(object val)
        {
            this.value = this.textConverter.Convert(val, this);
            Localize();
        }

        private void OnUpdateColor(object val)
        {
            this.text.color = this.colorConverter.Convert(val, this);
        }

        private void OnUpdateFormatKey(object val)
        {
            var value = this.formatKeyConverter.Convert(val, this);
            SetKey(value);
        }

        public void SetKey(string value)
        {
            if (string.Equals(this.formatKey, value))
                return;

            this.formatKey = value ?? string.Empty;
            Localize();
        }

        public void Localize()
        {
            string text;

            if (string.IsNullOrEmpty(this.formatKey))
            {
                text = GetValue();
            }
            else
            {
                var format = L10n.Localize(this.formatKey, this.silent, keyAsDefault: true);
                text = string.Format(format, GetValue());
            }

            this.text.SetText(text);
        }

        private string GetValue()
            => this.shouldLocalizeText ? L10n.Localize(this.value, this.silent, keyAsDefault: true) : this.value;
    }
}