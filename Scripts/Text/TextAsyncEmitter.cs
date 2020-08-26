using UnityEngine;
using Cysharp.Threading.Tasks;

namespace HegaCore
{
    public sealed partial class TextAsyncEmitter : SimpleComponentAsyncSpawner<TextModule>, ITextEmitter
    {
        [SerializeField, Min(0f)]
        private float displayDuration = 1f;

        public void Emit(object value, in Vector3 position, in Vector3 offset, in Color color, float? size = null)
            => Emit(value, position, new TextEmitterParams(offset, color, size));

        public void Emit(object value, in Vector3 position, in TextEmitterParams @params)
            => Emit(value.ToString(), position, @params);

        public void Emit(string value, in Vector3 position, in Vector3 offset, in Color color, float? size = null)
            => Emit(value, position, new TextEmitterParams(offset, color, size));

        public void Emit(string value, in Vector3 position, in TextEmitterParams @params)
        {
            if (!this)
                return;

            EmitAsync(value, position, @params).Forget();
        }

        private async UniTaskVoid EmitAsync(string value, Vector3 position, TextEmitterParams @params)
        {
            var text = await GetAsync();
            Initialize(text, value, position, @params);
            Show(text, this.displayDuration).Forget();
        }

        private void Initialize(TextModule module, string value, in Vector3 position, in TextEmitterParams @params)
        {
            if (module.Text)
            {
                module.Text.SetText(value);
                module.Text.color = @params.Color;

                if (@params.Size.Custom)
                    module.Text.fontSize = @params.Size.Value;
            }

            module.transform.position = OffsetPosition(position, @params.OffsetPosition);
            module.transform.localScale = Vector3.one;
        }

        private async UniTaskVoid Show(TextModule module, float duration)
        {
            await UniTask.Delay(System.TimeSpan.FromSeconds(duration));

            Return(module);
        }

        private Vector3 OffsetPosition(Vector3 position, in Vector3 offset)
        {
            var min = position.x - offset.x;
            var max = position.x + offset.x;
            position.x = Random.Range(min, max);

            min = position.y - offset.y;
            max = position.y + offset.y;
            position.y = Random.Range(min, max);

            min = position.z - offset.z;
            max = position.z + offset.z;
            position.z = Random.Range(min, max);

            return position;
        }
    }
}
