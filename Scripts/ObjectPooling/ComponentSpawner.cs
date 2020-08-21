using UnityEngine;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;

namespace HegaCore
{
    public abstract class ComponentSpawner<T> : MonoBehaviour where T : Component
    {
        [SerializeField]
        private Transform root = null;

        [SerializeField]
        private AssetReferenceGameObject prefabReference = null;

        private readonly AsyncComponentInstantiator<T> instantiator;
        private readonly ComponentPool<T> pool;

        public ComponentSpawner()
        {
            this.instantiator = new AsyncComponentInstantiator<T>();
            this.pool = new ComponentPool<T>(this.instantiator);
        }

        public async UniTask PrepareAsync(int amount)
        {
            this.instantiator.Initialize(this.root ? this.root : this.transform, this.prefabReference);
            await this.pool.PrepoolAsync(amount);
        }

        public void ReturnAll()
            => this.pool.ReturnAll();

        public T Get()
            => this.pool.Get();
    }
}