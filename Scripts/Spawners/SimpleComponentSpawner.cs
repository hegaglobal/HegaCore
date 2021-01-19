using System.Collections.Generic;
using System.Collections.Pooling;
using UnityEngine;
using UnityEngine.Pooling;

namespace HegaCore
{
    public abstract class SimpleComponentSpawner<T> : MonoBehaviour, IPool<T>, IInstantiator<T>, IReturnInactive
        where T : Component
    {
        [SerializeField]
        private Transform poolRoot = null;

        [SerializeField]
        private T prefab = null;

        [SerializeField, Min(0)]
        private int prepoolAmount = 0;

        [SerializeField]
        private bool initializeOnAwake = true;

        private readonly ComponentPool<T> pool;

        public SimpleComponentSpawner()
        {
            this.pool = new ComponentPool<T>(this);
        }

        protected virtual void Awake()
        {
            if (this.initializeOnAwake)
                Initialize();
        }

        protected Transform GetPoolRoot()
            => this.poolRoot ? this.poolRoot : this.transform;

        public void Initialize(int prepoolAmount)
        {
            this.prepoolAmount = prepoolAmount;
            Initialize();
        }

        public void Initialize()
        {
            this.pool.Prepool(this.prepoolAmount);
            OnInitialize();
        }

        protected virtual void OnInitialize() { }

        public void DestroyAll()
            => this.pool.DestroyAll();

        public void RegisterPoolItem(T prefab, int prepoolAmount)
        {
            this.prefab = prefab;
            this.prepoolAmount = Mathf.Max(prepoolAmount, 0);
        }

        public T Get()
            => this.pool.Get();

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

        T IInstantiator<T>.Instantiate()
            => Instantiate(this.prefab, Vector3.zero, Quaternion.identity, GetPoolRoot());
    }
}