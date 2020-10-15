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

        public async UniTask LoadAsync(string id, Transform root = null)
        {
            try
            {
                var result = await AddressablesManager.InstantiateAsync(id);
                result.Value.name = id;

                if (root)
                    result.Value.transform.SetParent(root, false);

                Add(id, result.Value);
            }
            catch (Exception ex)
            {
                UnuLogger.LogException(ex);
            }
        }

        public async UniTask LoadAsync(params string[] ids)
        {
            if (ids == null)
                return;

            foreach (var id in ids)
            {
                try
                {
                    var result = await AddressablesManager.InstantiateAsync(id);
                    result.Value.name = id;

                    Add(id, result.Value);
                }
                catch (Exception ex)
                {
                    UnuLogger.LogException(ex);
                }
            }
        }

        public async UniTask LoadAsync(Transform root, params string[] ids)
        {
            if (ids == null)
                return;

            foreach (var id in ids)
            {
                try
                {
                    var result = await AddressablesManager.InstantiateAsync(id);
                    result.Value.name = id;

                    if (root)
                        result.Value.transform.SetParent(root, false);

                    Add(id, result.Value);
                }
                catch (Exception ex)
                {
                    UnuLogger.LogException(ex);
                }
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

            try
            {
                AddressablesManager.ReleaseInstances(id);
            }
            catch (Exception ex)
            {
                UnuLogger.LogException(ex);
            }
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
