using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using VisualNovelData.Data;
using DG.Tweening;

namespace HegaCore
{
    public sealed class Live2DManager : SingletonBehaviour<Live2DManager>
    {
        [SerializeField]
        private Live2DSpawner spawner = null;

        [SerializeField]
        private float showDuration = 0f;

        [SerializeField]
        private float hideDuration = 0f;

        private readonly Dictionary<string, Live2DController> models
            = new Dictionary<string, Live2DController>();

        private readonly Dictionary<string, Tweener> showTweens
            = new Dictionary<string, Tweener>();

        private readonly Dictionary<string, Tweener> hideTweens
            = new Dictionary<string, Tweener>();

        public async UniTask InitializeAsync(CharacterData data)
        {
            await this.spawner.InitializeAsync(data);

            foreach (var character in data.Characters.Values)
            {
                var key = character?.Model ?? string.Empty;

                if (string.IsNullOrEmpty(key))
                    continue;

                if (this.models.ContainsKey(key))
                {
                    UnuLogger.LogWarning($"A model with key={key} has already existed");
                    continue;
                }

                var model = this.spawner.Get(key);

                if (!model)
                    continue;

                await UniTask.Delay(System.TimeSpan.FromSeconds(0.05f));

                model.Hide();
                this.models.Add(key, model);
            }
        }

        public void HideAll()
        {
            foreach (var model in this.models.Values)
            {
                model.Hide();
            }
        }

        public void HideAll(float? duration)
        {
            foreach (var id in this.models.Keys)
            {
                Hide(id, duration);
            }
        }

        public void Hide(string id)
        {
            if (!this.models.TryGetValue(id, out var model))
                return;

            model.Hide();
        }

        public void Hide(string id, float? duration)
        {
            var dur = GetDuration(duration, this.hideDuration);

            KillTweens(id);

            if (!this.models.TryGetValue(id, out var model))
                return;

            this.hideTweens[id] = DOTween.To(model.GetAlpha, model.SetAlpha, 0f, dur)
                                         .SetEase(Ease.InOutQuad)
                                         .OnComplete(model.Hide);
        }

        public void Hide(string id, Vector3 to, float? duration = null)
        {
            var dur = GetDuration(duration, this.hideDuration);

            KillTweens(id);

            if (!this.models.TryGetValue(id, out var model))
                return;

            this.hideTweens[id] = model.transform.DOMove(to, dur)
                                                 .SetEase(Ease.InOutQuad)
                                                 .OnComplete(model.Hide);
        }

        public Live2DController Show(string id)
        {
            if (!this.models.TryGetValue(id, out var model))
                return null;

            model.Show();
            return model;
        }

        public Live2DController Show(string id, Vector3 position, float? duration = null)
        {
            var dur = GetDuration(duration, this.showDuration);

            KillTweens(id);

            if (!this.models.TryGetValue(id, out var model))
                return null;

            model.Show(position, 0f);
            this.showTweens[id] = DOTween.To(model.GetAlpha, model.SetAlpha, 1f, dur).SetEase(Ease.InOutQuad);

            return model;
        }

        public Live2DController Show(string id, Vector3 from, Vector3 to, float? duration = null)
        {
            var dur = GetDuration(duration, this.showDuration);

            KillTweens(id);

            if (!this.models.TryGetValue(id, out var model))
                return null;

            model.Show(from);
            this.showTweens[id] = model.transform.DOMove(to, dur).SetEase(Ease.InOutQuad);

            return model;
        }

        public void PlayAnimation(string modelId, int animId)
        {
            if (!this.models.TryGetValue(modelId, out var model))
                return;

            if (model.gameObject.activeSelf)
                model.PlayAnimation(animId);
        }

        public void SetColor(string modelId, Color color)
        {
            if (!this.models.TryGetValue(modelId, out var model))
                return;

            if (model.gameObject.activeSelf)
                model.SetColor(color);
        }

        public void SetColor(string modelId, Color modelColor, Color otherColor)
        {
            foreach (var kv in this.models)
            {
                if (!kv.Value.gameObject.activeSelf)
                    continue;

                if (string.Equals(kv.Key, modelId))
                    kv.Value.SetColor(modelColor);
                else
                    kv.Value.SetColor(otherColor);
            }
        }

        private void KillTweens(string id)
        {
            if (this.hideTweens.TryGetValue(id, out var hideTween))
                hideTween?.Kill();

            if (this.showTweens.TryGetValue(id, out var showTween))
                showTween?.Kill();
        }

        private static float GetDuration(float? duration, float @default)
            => duration.HasValue && duration.Value > 0f ? duration.Value : @default;
    }
}