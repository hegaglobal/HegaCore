using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using DG.Tweening;

namespace HegaCore
{
    public class SpriteRendererOnShow : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer[] renderers = new SpriteRenderer[0];

        [TitleGroup("Colors")]
        [SerializeField, LabelText("Default")]
        private Color defaultColor = Color.white;

        [SerializeField, LabelText("Show")]
        private Color showColor = Color.white;

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

        private Color color;
        private Tweener tweener;

        private void Awake()
        {
            SetValue(this.defaultColor);
            InvokeOnBeginDefault();
            InvokeOnCompleteDefault();
        }

        public void Default(bool instant = false)
        {
            if (instant)
            {
                SetValue(this.defaultColor);
                InvokeOnCompleteDefault();
            }
            else
                SetColor(this.defaultColor, false);
        }

        public void Show(bool instant = false)
        {
            if (instant)
            {
                SetValue(this.showColor);
                InvokeOnCompleteShow();
            }
            else
                SetColor(this.showColor, true);
        }

        private void SetColor(in Color value, bool shown)
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