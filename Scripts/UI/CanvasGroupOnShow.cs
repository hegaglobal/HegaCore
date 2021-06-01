using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using DG.Tweening;

namespace HegaCore.UI
{
    public class CanvasGroupOnShow : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup[] canvasGroups = new CanvasGroup[0];

        [TitleGroup("Alpha")]
        [SerializeField, LabelText("Default"), Range(0f, 1f)]
        private float defaultAlpha = 1f;

        [SerializeField, LabelText("Show"), Range(0f, 1f)]
        private float showAlpha = 1f;

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
        [SerializeField, FoldoutGroup("Events On Begin/Events", false), LabelText("Default")]
        private UnityEvent onBeginDefault = new UnityEvent();

        [ShowIf(nameof(eventsOnBegin))]
        [SerializeField, FoldoutGroup("Events On Begin/Events", false), LabelText("Show")]
        private UnityEvent onBeginShow = new UnityEvent();

        [TitleGroup("Events On Complete")]
        [SerializeField, LabelText("Enable")]
        private bool eventsOnComplete = false;

        [ShowIf(nameof(eventsOnComplete))]
        [SerializeField, FoldoutGroup("Events On Complete/Events", false), LabelText("Default")]
        private UnityEvent onCompleteDefault = new UnityEvent();

        [ShowIf(nameof(eventsOnComplete))]
        [SerializeField, FoldoutGroup("Events On Complete/Events", false), LabelText("Show")]
        private UnityEvent onCompleteShow = new UnityEvent();

        private float alpha;
        private Tweener tweener;

        private void Awake()
        {
            SetValue(this.defaultAlpha);
            InvokeOnBeginDefault();
            InvokeOnCompleteDefault();
        }

        public void Default(bool instant = false)
        {
            if (instant)
            {
                SetValue(this.defaultAlpha);
                InvokeOnCompleteDefault();
            }
            else
                SetAlpha(this.defaultAlpha, false);
        }

        public void Show(bool instant = false)
        {
            if (instant)
            {
                SetValue(this.showAlpha);
                InvokeOnCompleteShow();
            }
            else
                SetAlpha(this.showAlpha, true);
        }

        private void SetAlpha(float value, bool shown)
        {
            if (shown)
                InvokeOnBeginShow();
            else
                InvokeOnBeginDefault();

            if (!this.tween)
            {
                SetValue(value);

                if (shown)
                    InvokeOnCompleteShow();
                else
                    InvokeOnCompleteDefault();

                return;
            }

            this.tweener?.Kill();
            this.tweener = DOTween.To(GetValue, SetValue, value, this.tweenDuration).SetEase(this.tweenEase);

            if (shown)
                this.tweener.OnComplete(InvokeOnCompleteShow);
            else
                this.tweener.OnComplete(InvokeOnCompleteDefault);
        }

        private float GetValue()
            => this.alpha;

        private void SetValue(float value)
        {
            this.alpha = value;

            foreach (var canvasGroup in this.canvasGroups)
            {
                if (canvasGroup)
                    canvasGroup.alpha = this.alpha;
            }
        }

        private void InvokeOnBeginDefault()
        {
            if (this.eventsOnBegin)
                this.onBeginDefault?.Invoke();
        }

        private void InvokeOnBeginShow()
        {
            if (this.eventsOnBegin)
                this.onBeginShow?.Invoke();
        }

        private void InvokeOnCompleteDefault()
        {
            if (this.eventsOnComplete)
                this.onCompleteDefault?.Invoke();
        }

        private void InvokeOnCompleteShow()
        {
            if (this.eventsOnComplete)
                this.onCompleteShow?.Invoke();
        }
    }
}