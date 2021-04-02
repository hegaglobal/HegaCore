using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using DG.Tweening;

namespace HegaCore
{
    public class SpriteRendererOnMouse : MonoBehaviour
    {
        [field: SerializeField, LabelText(nameof(RaycastTarget), true)]
        public bool RaycastTarget { get; set; } = true;

        [SerializeField]
        private SpriteRenderer[] renderers = new SpriteRenderer[0];

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

        private Color color;
        private Tweener tweener;

        private void Awake()
        {
            SetValue(this.defaultColor);
            InvokeOnBeginDefault();
            InvokeOnCompleteDefault();
        }

        private void OnMouseDown()
        {
            if (this.RaycastTarget)
                SetColor(this.clickColor, MouseType.Click);
        }

        private void OnMouseUp()
        {
            if (this.RaycastTarget)
                SetColor(this.hoverColor, MouseType.Hover);
        }

        private void OnMouseEnter()
        {
            if (this.RaycastTarget)
                SetColor(this.hoverColor, MouseType.Hover);
        }

        private void OnMouseExit()
        {
            if (this.RaycastTarget)
                SetColor(this.defaultColor, MouseType.Default);
        }

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

            foreach (var renderer in this.renderers)
            {
                if (renderer)
                    renderer.color = value;
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