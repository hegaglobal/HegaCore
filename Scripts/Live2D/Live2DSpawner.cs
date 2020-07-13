using UnityEngine;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;
using VisualNovelData.Data;

namespace HegaCore
{
    public sealed class Live2DSpawner : ComponentSpawner<Live2DController>
    {
        public async UniTask InitializeAsync(CharacterData data)
        {
            foreach (var character in data.Characters.Values)
            {
                var key = character?.Model ?? string.Empty;

                if (string.IsNullOrEmpty(key))
                    continue;

                if (!AddressablesManager.ContainsKey(key))
                {
                    UnuLogger.LogError($"Cannot find any addressable asset with key={key}");
                    continue;
                }

                var handle = AddressablesManager.LoadAssetAsync<GameObject>(key);
                await handle;

                RegisterPoolItem(key, handle.GetAwaiter().GetResult(), 1);
            }

            Initialize();
        }
    }
}