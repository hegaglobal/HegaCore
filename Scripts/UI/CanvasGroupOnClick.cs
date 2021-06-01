using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;
using DG.Tweening;

namespace HegaCore.UI
{
    public class CanvasGroupOnClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [field: SerializeField, LabelText(nameof(RaycastTarget), true)]
        public bool RaycastTarget { get; set; } = true;

        [SerializeField]
        private CanvasGroup[] canvasGroups = new CanvasGroup[0];

        [TitleGroup("Alpha")]
        [SerializeField, LabelText("Default"), Range(0f, 1f)]
        private float defaultAlpha = 1f;

        [SerializeField, LabelText("Click"), Range(0f, 1f)]
        private float clickAlpha = 1f;

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
        [SerializeField, FoldoutGroup("Events On Begin/Events", false), LabelText("Click")]
        private UnityEvent onBeginClick = new UnityEvent();

        [TitleGroup("Events On Complete")]
        [SerializeField, LabelText("Enable")]
        private bool eventsOnComplete = false;

        [ShowIf(nameof(eventsOnComplete))]
        [SerializeField, FoldoutGroup("Events On Complete/Events", false), LabelText("Default")]
        private UnityEvent onCompleteDefault = new UnityEvent();

        [ShowIf(nameof(eventsOnComplete))]
        [SerializeField, FoldoutGroup("Events On Complete/Events", false), LabelText("Click")]
        private UnityEvent onCompleteClick = new UnityEvent();

        private float alpha;
        private Tweener tweener;

        private void Awake()
        {
            SetValue(this.defaultAlpha);
            InvokeOnBeginDefault();
            InvokeOnCompleteDefault();
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            if (this.RaycastTarget)
                SetAlpha(this.defaultAlpha, false);
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            if (this.RaycastTarget)
                SetAlpha(this.clickAlpha, true);
        }

        private void SetAlpha(float value, bool clicked)
        {
            if (clicked)
                InvokeOnBeginClick();
            else
                InvokeOnBeginDefault();

            if (!this.tween)
            {
                SetValue(value);

                if (clicked)
                    InvokeOnCompleteClick();
                else
                    InvokeOnCompleteDefault();

                return;
            }

            this.tweener?.Kill();
            this.tweener = DOTween.To(GetValue, SetValue, value, this.tweenDuration).SetEase(this.tweenEase);

            if (clicked)
                this.tweener.OnComplete(InvokeOnCompleteClick);
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

        private void InvokeOnBeginClick()
        {
            if (this.eventsOnBegin)
                this.onBeginClick?.Invoke();
        }

        private void InvokeOnCompleteDefault()
        {
            if (this.eventsOnComplete)
                this.onCompleteDefault?.Invoke();
        }

        private void InvokeOnCompleteClick()
        {
            if (this.eventsOnComplete)
                this.onCompleteClick?.Invoke();
        }
    }
}