using UnityEngine;

namespace HegaCore
{
    public sealed class TextManager : SingletonBehaviour<TextManager>, IShowText
    {
        [SerializeField]
        private TextEmitter textEmitter = null;

        public void Initialize(int prepoolAmount)
        {
            this.textEmitter.Initialize(prepoolAmount);
        }

        public void Deinitialize()
            => this.textEmitter.Deinitialize();

        public void Show(string value, Vector3 position, Color color, float? size = null)
            => this.textEmitter.Emit(value, position, color, size);
    }
}