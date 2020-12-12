using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace HegaCore
{
    public sealed class TextEmission
    {
        private ITextSpawner spawner;
        private string key;

        public void Initialize(ITextSpawner spawner, string key)
        {
            this.spawner = spawner ?? throw new ArgumentNullException(nameof(spawner));
            this.key = key;
        }

        public void Emit(object value, in Vector3 position, in Vector3 offset, in Color color, float? size = null,
                         float displayDuration = 1f)
            => Emit(value, position, new TextEmitterParams(offset, color, size), displayDuration);

        public void Emit(string value, in Vector3 position, in Vector3 offset, in Color color, float? size = null,
                         float displayDuration = 1f)
            => Emit(value, position, new TextEmitterParams(offset, color, size), displayDuration);

        public void Emit(object value, in Vector3 position, in Vector3Range offset, in Color color, float? size = null,
                         float displayDuration = 1f)
            => Emit(value, position, new TextEmitterParams(offset, color, size), displayDuration);

        public void Emit(string value, in Vector3 position, in Vector3Range offset, in Color color, float? size = null,
                         float displayDuration = 1f)
            => Emit(value, position, new TextEmitterParams(offset, color, size), displayDuration);

        public void Emit(object value, in Vector3 position, in TextEmitterParams @params, float displayDuration = 1f)
            => Emit(value.ToString(), position, @params, displayDuration);

        public void Emit(string value, in Vector3 position, in TextEmitterParams @params, float displayDuration = 1f)
        {
            if (this.spawner == null)
            {
                UnuLogger.LogError($"Must call {nameof(Initialize)} before emitting");
                return;
            }

            EmitInternal(value, position, @params, displayDuration).Forget();
        }

        private async UniTaskVoid EmitInternal(string value, Vector3 position, TextEmitterParams @params, float displayDuration)
        {
            var text = await this.spawner.GetTextAsync(this.key);

            if (!text)
                return;

            Set(text, value, position, @params);
            Show(text, displayDuration).Forget();
        }

        private void Set(TextModule module, string value, in Vector3 position, in TextEmitterParams @params)
        {
            if (module.Text)
            {
                module.Text.SetText(value);
                module.Text.color = @params.Color;

                if (@params.Size.Custom)
                    module.Text.fontSize = @params.Size.Value;
            }

            module.transform.position = OffsetPosition(position, @params);
            module.transform.localScale = Vector3.one;
        }

        private async UniTaskVoid Show(TextModule module, float duration)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(duration));

            this.spawner.Return(module);
        }

        private Vector3 OffsetPosition(Vector3 position, in TextEmitterParams @params)
        {
            if (@params.RandomInRange)
            {
                var offset = @params.OffsetRange;

                var min = position.x - offset.Min.x;
                var max = position.x + offset.Max.x;
                position.x = UnityEngine.Random.Range(min, max);

                min = position.y - offset.Min.y;
                max = position.y + offset.Max.y;
                position.y = UnityEngine.Random.Range(min, max);

                min = position.z - offset.Min.z;
                max = position.z + offset.Max.z;
                position.z = UnityEngine.Random.Range(min, max);
            }
            else
            {
                position += @params.Offset;
            }

            return position;
        }
    }
}