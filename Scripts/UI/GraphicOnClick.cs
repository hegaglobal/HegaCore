using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using DG.Tweening;

namespace HegaCore.UI
{
    public class GraphicOnClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [field: SerializeField, LabelText(nameof(RaycastTarget), true)]
        public bool RaycastTarget { get; set; } = true;

        [SerializeField]
        private Graphic[] graphics = new Graphic[0];

        [TitleGroup("Colors")]
        [SerializeField, LabelText("Default")]
        private Color defaultColor = Color.white;

        [SerializeField, LabelText("Click")]
        private Color clickColor = Color.white;

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

        private Color color;
        private Tweener tweener;

        private void Awake()
        {
            SetValue(this.defaultColor);
            InvokeOnBeginDefault();
            InvokeOnCompleteDefault();
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            if (this.RaycastTarget)
                SetColor(this.defaultColor, false);
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            if (this.RaycastTarget)
                SetColor(this.clickColor, true);
        }

        private void SetColor(in Color value, bool clicked)
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