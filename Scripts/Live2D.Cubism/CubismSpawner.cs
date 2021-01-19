using System.Collections.Generic;
using System.Collections.Pooling;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;
using VisualNovelData.Data;

namespace HegaCore
{
    public sealed class CubismSpawner : MonoBehaviour, IKeyedPool<CubismController>
    {
        [SerializeField]
        private Transform root = null;

        private readonly Dictionary<string, CubismController> map = new Dictionary<string, CubismController>();

        public async UniTask InitializeAsync(CharacterData data, bool darkLord)
        {
            foreach (var character in data.Characters.Values)
            {
                var key = character?.P1.OrDarkLord(darkLord);

                if (string.IsNullOrEmpty(key))
                    continue;

                if (!AddressablesManager.ContainsKey(key))
                {
                    UnuLogger.LogError($"Cannot find any addressable asset with key={key}");
                    continue;
                }

                if (this.map.ContainsKey(key))
                    continue;

                try
                {
                    var result = await AddressablesManager.InstantiateAsync(key, GetRoot());

                    if (!result.Succeeded)
                    {
                        UnuLogger.LogError($"Cannot instantiate asset with key={key}");
                        continue;
                    }

                    var go = result.Value;
                    var component = go.GetComponent<CubismController>();
                    this.map[key] = component;
                }
                catch (System.Exception ex)
                {
                    UnuLogger.LogException(ex, this);
                }
            }
        }

        private Transform GetRoot()
            => this.root ? this.root : this.transform;

        public CubismController Get(string key = null)
        {
            if (string.IsNullOrEmpty(key))
            {
                UnuLogger.LogWarning("Key is empty", this);
                return null;
            }

            if (!this.map.TryGetValue(key, out var item))
            {
                UnuLogger.LogWarning($"Asset with key={key} does not exist");
                return null;
            }

            if (item && !item.gameObject.activeSelf)
                item.gameObject.SetActive(true);

            return item;
        }

        public void Return(CubismController item)
        {
            if (item)
                item.gameObject.SetActive(false);
        }

        public void Return(params CubismController[] items)
        {
            foreach (var item in items)
            {
                Return(item);
            }
        }

        public void Return(IEnumerable<CubismController> items)
        {
            foreach (var item in items)
            {
                Return(item);
            }
        }

        public void ReturnAll()
        {
            foreach (var item in this.map.Values)
            {
                Return(item);
            }
        }
    }
}