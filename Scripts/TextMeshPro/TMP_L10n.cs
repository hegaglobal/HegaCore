using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;
using SimpleLocalization;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.UI;

namespace HegaCore
{
    //[RequireComponent(typeof(TMP_Text))]
    public class TMP_L10n : MonoBehaviour, IL10n
    {
        [SerializeField]
        private string key = string.Empty;

        [SerializeField] private bool upperCase = false;
        
        [SerializeField]
        private bool silent = false;

        [Header("Localize font config")]
        [InfoBox("if true & config preset = null in curent language use defaultFont & fallback language")]
        [SerializeField] private bool updateFontWithNullConfig = true;
        [SerializeField]
        private TMP_Text text = null;
        [SerializeField]
        private CustomMaterialPreset preset;

        [SerializeField]
        private TMP_FontAsset defaultFont;
        [SerializeField]
        private Material defaultFontMat;
        [SerializeField]
        private float defaultFontSize = 0;
        [SerializeField]
        private bool defaultAutosize = false;

        // Add Spacing Options
        [Header("Spacing Options")]
        [SerializeField]
        private float defaultCharacterSpacing = 0;  // Default character spacing
        [SerializeField]
        private float defaultLineSpacing = 0;       // Default line spacing

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (UnityEditor.EditorApplication.isPlaying) return;
            this.text = GetComponent<TMP_Text>();
            if (this.text == null)
            {
                return;
            }
            defaultFontSize = text.fontSize;
            defaultAutosize = text.enableAutoSizing;
            defaultCharacterSpacing = text.characterSpacing;
            defaultLineSpacing = text.lineSpacing;
            LoadMaterialDefault();
        }
#endif

        private void Start()
        {

            L10n.Register(this);

            LazyLocalize().Forget();

        }

        private async UniTaskVoid LazyLocalize()
        {
            await UniTask.WaitUntil(() => L10n.IsInitialized);

            Localize();
        }

        private void OnDestroy()
        {
            if (SingletonBehaviour.Quitting)
                return;
            L10n.Deregister(this);
        }

        public void Localize()
        {
            if (!string.IsNullOrEmpty(key))
            {
                var text = "";
                if (upperCase)
                    text = L10n.Localize(this.key, this.silent).ToUpper();
                else
                    text = L10n.Localize(this.key, this.silent);

                this.text.SetText(text);
            }
            CheckFont();
        }
        void CheckFont()
        {
            if (LocalizeFontAsset.Instance.CurrentFont != null)
            {

                var fontData = preset.GetFontData(LocalizeFontAsset.Instance.CurrentLanguage);

                if(fontData == null && !updateFontWithNullConfig)
                {
                    SetDefaultFont();
                    return;
                }
                text.font = LocalizeFontAsset.Instance.CurrentFont;
                if (fontData != null)
                {
                    if (fontData.materialPreset != null)
                    {
                        text.fontMaterial = fontData.materialPreset;
                        text.material = fontData.materialPreset;
                    }
                    if (fontData.fontSize > 0)
                    {
                        if (text.enableAutoSizing) text.enableAutoSizing = false;
                        text.fontSize = fontData.fontSize;
                    }
                    else if (defaultFontSize > 0)
                    {
                        text.fontSize = defaultFontSize;
                        text.enableAutoSizing = defaultAutosize;
                    }
                    // Apply custom spacing options
                    text.characterSpacing = fontData.characterSpacing != 0 ? fontData.characterSpacing : defaultCharacterSpacing;
                    text.lineSpacing = fontData.lineSpacing != 0 ? fontData.lineSpacing : defaultLineSpacing;
                }
                
            }
            else
            {
                SetDefaultFont();
            }
        }

        void SetDefaultFont()
        {
            if (defaultFont)
                text.font = defaultFont;
            if (defaultFontMat)
            {
                text.fontMaterial = defaultFontMat;
                text.material = defaultFontMat;
            }

            if (defaultFontSize > 0)
            {
                text.fontSize = defaultFontSize;
            }
            text.enableAutoSizing = defaultAutosize;

            // Apply default spacing
            text.characterSpacing = defaultCharacterSpacing;
            text.lineSpacing = defaultLineSpacing;
        }



#if UNITY_EDITOR

