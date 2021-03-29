using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using DG.Tweening;

namespace HegaCore.UI
{
    public class GraphicOnClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField]
        private Graphic[] graphics = new Graphic[0];

        [SerializeField, BoxGroup("Colors"), LabelText("Default")]
        private Color defaultColor = Color.white;

        [SerializeField, BoxGroup("Colors"), LabelText("Click")]
        private Color clickColor = Color.white;

        [SerializeField, BoxGroup("Tween")]
        private bool tween = false;

        [ShowIf(nameof(tween))]
        [SerializeField, BoxGroup("Tween"), LabelText("Duration")]
        private float tweenDuration = 0f;

        [ShowIf(nameof(tween))]
        [SerializeField, BoxGroup("Tween"), LabelText("Ease")]
        private Ease tweenEase = Ease.Linear;

        private Color color;
        private Tweener tweener;

        private void Awake()
        {
            SetValue(this.defaultColor);
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
            => SetColor(this.defaultColor);

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
            => SetColor(this.clickColor);

        private void SetColor(in Color value)
        {
            if (!this.tween)
            {
                SetValue(value);
                return;
            }

            this.tweener?.Kill();
            this.tweener = DOTween.To(GetValue, SetValue, value, this.tweenDuration).SetEase(this.tweenEase);
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
    }
}