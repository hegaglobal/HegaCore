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
        private SortingLayerId sortingLayer = default;

        [SerializeField]
        private int sortingOrder = 0;

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

        private float alpha = 0f;
        private Tweener fadeTween;

        protected virtual void Awake()
        {
            FindSortingGroup();
            FindAllRenderers();
            FindAllSpriteRenderers();
            ResetLayer();
        }

        private void FindSortingGroup()
            => this.sortingGroup = GetComponent<SortingGroup>();

        private void FindAllRenderers()
            => this.renderers = GetComponentsInChildren<Renderer>();

        private void FindAllSpriteRenderers()
            => this.spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        public void SetDefaultLayer(in SortingLayerId value)
            => this.sortingLayer = value;

        public void SetDefaultSortingOrder(int value)
            => this.sortingOrder = value;

        public void ResetLayer()
        {
            SetLayer(this.sortingLayer);
            SetSortingOrder(this.sortingOrder);
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

        protected virtual void OnSetLayer(in SortingLayerId value) { }

        protected virtual void OnSetSortingOrder(int value) { }

        public float GetAlpha()
            => this.alpha;

        public void SetAlpha(float value)
        {
            value = Mathf.Clamp(value, 0f, 1f);
            this.alpha = value;

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

        public void FadeIn(float duration = 0.25f, TweenCallback onComplete = null)
        {
            this.fadeTween?.Kill();
            this.fadeTween = DOTween.To(GetAlpha, SetAlpha, 1f, duration)
                                    .SetEase(Ease.Linear)
                                    .OnComplete(onComplete ?? OnFadeInComplete);
        }

        public void FadeOut(float duration = 0.25f, TweenCallback onComplete = null)
        {
            this.fadeTween?.Kill();
            this.fadeTween = DOTween.To(GetAlpha, SetAlpha, 0f, duration)
                                    .SetEase(Ease.Linear)
                                    .OnComplete(onComplete ?? OnFadeOutComplete);
        }

        private void OnFadeInComplete()
        {
        }

        private void OnFadeOutComplete()
        {
        }

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
