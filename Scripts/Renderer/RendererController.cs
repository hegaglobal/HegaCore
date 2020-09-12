using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace HegaCore
{
    public class RendererController : MonoBehaviour
    {
        [SerializeField, InlineButton(nameof(FindSortingGroup), "Find")]
        private SortingGroup sortingGroup = null;

        [SerializeField, InlineButton(nameof(FindAllSpriteRenderers), "Find"), PropertySpace(6)]
        private SpriteRenderer[] spriteRenderers = null;

        private float alpha = 0f;
        private Tweener fadeTween;

        private void Awake()
        {
            FindSortingGroup();
            FindAllSpriteRenderers();
        }

        private void FindSortingGroup()
            => this.sortingGroup = GetComponent<SortingGroup>();

        private void FindAllSpriteRenderers()
            => this.spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        public void SetLayer(in SortingLayerId layerId)
        {
            foreach (var renderer in this.spriteRenderers)
            {
                if (renderer)
                    renderer.sortingLayerID = layerId.id;
            }

            if (this.sortingGroup)
                this.sortingGroup.sortingLayerID = layerId.id;

            OnSetLayer(layerId);
        }

        protected virtual void OnSetLayer(in SortingLayerId layerId) { }

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
    }
}
