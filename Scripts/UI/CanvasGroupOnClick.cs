using UnityEngine;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;
using DG.Tweening;

namespace HegaCore.UI
{
    public class CanvasGroupOnClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField]
        private CanvasGroup[] canvasGroups = new CanvasGroup[0];

        [SerializeField, BoxGroup("Alpha"), LabelText("Default"), Range(0f, 1f)]
        private float defaultAlpha = 1f;

        [SerializeField, BoxGroup("Alpha"), LabelText("Click"), Range(0f, 1f)]
        private float clickAlpha = 1f;

        [SerializeField, BoxGroup("Tween")]
        private bool tween = false;

        [ShowIf(nameof(tween))]
        [SerializeField, BoxGroup("Tween"), LabelText("Duration")]
        private float tweenDuration = 0f;

        [ShowIf(nameof(tween))]
        [SerializeField, BoxGroup("Tween"), LabelText("Ease")]
        private Ease tweenEase = Ease.Linear;

        private float alpha;
        private Tweener tweener;

        private void Awake()
        {
            SetValue(this.defaultAlpha);
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
            => SetAlpha(this.defaultAlpha);

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
            => SetAlpha(this.clickAlpha);

        private void SetAlpha(float value)
        {
            if (!this.tween)
            {
                SetValue(value);
                return;
            }

            this.tweener?.Kill();
            this.tweener = DOTween.To(GetValue, SetValue, value, this.tweenDuration).SetEase(this.tweenEase);
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
    }
}