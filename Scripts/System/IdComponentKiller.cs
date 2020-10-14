using System.Collections.Generic;
using UnityEngine;

namespace HegaCore
{
    public class IdComponentKiller<TId, TComponent>
        where TId : unmanaged
        where TComponent : Component, IAlive
    {
        private readonly Dictionary<TId, TComponent> temp = new Dictionary<TId, TComponent>();
        private readonly Dictionary<TId, TComponent> items = new Dictionary<TId, TComponent>();
        private readonly List<TId> cache = new List<TId>();

        public ReadDictionary<TId, TComponent> Temp => this.temp;

        public ReadDictionary<TId, TComponent> Items => this.items;

        public void Add(in TId id, TComponent item)
        {
            if (Contains(id))
                return;

            this.temp[id] = item;
        }

        public bool Contains(in TId id)
            => this.temp.ContainsKey(id) || this.items.ContainsKey(id);

        public void Kill(in TId id)
        {
            if (!Contains(id))
                return;

            this.cache.Add(id);
        }

        public void TryKill()
        {
            if (this.cache.Count > 0)
            {
                foreach (var id in this.cache)
                {
                    if (!this.items.TryGetValue(id, out var item))
                        continue;

                    item.gameObject.SetActive(false);
                    this.items.Remove(id);
                }

                this.cache.Clear();
            }

            if (this.temp.Count > 0)
            {
                foreach (var kv in this.temp)
                {
                    this.items.Add(kv.Key, kv.Value);

                }
                this.temp.Clear();
            }

            foreach (var kv in this.items)
            {
                if (!kv.Value.Alive)
                    this.cache.Add(kv.Key);
            }
        }
    }
}