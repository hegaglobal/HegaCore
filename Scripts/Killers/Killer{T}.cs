using System.Collections.Pooling;
using System.Collections.Generic;

namespace HegaCore
{
    public class Killer<T> where T : IAlive, ISetActive
    {
        private readonly Dictionary<string, HashSet<T>> mapItems = new Dictionary<string, HashSet<T>>();
        private readonly Dictionary<string, List<T>> mapCache = new Dictionary<string, List<T>>();

        public void Get(ICollection<T> items)
        {
            if (items == null)
                return;

            var keys = Pool.Provider.List<string>();
            keys.AddRange(this.mapItems.Keys);

            foreach (var key in keys)
            {
                if (this.mapItems.TryGetValue(key, out var set))
                    items.AddRange(set);
            }

            Pool.Provider.Return(keys);
        }

        public void Get(string key, ICollection<T> items)
        {
            if (items == null)
                return;

            if (this.mapItems.TryGetValue(key, out var set))
                items.AddRange(set);
        }

        public void Clear()
        {
            var keys = Pool.Provider.List<string>();
            keys.AddRange(this.mapItems.Keys);

            foreach (var key in keys)
            {
                if (this.mapItems.TryGetValue(key, out var set))
                    Pool.Provider.Return(set);
            }

            this.mapItems.Clear();

            keys.Clear();
            keys.AddRange(this.mapCache.Keys);

            foreach (var key in keys)
            {
                if (this.mapCache.TryGetValue(key, out var list))
                    Pool.Provider.Return(list);
            }

            this.mapCache.Clear();

            Pool.Provider.Return(keys);
        }

        public void Add(string key, T item)
        {
            if (!this.mapItems.TryGetValue(key, out var set))
            {
                set = Pool.Provider.HashSet<T>();
                this.mapItems.Add(key, set);
            }

            if (!set.Contains(item))
                set.Add(item);
        }

        public bool Contains(string key, T item)
            => this.mapItems.TryGetValue(key, out var set) &&
               set.Contains(item);

        public void Kill(string key)
        {
            if (!this.mapItems.TryGetValue(key, out var set))
                return;

            foreach (var item in set)
            {
                item.SetActive(false);
            }
        }

        public void TryKill()
        {
            foreach (var (key, list) in this.mapCache)
            {
                if (this.mapItems.TryGetValue(key, out var set))
                {
                    for (var i = 0; i < list.Count; i++)
                    {
                        list[i].SetActive(false);
                        set.Remove(list[i]);
                    }
                }
                else
                {
                    for (var i = 0; i < list.Count; i++)
                    {
                        list[i].SetActive(false);
                    }
                }

                Pool.Provider.Return(list);
            }

            this.mapCache.Clear();

            var keys = Pool.Provider.List<string>();
            keys.AddRange(this.mapItems.Keys);

            foreach (var key in keys)
            {
                if (!this.mapItems.TryGetValue(key, out var set) ||
                    set.Count <= 0)
                    continue;

                if (!this.mapCache.TryGetValue(key, out var list))
                {
                    list = Pool.Provider.List<T>();
                    this.mapCache.Add(key, list);
                }

                foreach (var item in set)
                {
                    if (!item.Alive)
                        list.Add(item);
                }
            }

            Pool.Provider.Return(keys);
        }

        public void TryKill(string key)
        {
            if (this.mapCache.TryGetValue(key, out var list))
            {
                for (var i = 0; i < list.Count; i++)
                {
                    list[i].SetActive(false);
                }

                Pool.Provider.Return(list);
                this.mapCache.Remove(key);
            }

            if (!this.mapItems.TryGetValue(key, out var set) ||
                set.Count <= 0)
                return;

            if (!this.mapCache.TryGetValue(key, out list))
            {
                list = Pool.Provider.List<T>();
                this.mapCache.Add(key, list);
            }

            foreach (var item in set)
            {
                if (!item.Alive)
                    list.Add(item);
            }
        }
    }
}