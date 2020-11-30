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
        private bool ignoreTextSize = false;

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

            float width, height;

            if (this.ignoreTextSize)
                width = height = 0f;
            else
            {
                width = this.text.preferredWidth;
                height = this.text.preferredHeight;
            }

            if (!this.tween.Enabled)
            {
                if (this.width)
                    SetWidth(CalcNewWidth(width));

                if (this.height)
                    SetHeight(CalcNewHeight(height));

                return;
            }

            this.tweenSequence?.Kill();
            this.tweenSequence = null;

            if (this.width)
            {
                EnsureTween();

                var tween = DOTween.To(GetWidth, SetWidth, CalcNewWidth(width), this.tween.Duration).SetEase(this.tween.Ease);
                this.tweenSequence.Insert(0f, tween);
            }

            if (this.height)
            {
                EnsureTween();

                var tween = DOTween.To(GetHeight, SetHeight, CalcNewHeight(height), this.tween.Duration).SetEase(this.tween.Ease);
                this.tweenSequence.Insert(0f, tween);
            }
        }

        private float CalcNewWidth(float value)
            => CalcNewSize(this.width, value);

        private float CalcNewHeight(float value)
            => CalcNewSize(this.height, value);

        private float CalcNewSize(in SizeComponent size, float value)
            => string.IsNullOrEmpty(this.textVal)
               ? size.Min + size.Empty
               : value + size.Min + size.Offset;

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
            [HorizontalGroup(PaddingLeft = 6), LabelWidth(1), LabelText(" ")]
            public bool Enable;

            [HorizontalGroup, ShowIf(nameof(Enable)), LabelWidth(25)]
            public float Min;

            [HorizontalGroup, ShowIf(nameof(Enable)), LabelWidth(40)]
            public float Offset;

            [HorizontalGroup, ShowIf(nameof(Enable)), LabelWidth(40)]
            public float Empty;

            public SizeComponent(bool enabled, float min, float empty, float offset)
            {
                this.Enable = enabled;
                this.Min = min;
                this.Empty = empty;
                this.Offset = offset;
            }

            public static implicit operator bool(in SizeComponent value)
                => value.Enable;
        }
    }
}