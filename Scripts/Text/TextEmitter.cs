using UnityEngine;
using Cysharp.Threading.Tasks;
using TMPro;

namespace HegaCore
{
    public sealed class TextEmitter : MonoBehaviour, IInstantiator<TMP_Text>
    {
        [SerializeField]
        private Transform poolRoot = null;

        [SerializeField]
        private TMP_Text textPrefab = null;

        [SerializeField]
        private float displayDuration = 2f;

        private readonly ComponentPool<TMP_Text> pool;

        public TextEmitter()
        {
            this.pool = new ComponentPool<TMP_Text>(this);
        }

        public void Initialize(int prepoolAmount)
        {
            this.pool.Prepool(prepoolAmount);
        }

        public void Deinitialize()
        {
            this.pool.ReturnAll();
        }

        public void Emit(string value, Vector3 position, Color color, float? size = null)
        {
            if (!this)
                return;

            position.z = 0f;
            var text = this.pool.Get();

            text.Set(value ?? string.Empty, color, size);
            text.transform.position = position;
            text.transform.localScale = Vector3.one;

            Show(text, this.displayDuration).Forget();
        }

        private async UniTaskVoid Show(TMP_Text tmpro, float duration)
        {
            await UniTask.Delay(System.TimeSpan.FromSeconds(duration));

            this.pool.Return(tmpro);
        }

        TMP_Text IInstantiator<TMP_Text>.Instantiate()
            => Instantiate(this.textPrefab, Vector3.zero, Quaternion.identity, this.poolRoot);
    }
}
