using System.Collections.Generic;
using System.Collections.Pooling;
using UnityEngine;

namespace HegaCore
{
    public class IdComponentKiller<TId, TComponent>
        where TId : unmanaged
        where TComponent : Component, IAlive
    {
        private readonly Dictionary<TId, TComponent> items = new Dictionary<TId, TComponent>();
        private readonly HashSet<TId> cache = new HashSet<TId>();

        public ReadDictionary<TId, TComponent> Items => this.items;

        public void Add(in TId id, TComponent item)
        {
            if (Contains(id))
                return;

            this.items[id] = item;
        }

        public bool TryGet(in TId id, out TComponent component)
            => this.items.TryGetValue(id, out component);

        public bool Contains(in TId id)
            => this.items.ContainsKey(id);

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

            var keys = Pool.Provider.List<TId>();
            keys.AddRange(this.items.Keys);

            foreach (var key in keys)
            {
                if (!this.items.TryGetValue(key, out var item))
                    continue;

                if (!item.Alive)
                    this.cache.Add(key);
            }

            Pool.Provider.Return(keys);
        }
    }
}