using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;
using DG.Tweening;

namespace HegaCore.UI
{
    public class CanvasGroupOnMouse : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [field: SerializeField, LabelText(nameof(RaycastTarget), true)]
        public bool RaycastTarget { get; set; } = true;

        [SerializeField]
        private CanvasGroup[] canvasGroups = new CanvasGroup[0];

        [TitleGroup("Alpha")]
        [SerializeField, LabelText("Default"), Range(0f, 1f)]
        private float defaultAlpha = 1f;

        [SerializeField, LabelText("Hover"), Range(0f, 1f)]
        private float hoverAlpha = 1f;

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
        [SerializeField, FoldoutGroup("Events On Begin/Events", false), LabelText("Hover")]
        private UnityEvent onBeginHover = new UnityEvent();

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
        [SerializeField, FoldoutGroup("Events On Complete/Events", false), LabelText("Hover")]
        private UnityEvent onCompleteHover = new UnityEvent();

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

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            if (this.RaycastTarget)
                SetAlpha(this.clickAlpha, MouseType.Click);
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            if (this.RaycastTarget)
                SetAlpha(this.hoverAlpha, MouseType.Hover);
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (this.RaycastTarget)
                SetAlpha(this.hoverAlpha, MouseType.Hover);
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            if (this.RaycastTarget)
                SetAlpha(this.defaultAlpha, MouseType.Default);
        }

        private void SetAlpha(float value, MouseType mouse)
        {
            switch (mouse)
            {
                case MouseType.Default: InvokeOnBeginDefault(); break;
                case MouseType.Hover: InvokeOnBeginHover(); break;
                case MouseType.Click: InvokeOnBeginClick(); break;
            }

            if (!this.tween)
            {
                SetValue(value);

                switch (mouse)
                {
                    case MouseType.Default: InvokeOnCompleteDefault(); break;
                    case MouseType.Hover: InvokeOnCompleteHover(); break;
                    case MouseType.Click: InvokeOnCompleteClick(); break;
                }
                return;
            }

            this.tweener?.Kill();
            this.tweener = DOTween.To(GetValue, SetValue, value, this.tweenDuration).SetEase(this.tweenEase);

            switch (mouse)
            {
                case MouseType.Default: this.tweener.OnComplete(InvokeOnCompleteDefault); break;
                case MouseType.Hover: this.tweener.OnComplete(InvokeOnCompleteHover); break;
                case MouseType.Click: this.tweener.OnComplete(InvokeOnCompleteClick); break;
            }
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

        private void InvokeOnBeginHover()
        {
            if (this.eventsOnBegin)
                this.onBeginHover?.Invoke();
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

        private void InvokeOnCompleteHover()
        {
            if (this.eventsOnComplete)
                this.onCompleteHover?.Invoke();
        }

        private void InvokeOnCompleteClick()
        {
            if (this.eventsOnComplete)
                this.onCompleteClick?.Invoke();
        }

        private enum MouseType
        {
            Default, Hover, Click
        }
    }
}