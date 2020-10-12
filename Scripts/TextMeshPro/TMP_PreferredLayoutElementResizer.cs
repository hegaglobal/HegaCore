using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace HegaCore
{
    [RequireComponent(typeof(LayoutElement))]
    public class TMP_PreferredLayoutElementResizer : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text text = null;

        [SerializeField]
        private bool auto = false;

        [SerializeField]
        private SizeComponent width = default;

        [SerializeField]
        private SizeComponent height = default;

        [SerializeField, BoxGroup]
        private DOTweenData tween = DOTweenData.Disable;

        private LayoutElement layoutElement;
        private string textVal;
        private Sequence tweenSequence;

        private void Awake()
        {
            this.layoutElement = GetComponent<LayoutElement>();
            this.textVal = this.text ? this.text.text : string.Empty;
        }

        private void Update()
        {
            if (this.auto)
                Resize();
        }

        public void Resize()
        {
            if (!this.text || string.Equals(this.text.text, this.textVal))
                return;

            this.textVal = this.text.text;
            var width = this.text.preferredWidth;
            var height = this.text.preferredHeight;

            if (!this.tween.Enabled)
            {
                if (this.width)
                    this.layoutElement.preferredWidth = width + this.width.Min;

                if (this.height)
                    this.layoutElement.preferredHeight = height + this.height.Min;

                return;
            }

            this.tweenSequence?.Kill();
            this.tweenSequence = null;

            if (this.width)
            {
                EnsureTween();

                var tween = DOTween.To(GetWidth, SetWidth, width + this.width.Min, this.tween.Duration).SetEase(this.tween.Ease);
                this.tweenSequence.Insert(0f, tween);
            }

            if (this.height)
            {
                EnsureTween();

                var tween = DOTween.To(GetHeight, SetHeight, height + this.height.Min, this.tween.Duration).SetEase(this.tween.Ease);
                this.tweenSequence.Insert(0f, tween);
            }
        }

        private float GetWidth()
            => this.layoutElement.preferredWidth;

        private void SetWidth(float value)
            => this.layoutElement.preferredWidth = value;

        private float GetHeight()
            => this.layoutElement.preferredHeight;

        private void SetHeight(float value)
            => this.layoutElement.preferredHeight = value;

        private void EnsureTween()
        {
            if (this.tweenSequence == null)
                this.tweenSequence = DOTween.Sequence();
        }

        [Serializable, InlineProperty]
        private struct SizeComponent
        {
            [HorizontalGroup, LabelWidth(1), LabelText(" ")]
            public bool Enable;

            [HorizontalGroup, ShowIf(nameof(Enable)), LabelWidth(25)]
            public float Min;

            public SizeComponent(bool enabled, float min)
            {
                this.Enable = enabled;
                this.Min = min;
            }

            public static implicit operator bool(in SizeComponent value)
                => value.Enable;
        }
    }
}