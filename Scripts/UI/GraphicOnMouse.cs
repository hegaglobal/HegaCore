using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using DG.Tweening;

namespace HegaCore.UI
{
    public class GraphicOnMouse : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private Graphic[] graphics = new Graphic[0];

        [TitleGroup("Colors")]
        [SerializeField, LabelText("Default")]
        private Color defaultColor = Color.white;

        [SerializeField, LabelText("Hover")]
        private Color hoverColor = Color.white;

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
        [SerializeField, FoldoutGroup("Events On Begin/Events"), LabelText("Default")]
        private UnityEvent onBeginDefault = new UnityEvent();

        [ShowIf(nameof(eventsOnBegin))]
        [SerializeField, FoldoutGroup("Events On Begin/Events"), LabelText("Hover")]
        private UnityEvent onBeginHover = new UnityEvent();

        [ShowIf(nameof(eventsOnBegin))]
        [SerializeField, FoldoutGroup("Events On Begin/Events"), LabelText("Click")]
        private UnityEvent onBeginClick = new UnityEvent();

        [TitleGroup("Events On Complete")]
        [SerializeField, LabelText("Enable")]
        private bool eventsOnComplete = false;

        [ShowIf(nameof(eventsOnComplete))]
        [SerializeField, FoldoutGroup("Events On Complete/Events"), LabelText("Default")]
        private UnityEvent onCompleteDefault = new UnityEvent();

        [ShowIf(nameof(eventsOnComplete))]
        [SerializeField, FoldoutGroup("Events On Complete/Events"), LabelText("Hover")]
        private UnityEvent onCompleteHover = new UnityEvent();

        [ShowIf(nameof(eventsOnComplete))]
        [SerializeField, FoldoutGroup("Events On Complete/Events"), LabelText("Click")]
        private UnityEvent onCompleteClick = new UnityEvent();

        private Color color;
        private Tweener tweener;

        private void Awake()
        {
            SetValue(this.defaultColor);
            InvokeOnCompleteDefault();
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
            => SetColor(this.clickColor, MouseType.Click);

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
            => SetColor(this.hoverColor, MouseType.Hover);

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
            => SetColor(this.hoverColor, MouseType.Hover);

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
            => SetColor(this.defaultColor, MouseType.Default);

        private void SetColor(in Color value, MouseType mouse)
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