        [Button]
        private void LoadMaterialDefault()
        {

            if (text == null)
                this.text = GetComponent<TMP_Text>();

            // Retrieve the original font asset from the project
            TMP_FontAsset originalFont = GetOriginalFontAsset(text.font);
            if (originalFont != null)
            {
                defaultFont = originalFont;

                // Use the current material preset used by the TMP_Text component
                Material currentMaterialPreset = text.fontSharedMaterial;
                if (currentMaterialPreset != null)
                {
                    defaultFontMat = GetOriginalMaterial(currentMaterialPreset);
                }
            }
            else
            {
                Debug.LogWarning("Could not find the original font asset in the project.");
            }

            UnityEditor.EditorUtility.SetDirty(this);
            //UnityEditor.AssetDatabase.SaveAssets();
        }

        // Helper method to find the original TMP_FontAsset in the project
        private TMP_FontAsset GetOriginalFontAsset(TMP_FontAsset fontInstance)
        {
            // Check if the font instance is a dynamic copy (indicating it's an instance and not the original)
            if (UnityEditor.AssetDatabase.Contains(fontInstance))
            {
                return fontInstance;
            }

            // Find all TMP_FontAssets in the project
            string[] fontGuids = UnityEditor.AssetDatabase.FindAssets("t:TMP_FontAsset");
            foreach (string guid in fontGuids)
            {
                string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                TMP_FontAsset assetFont = UnityEditor.AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(path);
                if (assetFont != null && assetFont.name == fontInstance.name)
                {
                    return assetFont;
                }
            }
            return null;
        }

        // Helper method to find the original Material in the project
        private Material GetOriginalMaterial(Material materialInstance)
        {
            // Check if the material instance is a dynamic copy (indicating it's an instance and not the original)
            if (UnityEditor.AssetDatabase.Contains(materialInstance))
            {
                return materialInstance;
            }

            // Find all Materials in the project
            string[] materialGuids = UnityEditor.AssetDatabase.FindAssets("t:Material");
            foreach (string guid in materialGuids)
            {
                string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                Material assetMaterial = UnityEditor.AssetDatabase.LoadAssetAtPath<Material>(path);
                if (assetMaterial != null && assetMaterial.name == materialInstance.name)
                {
                    return assetMaterial;
                }
            }
            return null;
        }

        [Button]
        public void TestFontSetting(eLanguage language)
        {
            var fontData = preset.GetFontData(language);

            if (fontData != null)
            {
                if (fontData.materialPreset != null)
                {
                    text.fontMaterial = fontData.materialPreset;
                    text.material = fontData.materialPreset;
                }
                if (fontData.fontSize > 0)
                {
                    if (text.enableAutoSizing) text.enableAutoSizing = false;
                    text.fontSize = fontData.fontSize;
                }
                else if (defaultFontSize > 0)
                {
                    text.fontSize = defaultFontSize;
                    text.enableAutoSizing = defaultAutosize;
                }
                // Apply custom spacing options
                text.characterSpacing = fontData.characterSpacing != 0 ? fontData.characterSpacing : defaultCharacterSpacing;
                text.lineSpacing = fontData.lineSpacing != 0 ? fontData.lineSpacing : defaultLineSpacing;
            }
            else
            {
                Debug.Log($"<color=red>Config of language <color=yellow>{language}</color> is null</color>");
            }
        }
#endif
    }


    [System.Serializable]
    public class CustomMaterialPreset
    {
        [SerializeField,ListDrawerSettings(ShowIndexLabels = true)]
        private List<LanguageFontConfig> languageFontPairs = new List<LanguageFontConfig>();
        public FontData GetFontData(eLanguage _language)
        {
            var config = languageFontPairs.Find(c => c.language == _language);
            if (config != null)
            {
                return config.fontData;
            }

            return null;
        }

    }

    [System.Serializable]
    public class LanguageFontConfig
    {
        public eLanguage language;
        public FontData fontData;
    }

    [System.Serializable]
    public class FontData
    {
        public Material materialPreset = null;
        [InfoBox("if fonsize > 0 then override current font size of textmeshpro")]
        public float fontSize = 0;

        public float characterSpacing = 0;
        public float lineSpacing = 0;
    }
}