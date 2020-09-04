using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace HegaCore
{
    public sealed class KeyedTextEmitter : ComponentSpawner<TextModule>, ITextEmitter, ITextSpawner
    {
        [SerializeField]
        private List<TextItem> items = new List<TextItem>();

        private readonly Dictionary<string, TextEmission> emissionMap;
        private readonly TextEmission defaultEmission;

        public KeyedTextEmitter()
        {
            this.emissionMap = new Dictionary<string, TextEmission>();
            this.defaultEmission = new TextEmission();
            this.defaultEmission.Initialize(this, null);
        }

        public void Prepool(bool silent = false)
        {
            foreach (var item in this.items)
            {
                if (item == null || !item.Validate() || ContainsKey(item.Key))
                    continue;

                if (!this.emissionMap.ContainsKey(item.Key))
                {
                    var emission = new TextEmission();
                    emission.Initialize(this, item.Key);
                }

                RegisterPoolItem(item.Key, item.Prefab.gameObject, item.PrepoolAmount);
            }

            Initialize(silent);
        }

        public TextEmission GetEmission(string key)
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

            public TextModule Prefab;

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
                => !string.IsNullOrEmpty(this.key) && this.Prefab && this.Prefab.gameObject;
        }
    }
}