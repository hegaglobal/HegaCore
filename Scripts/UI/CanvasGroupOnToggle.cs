using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using DG.Tweening;

namespace HegaCore.UI
{
    [RequireComponent(typeof(Toggle))]
    public class CanvasGroupOnToggle : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup[] canvasGroups = new CanvasGroup[0];

        [SerializeField, BoxGroup("Alpha"), LabelText("Off"), Range(0f, 1f)]
        private float offAlpha = 1f;

        [SerializeField, BoxGroup("Alpha"), LabelText("On"), Range(0f, 1f)]
        private float onAlpha = 1f;

        [SerializeField, BoxGroup("Tween")]
        private bool tween = false;

        [ShowIf(nameof(tween))]
        [SerializeField, BoxGroup("Tween"), LabelText("Duration")]
        private float tweenDuration = 0f;

        [ShowIf(nameof(tween))]
        [SerializeField, BoxGroup("Tween"), LabelText("Ease")]
        private Ease tweenEase = Ease.Linear;

        private Toggle toggle;
        private float alpha;
        private Tweener tweener;

        private void Awake()
        {
            this.toggle = GetComponent<Toggle>();
            SetValue(GetAlpha(this.toggle.isOn));

            this.toggle.onValueChanged.AddListener(OnToggleChanged);
        }

        private float GetAlpha(bool value)
            => value ? this.onAlpha : this.offAlpha;

        private void OnToggleChanged(bool value)
            => SetAlpha(GetAlpha(value));

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