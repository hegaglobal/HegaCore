using UnityEngine;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;
using VisualNovelData.Data;

namespace HegaCore
{
    public sealed class CubismSpawner : ComponentSpawner<CubismController>
    {
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

                var result = await AddressablesManager.LoadAssetAsync<GameObject>(key);

                RegisterPoolItem(key, result.Value, 1);
            }

            Initialize();
        }
    }
}