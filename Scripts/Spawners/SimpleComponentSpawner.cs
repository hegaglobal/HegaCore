using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace HegaCore
{
    public abstract class SimpleComponentSpawner<T> : MonoBehaviour, IPool<T>, IAsyncPool<T>, IInstantiator<T>, IReturnInactiveItem
        where T : Component
    {
        [SerializeField]
        private Transform root = null;

        [SerializeField]
        private bool useAddressables = false;

        [SerializeField, HideIf(nameof(useAddressables))]
        private T prefab = null;

        [SerializeField, ShowIf(nameof(useAddressables))]
        private AssetReferenceGameObject prefabReference = null;

        private readonly AsyncComponentInstantiator<T> asyncInstantiator;
        private readonly ComponentPool<T> asyncPool;
        private readonly ComponentPool<T> pool;

        public SimpleComponentSpawner()
        {
            this.asyncInstantiator = new AsyncComponentInstantiator<T>();
            this.asyncPool = new ComponentPool<T>(this.asyncInstantiator);
            this.pool = new ComponentPool<T>(this);
        }

        private void ValidateDirect(string substitute)
        {
            if (this.useAddressables)
                throw new InvalidOperationException($"The spawner has been set up to get prefab by Addressables API. Use {substitute} method instead.");
        }

        private void ValidateAsync(string substitute)
        {
            if (!this.useAddressables)
                throw new InvalidOperationException($"The spawner has been set up to get prefab by direct reference. Use {substitute} method instead.");
        }

        public void Prepool(int amount)
        {
            ValidateDirect(nameof(PrepoolAsync));
            this.pool.Prepool(amount);
        }

        public async UniTask PrepoolAsync(int amount)
        {
            ValidateAsync(nameof(Prepool));
            this.asyncInstantiator.Initialize(this.root ? this.root : this.transform, this.prefabReference);
            await this.asyncPool.PrepoolAsync(amount);
        }

        public T Get(string key = null)
        {
            ValidateDirect(nameof(GetAsync));
            return this.pool.Get();
        }

        public async UniTask<T> GetAsync(string key = null)
        {
            ValidateAsync(nameof(Get));
            return await this.asyncPool.GetAsync();
        }

        public void ReturnInactiveItems()
        {
            if (this.useAddressables)
                ReturnInactiveItems(this.asyncPool.ActiveItems, this.asyncPool);
            else
                ReturnInactiveItems(this.pool.ActiveItems, this.pool);
        }

        private void ReturnInactiveItems(in ReadList<T> items, IReturnOnlyPool<T> pool)
        {
            var cache = ListPool<T>.Get();

            for (var i = 0; i < items.Count; i++)
            {
                if (items[i] && items[i].gameObject && !items[i].gameObject.activeSelf)
                    cache.Add(items[i]);
            }

            pool.Return(cache);
            cache.Clear();

            ListPool<T>.Return(cache);
        }

        public void ReturnAll()
        {
            if (this.useAddressables)
                this.asyncPool.ReturnAll();
            else
                this.pool.ReturnAll();
        }

        public void Return(T item)
        {
            if (this.useAddressables)
                this.asyncPool.Return(item);
            else
                this.pool.Return(item);
        }

        public void Return(params T[] items)
        {
            if (this.useAddressables)
                this.asyncPool.Return(items);
            else
                this.pool.Return(items);
        }

        public void Return(IEnumerable<T> items)
        {
            if (this.useAddressables)
                this.asyncPool.Return(items);
            else
                this.pool.Return(items);
        }

        T IInstantiator<T>.Instantiate()
            => Instantiate(this.prefab, Vector3.zero, Quaternion.identity, this.root);
    }
}