using System;
using System.Collections.Generic;
using System.Collections.Pooling;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pooling;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace HegaCore
{
    public abstract class SimpleAddressableComponentSpawner<T> : MonoBehaviour, IAsyncPool<T>, IReturnInactive
        where T : Component
    {
        [SerializeField]
        private Transform poolRoot = null;

        [SerializeField]
        private bool useAssetReference = true;

        [SerializeField, ShowIf(nameof(useAssetReference))]
        private AssetReferenceGameObject prefabReference = null;

        [SerializeField, HideIf(nameof(useAssetReference))]
        private string prefabKey = string.Empty;

        [SerializeField, Min(0)]
        private int prepoolAmount = 0;

        [SerializeField]
        private bool initializeOnAwake = true;

        private readonly ComponentAsyncInstantiator<T> instantiator;
        private readonly AsyncComponentPool<T> pool;

        public SimpleAddressableComponentSpawner()
        {
            this.instantiator = new ComponentAsyncInstantiator<T>();
            this.pool = new AsyncComponentPool<T>(this.instantiator);
        }

        protected virtual async void Awake()
        {
            if (this.initializeOnAwake)
                await InitializeAsync();
        }

        private Transform GetPoolRoot()
            => this.poolRoot ? this.poolRoot : this.transform;

        public async UniTask InitializeAsync(int prepoolAmount)
        {
            this.prepoolAmount = prepoolAmount;
            await InitializeAsync();
        }

        public async UniTask InitializeAsync()
        {
            this.instantiator.Initialize(GetPoolRoot(), this.useAssetReference, this.prefabReference, this.prefabKey);
            await this.pool.PrepoolAsync(this.prepoolAmount);

            OnInitialize();
        }

        public void Deinitialize()
        {
            this.pool.DestroyAll();
            OnDeinitialize();
        }

        protected virtual void OnInitialize() { }

        protected virtual void OnDeinitialize() { }

        public void RegisterPoolItem(AssetReferenceGameObject prefabReference, int prepoolAmount)
        {
            this.useAssetReference = true;
            this.prefabReference = prefabReference ?? throw new ArgumentNullException(nameof(prefabReference));
            this.prefabKey = string.Empty;
            this.prepoolAmount = Mathf.Max(prepoolAmount, 0);
        }

        public void RegisterPoolItem(string prefabKey, int prepoolAmount)
        {
            this.useAssetReference = false;
            this.prefabKey = prefabKey ?? throw new ArgumentNullException(nameof(prefabKey));
            this.prefabReference = null;
            this.prepoolAmount = Mathf.Max(prepoolAmount, 0);
        }

        public async UniTask<T> GetAsync()
            => await this.pool.GetAsync();

        public void ReturnInactive()
            => this.pool.ReturnInactive();

        public void ReturnAll()
            => this.pool.ReturnAll();

        public void Return(T item)
            => this.pool.Return(item);

        public void Return(params T[] items)
            => this.pool.Return(items);

        public void Return(IEnumerable<T> items)
            => this.pool.Return(items);
    }
}