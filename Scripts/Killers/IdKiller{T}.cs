using System.Collections.Generic;
using System.Collections.Pooling;

namespace HegaCore
{
    public class IdKiller<TId, TItem>
        where TId : unmanaged
        where TItem : IAlive, ISetActive
    {
        private readonly Dictionary<string, Dictionary<TId, HashSet<TItem>>> mapItems = new Dictionary<string, Dictionary<TId, HashSet<TItem>>>();
        private readonly Dictionary<string, Dictionary<TId, List<TItem>>> mapCache = new Dictionary<string, Dictionary<TId, List<TItem>>>();

        public void Get(ICollection<TItem> items)
        {
            if (items == null)
                return;

            var keys = Pool.Provider.List<string>();
            keys.AddRange(this.mapItems.Keys);

            var ids = Pool.Provider.List<TId>();

            foreach (var key in keys)
            {
                if (!this.mapItems.TryGetValue(key, out var map))
                    continue;

                ids.Clear();
                ids.AddRange(map.Keys);

                foreach (var id in ids)
                {
                    if (map.TryGetValue(id, out var set))
                        items.AddRange(set);
                }
            }

            Pool.Provider.Return(ids);
            Pool.Provider.Return(keys);
        }

        public void Get(in TId id, ICollection<TItem> items)
        {
            if (items == null)
                return;

            var keys = Pool.Provider.List<string>();
            keys.AddRange(this.mapItems.Keys);

            foreach (var key in keys)
            {
                if (!this.mapItems.TryGetValue(key, out var map))
                    continue;

                if (map.TryGetValue(id, out var set))
                    items.AddRange(set);
            }

            Pool.Provider.Return(keys);
        }

        public void Get(string key, ICollection<TItem> items)
        {
            if (items == null)
                return;

            if (!this.mapItems.TryGetValue(key, out var map))
                return;

            var ids = Pool.Provider.List<TId>();

            ids.Clear();
            ids.AddRange(map.Keys);

            foreach (var id in ids)
            {
                if (map.TryGetValue(id, out var set))
                    items.AddRange(set);
            }

            Pool.Provider.Return(ids);
        }

        public void Get(string key, in TId id, ICollection<TItem> items)
        {
            if (items == null)
                return;

            if (!this.mapItems.TryGetValue(key, out var map))
                return;

            if (map.TryGetValue(id, out var set))
                items.AddRange(set);
        }

        public void Clear()
        {
            var keys = Pool.Provider.List<string>();
            keys.AddRange(this.mapItems.Keys);

            var ids = Pool.Provider.List<TId>();

            foreach (var key in keys)
            {
                if (!this.mapItems.TryGetValue(key, out var map))
                    continue;

                ids.Clear();
                ids.AddRange(map.Keys);

                foreach (var id in ids)
                {
                    if (map.TryGetValue(id, out var set))
                        Pool.Provider.Return(set);
                }

                Pool.Provider.Return(map);
            }

            this.mapItems.Clear();

            keys.Clear();
            keys.AddRange(this.mapCache.Keys);

            foreach (var key in keys)
            {
                if (!this.mapCache.TryGetValue(key, out var map))
                    continue;

                ids.Clear();
                ids.AddRange(map.Keys);

                foreach (var id in ids)
                {
                    if (map.TryGetValue(id, out var list))
                        Pool.Provider.Return(list);
                }

                Pool.Provider.Return(map);
            }

            this.mapCache.Clear();

            Pool.Provider.Return(ids);
            Pool.Provider.Return(keys);
        }

        public void Add(string key, in TId id, TItem item)
        {
            if (!this.mapItems.TryGetValue(key, out var map))
            {
                map = Pool.Provider.Dictionary<TId, HashSet<TItem>>();
                this.mapItems.Add(key, map);
            }

            if (!map.TryGetValue(id, out var set))
            {
                set = Pool.Provider.HashSet<TItem>();
                map.Add(id, set);
            }

            if (!set.Contains(item))
                set.Add(item);
        }

        public bool Contains(string key, in TId id)
            => this.mapItems.TryGetValue(key, out var map) &&
               map.ContainsKey(id);

        public bool Contains(string key, in TId id, TItem item)
            => this.mapItems.TryGetValue(key, out var map) &&
               map.TryGetValue(id, out var set) &&
               set.Contains(item);

        public void Kill(string key, in TId id)
        {
            if (!this.mapItems.TryGetValue(key, out var map) ||
                !map.TryGetValue(id, out var set))
                return;

            foreach (var item in set)
            {
                item.SetActive(false);
            }
        }

        public void Kill(string key)
        {
            if (!this.mapItems.TryGetValue(key, out var map))
                return;

            foreach (var set in map.Values)
            {
                foreach (var item in set)
                {
                    item.SetActive(false);
                }
            }
        }

        public void Kill(in TId id)
        {
            foreach (var map in this.mapItems.Values)
            {
                if (!map.TryGetValue(id, out var set))
                    continue;

                foreach (var item in set)
                {
                    item.SetActive(false);
                }
            }
        }

        public void TryKill()
        {
            foreach (var (key, cache) in this.mapCache)
            {
                if (this.mapItems.TryGetValue(key, out var mapItem))
                {
                    foreach (var (id, list) in cache)
                    {
                        if (mapItem.TryGetValue(id, out var set))
                        {
                            for (var i = 0; i < cache.Count; i++)
                            {
                                list[i].SetActive(false);
                                set.Remove(list[i]);
                            }
                        }
                        else
                        {
                            for (var i = 0; i < cache.Count; i++)
                            {
                                list[i].SetActive(false);
                            }
                        }

                        Pool.Provider.Return(list);
                    }
                }
                else
                {
                    foreach (var list in cache.Values)
                    {
                        for (var i = 0; i < cache.Count; i++)
                        {
                            list[i].SetActive(false);
                        }

                        Pool.Provider.Return(list);
                    }
                }

                Pool.Provider.Return(cache);
            }

            this.mapCache.Clear();

            var keys = Pool.Provider.List<string>();
            keys.AddRange(this.mapItems.Keys);

            var ids = Pool.Provider.List<TId>();

            foreach (var key in keys)
            {
                if (!this.mapItems.TryGetValue(key, out var mapItem))
                    continue;

                if (!this.mapCache.TryGetValue(key, out var cache))
                {
                    cache = Pool.Provider.Dictionary<TId, List<TItem>>();
                    this.mapCache.Add(key, cache);
                }

                ids.Clear();
                ids.AddRange(mapItem.Keys);

                foreach (var id in ids)
                {
                    if (!mapItem.TryGetValue(id, out var set) ||
                        set.Count <= 0)
                        continue;

                    if (!cache.TryGetValue(id, out var list))
                    {
                        list = Pool.Provider.List<TItem>();
                        cache.Add(id, list);
                    }

                    foreach (var item in set)
                    {
                        if (!item.Alive)
                            list.Add(item);
                    }
                }
            }

            Pool.Provider.Return(ids);
            Pool.Provider.Return(keys);
        }

        public void TryKill(string key)
        {
            if (this.mapCache.TryGetValue(key, out var cache))
            {
                if (this.mapItems.TryGetValue(key, out var mapItem))
                {
                    foreach (var (id, list) in cache)
                    {
                        if (mapItem.TryGetValue(id, out var set))
                        {
                            for (var i = 0; i < cache.Count; i++)
                            {
                                list[i].SetActive(false);
                                set.Remove(list[i]);
                            }
                        }
                        else
                        {
                            for (var i = 0; i < cache.Count; i++)
                            {
                                list[i].SetActive(false);
                            }
                        }

                        Pool.Provider.Return(list);
                    }
                }
                else
                {
                    foreach (var list in cache.Values)
                    {
                        for (var i = 0; i < cache.Count; i++)
                        {
                            list[i].SetActive(false);
                        }

                        Pool.Provider.Return(list);
                    }
                }

                Pool.Provider.Return(cache);
                this.mapCache.Remove(key);
            }

            var ids = Pool.Provider.List<TId>();
            {
                if (this.mapItems.TryGetValue(key, out var mapItem))
                {
                    if (!this.mapCache.TryGetValue(key, out cache))
                    {
                        cache = Pool.Provider.Dictionary<TId, List<TItem>>();
                        this.mapCache.Add(key, cache);
                    }

                    ids.Clear();
                    ids.AddRange(mapItem.Keys);

                    foreach (var id in ids)
                    {
                        if (!mapItem.TryGetValue(id, out var set) ||
                            set.Count <= 0)
                            continue;

                        if (!cache.TryGetValue(id, out var list))
                        {
                            list = Pool.Provider.List<TItem>();
                            cache.Add(id, list);
                        }

                        foreach (var item in set)
                        {
                            if (!item.Alive)
                                list.Add(item);
                        }
                    }
                }
            }
            Pool.Provider.Return(ids);
        }

        public void TryKill(in TId id)
        {
            foreach (var (key, cache) in this.mapCache)
            {
                if (this.mapItems.TryGetValue(key, out var mapItem))
                {
                    if (cache.TryGetValue(id, out var list))
                    {
                        if (mapItem.TryGetValue(id, out var set))
                        {
                            for (var i = 0; i < cache.Count; i++)
                            {
                                list[i].SetActive(false);
                                set.Remove(list[i]);
                            }
                        }
                        else
                        {
                            for (var i = 0; i < cache.Count; i++)
                            {
                                list[i].SetActive(false);
                            }
                        }

                        Pool.Provider.Return(list);
                    }
                }
                else if (cache.TryGetValue(id, out var list))
                {
                    for (var i = 0; i < cache.Count; i++)
                    {
                        list[i].SetActive(false);
                    }

                    Pool.Provider.Return(list);
                }

                Pool.Provider.Return(cache);
            }

            this.mapCache.Clear();

            var keys = Pool.Provider.List<string>();
            keys.AddRange(this.mapItems.Keys);

            foreach (var key in keys)
            {
                if (!this.mapItems.TryGetValue(key, out var mapItem))
                    continue;

                if (!this.mapCache.TryGetValue(key, out var cache))
                {
                    cache = Pool.Provider.Dictionary<TId, List<TItem>>();
                    this.mapCache.Add(key, cache);
                }

                if (!mapItem.TryGetValue(id, out var set) ||
                    set.Count <= 0)
                    continue;

                if (!cache.TryGetValue(id, out var list))
                {
                    list = Pool.Provider.List<TItem>();
                    cache.Add(id, list);
                }

                foreach (var item in set)
                {
                    if (!item.Alive)
                        list.Add(item);
                }
            }

            Pool.Provider.Return(keys);
        }

        public void TryKill(string key, in TId id)
        {
            if (this.mapCache.TryGetValue(key, out var cache))
            {
                if (this.mapItems.TryGetValue(key, out var mapItem))
                {
                    if (cache.TryGetValue(id, out var list))
                    {
                        if (mapItem.TryGetValue(id, out var set))
                        {
                            for (var i = 0; i < cache.Count; i++)
                            {
                                list[i].SetActive(false);
                                set.Remove(list[i]);
                            }
                        }
                        else
                        {
                            for (var i = 0; i < cache.Count; i++)
                            {
                                list[i].SetActive(false);
                            }
                        }

                        Pool.Provider.Return(list);
                    }
                }
                else if (cache.TryGetValue(id, out var list))
                {
                    for (var i = 0; i < cache.Count; i++)
                    {
                        list[i].SetActive(false);
                    }

                    Pool.Provider.Return(list);
                }

                Pool.Provider.Return(cache);
            }

            this.mapCache.Clear();

            if (!this.mapItems.TryGetValue(key, out var map) ||
                !map.TryGetValue(id, out var setItem) ||
                setItem.Count <= 0)
                return;

            if (!this.mapCache.TryGetValue(key, out cache))
            {
                cache = Pool.Provider.Dictionary<TId, List<TItem>>();
                this.mapCache.Add(key, cache);
            }

            if (!cache.TryGetValue(id, out var listItem))
            {
                listItem = Pool.Provider.List<TItem>();
                cache.Add(id, listItem);
            }

            foreach (var item in setItem)
            {
                if (!item.Alive)
                    listItem.Add(item);
            }
        }
    }
}