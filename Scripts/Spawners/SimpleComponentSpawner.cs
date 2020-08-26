﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace HegaCore
{
    public abstract class SimpleComponentSpawner<T> : MonoBehaviour, IPool<T>, IInstantiator<T>, IReturnInactiveItem
        where T : Component
    {
        [SerializeField]
        private Transform root = null;

        [SerializeField]
        private T prefab = null;

        private readonly ComponentPool<T> pool;

        public SimpleComponentSpawner()
        {
            this.pool = new ComponentPool<T>(this);
        }

        public void Prepool(int amount)
        {
            this.pool.Prepool(amount);
        }

        public T Get()
        {
            return this.pool.Get();
        }

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

        T IInstantiator<T>.Instantiate()
            => Instantiate(this.prefab, Vector3.zero, Quaternion.identity, GetRoot());
    }
}