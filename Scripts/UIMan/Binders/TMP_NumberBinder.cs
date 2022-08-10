using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UnuGames.MVVM
{
    [RequireComponent(typeof(TMP_Text))]
    [DisallowMultipleComponent]
    public class TMP_NumberBinder : BinderBase
    {
        protected TMP_Text text;

        [HideInInspector]
        public BindingField valueField = new BindingField("Number");

        [HideInInspector]
        public BindingField colorField = new BindingField("Color");

        [HideInInspector]
        public BindingField formatField = new BindingField("Format");

        [HideInInspector]
        public BindingField durationField = new BindingField("Duration");

        [HideInInspector]
        public FloatConverter valueConverter = new FloatConverter("Number");

        [HideInInspector]
        public ColorConverter colorConverter = new ColorConverter("Color");

        [HideInInspector]
        public StringConverter formatConverter = new StringConverter("Format");

        [HideInInspector]
        public FloatConverter durationConverter = new FloatConverter("Duration");

        public string format;
        public float duration = 0.25f;
        public bool wholeNumber = false;
        private float value = 0f;

        public override void Initialize(bool forceInit)
        {
            if (!CheckInitialize(forceInit))
                return;

            this.text = GetComponent<TMP_Text>();
            SubscribeOnChangedEvent(this.valueField, OnUpdateNumber);
            SubscribeOnChangedEvent(this.colorField, OnUpdateColor);
            SubscribeOnChangedEvent(this.formatField, OnUpdateFormat);
            SubscribeOnChangedEvent(this.durationField, OnUpdateDuration);
        }

        private void OnUpdateNumber(object newVal)
        {
            var oldValue = this.value;
            var newValue = this.valueConverter.Convert(newVal, this);

            if (this.duration <= 0f)
            {
                SetValue(newValue);
                return;
            }

            UITweener.Value(this.gameObject, this.duration, oldValue, newValue)
                     .SetOnUpdate(SetValue)
                     .SetOnComplete(() => SetValue(newValue));
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

        private void OnUpdateDuration(object val)
        {
            this.duration = this.durationConverter.Convert(val, this);
        }

        private void SetValue(float value)
        {
            if (string.IsNullOrEmpty(this.format))
            {
                if (wholeNumber)
                {
                    int parseInt = (int)value;
                    this.text.text = parseInt.ToString();
                    this.value = parseInt;
                }
                else
                {
                    this.text.text = value.ToString();
                    this.value = value;
                }
            }
            else
            {
                if (wholeNumber)
                {
                    int parseInt = (int)value;
                    this.text.text = string.Format(this.format, parseInt);
                    this.value = parseInt;
                }
                else
                {
                    this.text.text = string.Format(this.format, value);
                    this.value = value;
                }
            }
        }
    }
}