using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;

namespace HegaCore
{
    public sealed class KeyedAddressableTextEmitter : ComponentSpawner<TextModule>, ITextEmitter, ITextSpawner
    {
        [SerializeField]
        private List<TextItem> items = new List<TextItem>();

        private readonly Dictionary<string, TextEmission> emissionMap;
        private readonly TextEmission defaultEmission;

        public KeyedAddressableTextEmitter()
        {
            this.emissionMap = new Dictionary<string, TextEmission>();
            this.defaultEmission = new TextEmission();
            this.defaultEmission.Initialize(this, null);
        }

        public async UniTask PrepoolAsync(bool silent = false)
        {
            foreach (var item in this.items)
            {
                if (item == null || !item.Validate() || ContainsKey(item.Key))
                    continue;

                var result = await AddressablesManager.LoadAssetAsync(item.Reference);

                RegisterPoolItem(item.Key, result.Value, item.PrepoolAmount);
            }

            Initialize(silent);
        }

        public async UniTask PrepoolAsync(Segment<string> keys, int prepoolAmount, bool silent = false)
        {
            foreach (var key in keys)
            {
                if (string.IsNullOrEmpty(key) || ContainsKey(key))
                    continue;

                var result = await AddressablesManager.LoadAssetAsync<GameObject>(key);

                RegisterPoolItem(key, result.Value, prepoolAmount);
            }

            Initialize(silent);
        }

        public TextEmission GetEmission(string key = null)
        {
            if (key != null && this.emissionMap.TryGetValue(key, out var emission))
                return emission;

            return this.defaultEmission;
        }

        UniTask<TextModule> ITextSpawner.GetTextAsync(string key)
        {
            TextModule text = null;

            if (key != null)
                text = Get(key);

            return UniTask.FromResult(text);
        }

        [Serializable]
        public class TextItem
        {
            [SerializeField]
            private string key = string.Empty;

            public AssetReferenceGameObject Reference;

            [SerializeField, Min(0)]
            private int prepoolAmount = 0;

            public string Key
            {
                get => this.key;
                set => this.key = value ?? string.Empty;
            }

            public int PrepoolAmount
            {
                get => this.prepoolAmount;
                set => this.prepoolAmount = Mathf.Max(value, 0);
            }

            public bool Validate()
                => !string.IsNullOrEmpty(this.key) && this.Reference != null;
        }
    }
}