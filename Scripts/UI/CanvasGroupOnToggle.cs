using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using DG.Tweening;

namespace HegaCore.UI
{
    [RequireComponent(typeof(Toggle))]
    public class CanvasGroupOnToggle : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup[] canvasGroups = new CanvasGroup[0];

        [TitleGroup("Colors")]
        [SerializeField, LabelText("Off"), Range(0f, 1f)]
        private float offAlpha = 1f;

        [SerializeField, LabelText("On"), Range(0f, 1f)]
        private float onAlpha = 1f;

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
        [SerializeField, FoldoutGroup("Events On Begin/Events"), LabelText("Off")]
        private UnityEvent onBeginOff = new UnityEvent();

        [ShowIf(nameof(eventsOnBegin))]
        [SerializeField, FoldoutGroup("Events On Begin/Events"), LabelText("On")]
        private UnityEvent onBeginOn = new UnityEvent();

        [TitleGroup("Events On Complete")]
        [SerializeField, LabelText("Enable")]
        private bool eventsOnComplete = false;

        [ShowIf(nameof(eventsOnComplete))]
        [SerializeField, FoldoutGroup("Events On Complete/Events"), LabelText("Off")]
        private UnityEvent onCompleteOff = new UnityEvent();

        [ShowIf(nameof(eventsOnComplete))]
        [SerializeField, FoldoutGroup("Events On Complete/Events"), LabelText("On")]
        private UnityEvent onCompleteOn = new UnityEvent();

        private Toggle toggle;
        private float alpha;
        private Tweener tweener;

        private void Awake()
        {
            this.toggle = GetComponent<Toggle>();
            SetValue(GetAlpha(this.toggle.isOn));
            InvokeOnCompleteOff();

            this.toggle.onValueChanged.AddListener(OnToggleChanged);
        }

        private float GetAlpha(bool value)
            => value ? this.onAlpha : this.offAlpha;

        private void OnToggleChanged(bool value)
            => SetAlpha(GetAlpha(value), value);

        private void SetAlpha(float value, bool isOn)
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

        private float GetValue()
            => this.alpha;

        private void SetValue(float value)
        {
            this.alpha = value;

            foreach (var canvasGroup in this.canvasGroups)
            {
                if (canvasGroup)
                    canvasGroup.alpha = alpha;
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