using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using DG.Tweening;

namespace HegaCore.UI
{
    [RequireComponent(typeof(Toggle))]
    public class GraphicOnToggle : MonoBehaviour
    {
        [SerializeField]
        private Graphic[] graphics = new Graphic[0];

        [SerializeField, BoxGroup("Colors"), LabelText("Off")]
        private Color offColor = Color.white;

        [SerializeField, BoxGroup("Colors"), LabelText("On")]
        private Color onColor = Color.white;

        [SerializeField, BoxGroup("Tween")]
        private bool tween = false;

        [ShowIf(nameof(tween))]
        [SerializeField, BoxGroup("Tween"), LabelText("Duration")]
        private float tweenDuration = 0f;

        [ShowIf(nameof(tween))]
        [SerializeField, BoxGroup("Tween"), LabelText("Ease")]
        private Ease tweenEase = Ease.Linear;

        private Toggle toggle;
        private Color color;
        private Tweener tweener;

        private void Awake()
        {
            this.toggle = GetComponent<Toggle>();
            SetValue(GetColor(this.toggle.isOn));

            this.toggle.onValueChanged.AddListener(OnToggleChanged);
        }

        private Color GetColor(bool value)
            => value ? this.onColor : this.offColor;

        private void OnToggleChanged(bool value)
            => SetColor(GetColor(value));

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