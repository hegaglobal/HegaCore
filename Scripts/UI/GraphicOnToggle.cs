using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using DG.Tweening;

namespace HegaCore.UI
{
    [RequireComponent(typeof(Toggle))]
    public class GraphicOnToggle : MonoBehaviour
    {
        [SerializeField]
        private Graphic[] graphics = new Graphic[0];

        [TitleGroup("Colors")]
        [SerializeField, LabelText("Off")]
        private Color offColor = Color.white;

        [SerializeField, LabelText("On")]
        private Color onColor = Color.white;

        [TitleGroup("Tween")]
        [SerializeField, LabelText("Enable")]
        private bool tween = false;

        [ShowIf(nameof(tween))]
        [SerializeField, LabelText("Duration")]
        private float tweenDuration = 0f;

        [ShowIf(nameof(tween))]
        [SerializeField, LabelText("Ease")]
        private Ease tweenEase = Ease.Linear;

        [TitleGroup("Events On Begin")]
        [SerializeField, LabelText("Enable")]
        private bool eventsOnBegin = false;

        [ShowIf(nameof(eventsOnBegin))]
        [SerializeField, FoldoutGroup("Events On Begin/Events", false), LabelText("Off")]
        private UnityEvent onBeginOff = new UnityEvent();

        [ShowIf(nameof(eventsOnBegin))]
        [SerializeField, FoldoutGroup("Events On Begin/Events", false), LabelText("On")]
        private UnityEvent onBeginOn = new UnityEvent();

        [TitleGroup("Events On Complete")]
        [SerializeField, LabelText("Enable")]
        private bool eventsOnComplete = false;

        [ShowIf(nameof(eventsOnComplete))]
        [SerializeField, FoldoutGroup("Events On Complete/Events", false), LabelText("Off")]
        private UnityEvent onCompleteOff = new UnityEvent();

        [ShowIf(nameof(eventsOnComplete))]
        [SerializeField, FoldoutGroup("Events On Complete/Events", false), LabelText("On")]
        private UnityEvent onCompleteOn = new UnityEvent();

        private Toggle toggle;
        private Color color;
        private Tweener tweener;

        private void Awake()
        {
            this.toggle = GetComponent<Toggle>();
            SetValue(GetColor(this.toggle.isOn));
            InvokeOnCompleteOff();

            this.toggle.onValueChanged.AddListener(OnToggleChanged);
        }

        private Color GetColor(bool value)
            => value ? this.onColor : this.offColor;

        private void OnToggleChanged(bool value)
            => SetColor(GetColor(value), value);

        private void SetColor(in Color value, bool isOn)
        {
            if (isOn)
                InvokeOnBeginOn();
            else
                InvokeOnBeginOff();

            if (!this.tween)
            {
                SetValue(value);

                if (isOn)
                    InvokeOnCompleteOn();
                else
                    InvokeOnCompleteOff();

                return;
            }

            this.tweener?.Kill();
            this.tweener = DOTween.To(GetValue, SetValue, value, this.tweenDuration).SetEase(this.tweenEase);

            if (isOn)
                this.tweener.OnComplete(InvokeOnCompleteOn);
            else
                this.tweener.OnComplete(InvokeOnCompleteOff);
        }

        private Color GetValue()
            => this.color;

        private void SetValue(Color value)
        {
            this.color = value;

            foreach (var graphic in this.graphics)
            {
                if (graphic)
                    graphic.color = value;
            }
        }

        private void InvokeOnBeginOff()
        {
            if (this.eventsOnBegin)
                this.onBeginOff?.Invoke();
        }

        private void InvokeOnBeginOn()
        {
            if (this.eventsOnBegin)
                this.onBeginOn?.Invoke();
        }

        private void InvokeOnCompleteOff()
        {
            if (this.eventsOnComplete)
                this.onCompleteOff?.Invoke();
        }

        private void InvokeOnCompleteOn()
        {
            if (this.eventsOnComplete)
                this.onCompleteOn?.Invoke();
        }
    }
}