using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;

namespace HegaCore
{
    public sealed class GameObjectManager : Singleton<GameObjectManager>
    {
        private readonly Dictionary<string, GameObject> map
            = new Dictionary<string, GameObject>();

        public async UniTask LoadAsync(string id)
        {
            var result = await AddressablesManager.InstantiateAsync(id);
            Add(id, result.Value);
        }

        public async UniTask LoadAsync(params string[] ids)
        {
            if (ids == null)
                return;

            foreach (var id in ids)
            {
                var result = await AddressablesManager.InstantiateAsync(id);
                Add(id, result.Value);
            }
        }

        public void Add(string id, GameObject obj)
        {
            if (this.map.ContainsKey(id))
            {
                UnuLogger.LogWarning($"An object with id={id} is has already existed.");
                return;
            }

            this.map.Add(id, obj);
        }

        public GameObject Get(string id)
        {
            if (!this.map.ContainsKey(id))
            {
                UnuLogger.LogError($"Cannot find any object with id={id}.");
                return null;
            }

            return this.map[id];
        }

        public bool TryGet(string id, out GameObject obj)
            => this.map.TryGetValue(id, out obj);

        public void Release(string id)
        {
            if (!this.map.ContainsKey(id))
                return;

            this.map.Remove(id);
            AddressablesManager.ReleaseInstances(id);
        }

        public void Release(params string[] ids)
        {
            if (ids == null)
                return;

            foreach (var id in ids)
            {
                Release(id);
            }
        }
    }
}
