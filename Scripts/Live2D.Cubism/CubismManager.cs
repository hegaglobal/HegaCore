using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using VisualNovelData.Data;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace HegaCore
{
    public sealed class CubismManager : SingletonBehaviour<CubismManager>
    {
        [SerializeField]
        private CubismSpawner spawner = null;

        [SerializeField]
        private float showDuration = 0f;

        [SerializeField]
        private float hideDuration = 0f;

        [SerializeField]
        private float colorDuration = 0f;

        [SerializeField]
        private SingleOrderLayer spawnLayer = default;

        [SerializeField, BoxGroup("Animation"), LabelText("ID")]
        private bool hasIdAnimation = false;

        [SerializeField, BoxGroup("Animation"), LabelText("Body")]
        private bool hasBodyAnimation = false;

        [SerializeField, BoxGroup("Animation"), LabelText("Emo")]
        private bool hasEmoAnimation = false;

        private readonly Dictionary<string, CubismController> models;
        private readonly Dictionary<string, Tweener> showTweens;
        private readonly Dictionary<string, Tweener> hideTweens;
        private readonly Dictionary<string, Tweener> colorTweens;

        private bool darkLord;

        public CubismManager()
        {
            this.models = new Dictionary<string, CubismController>();
            this.showTweens = new Dictionary<string, Tweener>();
            this.hideTweens = new Dictionary<string, Tweener>();
            this.colorTweens = new Dictionary<string, Tweener>();
        }

        public async UniTask InitializeAsync(CharacterData data, bool darkLord)
        {
            this.darkLord = darkLord;

            await this.spawner.InitializeAsync(data, darkLord);

            foreach (var character in data.Characters.Values)
            {
                var key = character?.P1;
                var actualKey = key.OrDarkLord(this.darkLord);

                if (string.IsNullOrEmpty(actualKey))
                    continue;

                if (this.models.ContainsKey(actualKey))
                {
                    UnuLogger.LogWarning($"A model with key={actualKey} has already existed");
                    continue;
                }

                UnuLogger.Log($"Initializing {character.P1}...");
                var model = this.spawner.Get(actualKey);

                if (!model)
                    continue;

                model.Initialize(key, this.hasIdAnimation, this.hasBodyAnimation, this.hasEmoAnimation);
                model.SetLayer(this.spawnLayer);
                model.SetAlpha(1f);
                this.models.Add(key, model);
            }

            HideAll(this.hideDuration);

            await UniTask.Delay(System.TimeSpan.FromSeconds(this.hideDuration));
        }

        public void HideAll(bool instant = false)
        {
            if (instant)
            {
                foreach (var model in this.models.Values)
                {
                    model.Hide();
                }

                return;
            }

            HideAll(this.hideDuration);
        }

        public void HideAll(float? duration)
        {
            foreach (var id in this.models.Keys)
            {
                Hide(id, duration);
            }
        }

        public void Hide(string modelId)
        {
            if (!this.models.TryGetValue(modelId, out var model))
                return;

            model.Hide();
        }

        public void Hide(string modelId, float? duration)
        {
            var dur = GetDuration(duration, this.hideDuration);

            KillTweens(modelId);

            if (!this.models.TryGetValue(modelId, out var model))
                return;

            this.hideTweens[modelId] = DOTween.To(model.GetAlpha, model.SetAlpha, 0f, dur)
                                              .SetEase(Ease.InOutQuad)
                                              .OnComplete(model.Hide);
        }

        public void Hide(string modelId, in Vector3 to, float? duration = null)
        {
            var dur = GetDuration(duration, this.hideDuration);

            KillTweens(modelId);

            if (!this.models.TryGetValue(modelId, out var model))
                return;

            this.hideTweens[modelId] = model.transform.DOMove(to, dur)
                                                      .SetEase(Ease.InOutQuad)
                                                      .OnComplete(model.Hide);
        }

        public CubismController Show(string modelId, in SingleOrderLayer? orderLayer = null, float? scale = null)
        {
            if (!this.models.TryGetValue(modelId, out var model))
                return null;

            if (orderLayer.HasValue)
                model.SetLayer(orderLayer.Value);

            if (scale.HasValue)
                model.SetScale(scale.Value);

            model.Show();
            return model;
        }

        public CubismController Show(string modelId, in Color color, in SingleOrderLayer? orderLayer = null, float? scale = null)
        {
            if (!this.models.TryGetValue(modelId, out var model))
                return null;

            if (orderLayer.HasValue)
                model.SetLayer(orderLayer.Value);

            if (scale.HasValue)
                model.SetScale(scale.Value);

            model.Show(color);
            return model;
        }

        public CubismController Show(string modelId, in Vector3 position, float? duration = null, in SingleOrderLayer? orderLayer = null, float? scale = null)
        {
            var dur = GetDuration(duration, this.showDuration);

            KillTweens(modelId);

            if (!this.models.TryGetValue(modelId, out var model))
                return null;

            model.Show(position, 0f);

            if (orderLayer.HasValue)
                model.SetLayer(orderLayer.Value);

            if (scale.HasValue)
                model.SetScale(scale.Value);

            this.showTweens[modelId] = DOTween.To(model.GetAlpha, model.SetAlpha, 1f, dur)
                                              .SetEase(Ease.InOutQuad).SetDelay(0.02f);
            return model;
        }

        public CubismController Show(string modelId, in Color color, in Vector3 position, float? duration = null, in SingleOrderLayer? orderLayer = null, float? scale = null)
        {
            var dur = GetDuration(duration, this.showDuration);

            KillTweens(modelId);

            if (!this.models.TryGetValue(modelId, out var model))
                return null;

            model.Show(position, color.With(a: 0f));

            if (orderLayer.HasValue)
                model.SetLayer(orderLayer.Value);

            if (scale.HasValue)
                model.SetScale(scale.Value);

            this.showTweens[modelId] = DOTween.To(model.GetAlpha, model.SetAlpha, 1f, dur)
                                              .SetEase(Ease.InOutQuad).SetDelay(0.02f);
            return model;
        }

        public CubismController Show(string modelId, in Vector3 from, in Vector3 to, float? duration = null, in SingleOrderLayer? orderLayer = null, float? scale = null)
        {
            var dur = GetDuration(duration, this.showDuration);

            KillTweens(modelId);

            if (!this.models.TryGetValue(modelId, out var model))
                return null;

            model.Show(from);

            if (orderLayer.HasValue)
                model.SetLayer(orderLayer.Value);

            if (scale.HasValue)
                model.SetScale(scale.Value);

            this.showTweens[modelId] = model.transform.DOMove(to, dur).SetEase(Ease.InOutQuad);
            return model;
        }

        public CubismController Show(string modelId, in Color color, in Vector3 from, in Vector3 to, float? duration = null, in SingleOrderLayer? orderLayer = null, float? scale = null)
        {
            var dur = GetDuration(duration, this.showDuration);

            KillTweens(modelId);

            if (!this.models.TryGetValue(modelId, out var model))
                return null;

            model.Show(from, color);

            if (orderLayer.HasValue)
                model.SetLayer(orderLayer.Value);

            if (scale.HasValue)
                model.SetScale(scale.Value);

            this.showTweens[modelId] = model.transform.DOMove(to, dur).SetEase(Ease.InOutQuad);
            return model;
        }

        public void SetColor(string modelId, in Color color)
        {
            if (!this.models.TryGetValue(modelId, out var model))
                return;

            if (model.gameObject.activeSelf)
                model.SetColor(in color);
        }

        public void SetColor(string modelId, in Color color, float? duration = null)
        {
            if (!this.models.TryGetValue(modelId, out var model))
                return;

            if (!model.gameObject.activeSelf)
                return;

            if (this.colorTweens.TryGetValue(modelId, out var colorTween))
                colorTween?.Kill();

            var dur = GetDuration(duration, this.colorDuration);
            this.colorTweens[modelId] = DOTween.To(model.GetColor, model.SetColor, color, dur)
                                               .SetEase(Ease.InOutQuad);
        }

        public void SetColor(string modelId, in Color modelColor, in Color otherColor)
        {
            foreach (var kv in this.models)
            {
                if (!kv.Value.gameObject.activeSelf)
                    continue;

                if (string.Equals(kv.Key, modelId))
                    kv.Value.SetColor(in modelColor);
                else
                    kv.Value.SetColor(in otherColor);
            }
        }

        private void KillTweens(string modelId)
        {
            if (this.hideTweens.TryGetValue(modelId, out var hideTween))
                hideTween?.Kill();

            if (this.showTweens.TryGetValue(modelId, out var showTween))
                showTween?.Kill();
        }

        private static float GetDuration(float? duration, float @default)
            => duration.HasValue && duration.Value > 0f ? duration.Value : @default;
    }
}