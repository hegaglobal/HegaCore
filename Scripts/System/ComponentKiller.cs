using System.Collections.Generic;
using UnityEngine;

namespace HegaCore
{
    public class ComponentKiller<T> where T : Component, IAlive
    {
        private readonly List<T> items = new List<T>();
        private readonly List<T> cache = new List<T>();

        public ReadList<T> Items => this.items;

        public void Add(T item)
        {
            if (Contains(item))
                return;

            this.items.Add(item);
        }

        public bool Contains(T item)
            => this.items.Contains(item);

        public void TryKill()
        {
            if (this.cache.Count > 0)
            {
                foreach (var item in this.cache)
                {
                    item.gameObject.SetActive(false);
                    this.items.Remove(item);
                }

                this.cache.Clear();
            }

            foreach (var item in this.items)
            {
                if (!item.Alive)
                    this.cache.Add(item);
            }
        }
    }
}