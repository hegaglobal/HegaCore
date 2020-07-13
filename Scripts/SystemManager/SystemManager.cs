using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;

namespace HegaCore
{
    public sealed class SystemManager : Singleton<SystemManager>
    {
        private readonly Dictionary<string, GameObject> systems
            = new Dictionary<string, GameObject>();

        public async UniTask Instantiate(string id)
        {
            var handle = AddressablesManager.InstantiateAsync(id);
            await handle;

            Add(id, handle.GetAwaiter().GetResult());
        }

        public void Add(string id, GameObject system)
        {
            if (this.systems.ContainsKey(id))
            {
                UnuLogger.LogError($"A system with id={id} is has already existed.");
                return;
            }

            this.systems.Add(id, system);
        }

        public GameObject Get(string id)
        {
            if (!this.systems.ContainsKey(id))
            {
                UnuLogger.LogError($"Cannot find any system with id={id}.");
                return null;
            }

            return this.systems[id];
        }

        public void Destroy(string id)
        {
            if (!this.systems.ContainsKey(id))
            {
                UnuLogger.LogError($"Cannot destroy any system with id={id}.");
                return;
            }

            this.systems.Remove(id);

            AddressablesManager.ReleaseInstances(id);
        }

        public void Destroy(params string[] ids)
        {
            if (ids == null)
                return;

            foreach (var id in ids)
            {
                Destroy(id);
            }
        }
    }
}
