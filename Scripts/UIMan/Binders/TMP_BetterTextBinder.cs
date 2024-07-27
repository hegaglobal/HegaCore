using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace UnuGames.MVVM
{
    [RequireComponent(typeof(TMP_Text))]
    [DisallowMultipleComponent]
    public class TMP_BetterTextBinder : BinderBase
    {
        protected TMP_Text text;

        private LayoutGroup _layoutGroup;
        private ContentSizeFitter _contentSizeFitter;
        public bool needRefresh = false;
        
        [HideInInspector]
        public BindingField textField = new BindingField("Text");

        [HideInInspector]
        public BindingField colorField = new BindingField("Color");

        [HideInInspector]
        public BindingField formatField = new BindingField("Format");

        [HideInInspector]
        public StringConverter textConverter = new StringConverter("Text");

        [HideInInspector]
        public ColorConverter colorConverter = new ColorConverter("Color");

        [HideInInspector]
        public StringConverter formatConverter = new StringConverter("Format");

        public string format;

        private string value = string.Empty;

        public override void Initialize(bool forceInit)
        {
            if (!CheckInitialize(forceInit))
                return;

            this.text = GetComponent<TMP_Text>();
            if (needRefresh)
            {
                _layoutGroup = transform.GetComponent<LayoutGroup>();
                _contentSizeFitter = transform.GetComponent<ContentSizeFitter>();
            }
            SubscribeOnChangedEvent(this.textField, OnUpdateText);
            SubscribeOnChangedEvent(this.colorField, OnUpdateColor);
            SubscribeOnChangedEvent(this.formatField, OnUpdateFormat);
        }

        private void OnUpdateText(object val)
        {
            SetValue(this.textConverter.Convert(val, this));
        }

        private void OnUpdateColor(object val)
        {
            this.text.color = this.colorConverter.Convert(val, this);
        }

        private void OnUpdateFormat(object val)
        {
            this.format = this.formatConverter.Convert(val, this);
            SetValue(this.value);
        }

        private void SetValue(string value)
        {
            this.value = value;
            this.text.SetText(string.IsNullOrEmpty(this.format) ? value : string.Format(this.format, value));
            Refresh();
        }
        
        protected void Refresh()
        {
            if (!needRefresh) return;
        
            var rectTransform = (RectTransform)transform;
            Refresh(rectTransform);
        }

        private void Refresh(RectTransform rectTransform)
        {
            if (rectTransform == null || !rectTransform.gameObject.activeSelf)
            {
                return;
            }

            foreach (RectTransform child in rectTransform)
            {
                Refresh(child);
            }
        
            if (_layoutGroup != null)
            {
                _layoutGroup.SetLayoutHorizontal();
                _layoutGroup.SetLayoutVertical();
            }

            if (_contentSizeFitter != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
            }
        }
    }
}