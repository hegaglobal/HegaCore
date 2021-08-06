using System;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace HegaCore
{
    public class RendererController : MonoBehaviour
    {
        [SerializeField]
        private Color color = Color.white;

        [SerializeField]
        private SortingLayerId sortingLayer = default;

        [SerializeField]
        private int sortingOrder = 0;

        [VerticalGroup("Sorting", PaddingTop = 10), BoxGroup("Sorting/Apply On Awake")]
        [SerializeField, LabelText("Color")]
        private bool applyColorOnAwake = true;

        [BoxGroup("Sorting/Apply On Awake")]
        [SerializeField, LabelText("Sorting Order")]
        private bool applySortingLayerOnAwake = true;

        [BoxGroup("Sorting/Apply On Awake")]
        [SerializeField, LabelText("Sorting Order")]
        private bool applySortingOrderOnAwake = true;

        [PropertySpace]
        [SerializeField, InlineButton(nameof(FindSortingGroup), "Find")]
        private SortingGroup sortingGroup = null;

        [SerializeField, InlineButton(nameof(FindAllRenderers), "Find"), PropertySpace(6)]
        private Renderer[] renderers = null;

        [SerializeField, InlineButton(nameof(FindAllSpriteRenderers), "Find"), PropertySpace(6)]
        private SpriteRenderer[] spriteRenderers = null;

        public SortingLayerId SortingLayer => this.sortingLayer;

        public int SortingOrder => this.sortingOrder;

        public SortingGroup SortingGroup => this.sortingGroup;

        public ReadArray1<Renderer> Renderers => this.renderers;

        public ReadArray1<SpriteRenderer> SpriteRenderers => this.spriteRenderers;

        private Color colorBak;
        private Tweener tweenFade;
        private Tweener tweenAnimateColor;

        protected virtual void Awake()
        {
            this.colorBak = this.color;

            FindSortingGroup();
            FindAllRenderers();
            FindAllSpriteRenderers();

            if (this.applyColorOnAwake)
                ApplyDefaultColor();

            if (this.applySortingLayerOnAwake)
                ApplyDefaultSortingLayer();

            if (this.applySortingOrderOnAwake)
                ApplyDefaultSortingOrder();
        }

        private void FindSortingGroup()
            => this.sortingGroup = GetComponent<SortingGroup>();

        private void FindAllRenderers()
            => this.renderers = GetComponentsInChildren<Renderer>(true).OrEmpty();

        private void FindAllSpriteRenderers()
            => this.spriteRenderers = GetComponentsInChildren<SpriteRenderer>(true).OrEmpty();

        public void SetDefaultColor(in Color value)
            => this.color = value;

        public void SetDefaultLayer(in SortingLayerId value)
            => this.sortingLayer = value;

        public void SetDefaultSortingOrder(int value)
            => this.sortingOrder = value;

        public void ApplyDefaultColor()
            => SetColor(this.color);

        public void ApplyDefaultSortingLayerAndOrder()
        {
            SetLayer(this.sortingLayer);
            SetSortingOrder(this.sortingOrder);
        }

        public void ApplyDefaultSortingLayer()
            => SetLayer(this.sortingLayer);

        public void ApplyDefaultSortingOrder()
            => SetSortingOrder(this.sortingOrder);

        public void SetColor(Color value)
        {
            this.colorBak = value;

            foreach (var renderer in this.spriteRenderers)
            {
                if (renderer)
                    renderer.color = value;
            }

            OnSetColor(value);
        }

        public void SetLayer(in SortingLayerId value)
        {
            foreach (var renderer in this.renderers)
            {
                if (renderer)
                    renderer.sortingLayerID = value.id;
            }

            if (this.sortingGroup)
                this.sortingGroup.sortingLayerID = value.id;

            OnSetLayer(value);
        }

        public void SetSortingOrder(int value)
        {
            foreach (var renderer in this.renderers)
            {
                if (renderer)
                    renderer.sortingOrder = value;
            }

            if (this.sortingGroup)
                this.sortingGroup.sortingOrder = value;

            OnSetSortingOrder(value);
        }

        protected virtual void OnSetColor(in Color value) { }

        protected virtual void OnSetLayer(in SortingLayerId value) { }

        protected virtual void OnSetSortingOrder(int value) { }

        public Color GetColor()
            => this.colorBak;

        public float GetAlpha()
            => this.colorBak.a;

        public void SetAlpha(float value)
        {
            value = Mathf.Clamp(value, 0f, 1f);
            this.colorBak = this.colorBak.With(a: value);

            foreach (var renderer in this.spriteRenderers)
            {
                if (!renderer)
                    continue;

                var color = renderer.color.With(a: value);
                renderer.color = color;
            }

            OnSetAlpha(value);
        }

        protected virtual void OnSetAlpha(float value) { }

        public void FadeIn(float duration = 0.25f, TweenCallback onComplete = null, Ease ease = Ease.Linear)
        {
            this.tweenFade?.Kill();
            this.tweenFade = DOTween.To(GetAlpha, SetAlpha, 1f, duration)
                                    .SetEase(ease)
                                    .OnComplete(onComplete ?? OnFadeInComplete);
        }

        public void FadeOut(float duration = 0.25f, TweenCallback onComplete = null, Ease ease = Ease.Linear)
        {
            this.tweenFade?.Kill();
            this.tweenFade = DOTween.To(GetAlpha, SetAlpha, 0f, duration)
                                    .SetEase(ease)
                                    .OnComplete(onComplete ?? OnFadeOutComplete);
        }

        public void AnimateColor(in Color endValue, float duration = 0.25f, TweenCallback onComplete = null, Ease ease = Ease.Linear)
        {
            this.tweenAnimateColor?.Kill();
            this.tweenAnimateColor = DOTween.To(GetColor, SetColor, endValue, duration)
                                            .SetEase(ease)
                                            .OnComplete(onComplete ?? OnAnimateColorComplete);
        }

        private void OnFadeInComplete() { }

        private void OnFadeOutComplete() { }

        private void OnAnimateColorComplete() { }

        protected virtual void Init(in SortingLayerId layer, int order)
        {
            this.sortingLayer = layer;
            this.sortingOrder = order;

            FindSortingGroup();
            FindAllRenderers();
            FindAllSpriteRenderers();
        }
    }
}
