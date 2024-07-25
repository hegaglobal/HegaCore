using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Sirenix.OdinInspector;
using System;
using System.Linq;
using HegaCore;

namespace SimpleLocalization
{
    public class LocalizeFontAsset : SingletonBehaviour<LocalizeFontAsset>
    {
        [SerializeField]
        private string labelKey = "Fonts";

        [SerializeField]
        private FontConfig[] fontConfig;

        [ShowInInspector]
        private Dictionary<string, TMP_FontAsset> loadedFontDictionary;
        [ShowInInspector]
        private Dictionary<string, TMP_FontAsset> fontToDictionary;
        [ShowInInspector]
        private Dictionary<string, Material> materialDictionary;

        [ShowInInspector]
        public eLanguage CurrentLanguage
        {
            get
            {
                eLanguage language = eLanguage.en;
                if (DataManager.Instance != null && DataManager.GameSettings != null)
                    Enum.TryParse(DataManager.GameSettings.Language, out language);
                return language;
            }
        }

        public TMP_FontAsset CurrentFont
        {
            get
            {
                if (loadedFontDictionary != null && loadedFontDictionary.TryGetValue(CurrentLanguage.ToString(), out var fontAsset))
                {
                    return fontAsset;
                }
                return null;
            }
        }

        private void Awake()
        {
            _ = LoadFontsAsync();
        }

        private async Task LoadFontsAsync()
        {
            var fontTask = Addressables.LoadAssetsAsync<TMP_FontAsset>(labelKey, null).Task;
            var matTask = Addressables.LoadAssetsAsync<Material>(labelKey, null).Task;

            try
            {
                await Task.WhenAll(fontTask, matTask);

                var loadedFonts = fontTask.Result;
                var loadedMaterials = matTask.Result;

                fontToDictionary = loadedFonts.ToDictionary(font => font.name, font => font);
                loadedFontDictionary = new Dictionary<string, TMP_FontAsset>();
                materialDictionary = loadedMaterials.ToDictionary(mat => mat.name, mat => mat);

                foreach (var config in fontConfig)
                {
                    if (fontToDictionary.TryGetValue(config.fontKey, out var fontAsset))
                    {
                        config.fontAsset = fontAsset;
                    }
                    loadedFontDictionary.Add(config.language.ToString(), fontAsset);
                    config.materials = config.materialKey
                        .Where(materialDictionary.ContainsKey)
                        .Select(key => materialDictionary[key])
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error loading assets with key {labelKey}: {ex.Message}");
            }

        }
    }
}

[System.Serializable]
public class FontConfig
{
    public eLanguage language;
    public string fontKey;
    public TMP_FontAsset fontAsset;
    public string[] materialKey;
    public List<Material> materials;
}

/// <summary>
/// SimplifiedChinese = ZH = SC Font = cn in enum(China)
/// TraditionalChinese = ZH_TW = TC font = zh in enum (Taiwan)
/// </summary>
public enum eLanguage
{
    en = 0,
    kr = 1,
    cn = 2,
    zh = 3,
    ru = 4,
    ja = 5
}

