using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;

namespace HegaCore
{
    public abstract class SimpleComponentAsyncSpawner<T> : MonoBehaviour, IAsyncPool<T>, IReturnInactiveItem
        where T : Component
    {
        [SerializeField]
        private Transform root = null;

        [SerializeField]
        private AssetReferenceGameObject prefabReference = null;

        private readonly ComponentAsyncInstantiator<T> instantiator;
        private readonly AsyncComponentPool<T> pool;

        public SimpleComponentAsyncSpawner()
        {
            this.instantiator = new ComponentAsyncInstantiator<T>();
            this.pool = new AsyncComponentPool<T>(this.instantiator);
        }

        public async UniTask PrepoolAsync(int amount)
        {
            this.instantiator.Initialize(GetRoot(), this.prefabReference);
            await this.pool.PrepoolAsync(amount);
        }

        public async UniTask<T> GetAsync()
            => await this.pool.GetAsync();

        public void ReturnInactiveItems()
        {
            var objects = this.pool.ActiveObjects;
            var cache = ListPool<T>.Get();

            for (var i = 0; i < objects.Count; i++)
            {
                if (objects[i] && objects[i].gameObject && !objects[i].gameObject.activeSelf)
                    cache.Add(objects[i]);
            }

            this.pool.Return(cache);
            cache.Clear();

            ListPool<T>.Return(cache);
        }

        public void ReturnAll()
            => this.pool.ReturnAll();

        public void Return(T item)
            => this.pool.Return(item);

        public void Return(params T[] items)
            => this.pool.Return(items);

        public void Return(IEnumerable<T> items)
            => this.pool.Return(items);

        private Transform GetRoot()
            => this.root ? this.root : this.transform;
    }
}