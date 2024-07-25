using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;
using SimpleLocalization;
using System.Collections.Generic;
using Sirenix.OdinInspector;

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

        [SerializeField]
        private TMP_Text text = null;
        [SerializeField]
        private CustomMaterialPreset preset;

        [SerializeField]
        private TMP_FontAsset defaultFont;
        [SerializeField]
        private Material defaultFontMat;

        private void OnValidate()
        {
            if (text == null)
                this.text = GetComponent<TMP_Text>();
        }

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
            var text = "";
            if (upperCase)
                text = L10n.Localize(this.key, this.silent).ToUpper();
            else
                text = L10n.Localize(this.key, this.silent);
            
            this.text.SetText(text);
            CheckFont();
        }
        void CheckFont()
        {
            if (LocalizeFontAsset.Instance.CurrentFont != null)
            {
                text.font = LocalizeFontAsset.Instance.CurrentFont;
                var fontData = preset.GetFontData(LocalizeFontAsset.Instance.CurrentLanguage);

                if (fontData != null)
                {
                    if (fontData.materialPreset != null)
                    {
                        text.fontMaterial = fontData.materialPreset;
                        text.material = fontData.materialPreset;
                    }
                    if(fontData.fontSize > 0)
                        text.fontSize = fontData.fontSize;
                }
            }
            else
            {
                if (defaultFont)
                    text.font = defaultFont;
                if (defaultFontMat)
                    text.fontMaterial = defaultFontMat;
                    text.material = defaultFontMat;
            }
        }


        [System.Serializable]
        private class CustomMaterialPreset
        {
            [SerializeField]
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
        private class LanguageFontConfig
        {
            public eLanguage language;
            public FontData fontData;
        }

        [System .Serializable]
        private class FontData
        {            
            public Material materialPreset = null;
            [InfoBox("if fonsize > 0 then override current font size of textmeshpro")]
            public int fontSize = 0;
        }


#if UNITY_EDITOR

        [Button]
        private void LoadMaterialButton()
        {

            if (text == null)
                this.text = GetComponent<TMP_Text>();

            if (defaultFont == null)
            {
                defaultFont = text.font;
            }
            //if (defaultFontMat == null)
            //{
            //    defaultFontMat = text.font;
            //}
            UnityEditor.EditorUtility.SetDirty(transform.root);
            UnityEditor.AssetDatabase.SaveAssets();
        }

        [UnityEditor.MenuItem("CONTEXT/TextMeshProUGUI/Convert to ObservableTextMeshProUGUI")]
        private static void Convert(UnityEditor.MenuCommand command)
        {
            TextMeshProUGUI oldTextMeshPro = command.context as TextMeshProUGUI;

            if (oldTextMeshPro == null)
            {
                Debug.LogError("Can't convert. Object is not TextMeshProUGUI.");
                return;
            }

            // Lưu trữ các thuộc tính từ TextMeshProUGUI cũ
            GameObject gameObject = oldTextMeshPro.gameObject;
            string text = oldTextMeshPro.text;
            Color color = oldTextMeshPro.color;
            TMP_FontAsset font = oldTextMeshPro.font;
            Material fontMaterial = oldTextMeshPro.fontSharedMaterial;
            float fontSize = oldTextMeshPro.fontSize;
            FontStyles fontStyle = oldTextMeshPro.fontStyle;
            TextAlignmentOptions alignment = oldTextMeshPro.alignment;
            bool raycastTarget = oldTextMeshPro.raycastTarget;
            bool enableAutoSizing = oldTextMeshPro.enableAutoSizing;
            float fontSizeMin = oldTextMeshPro.fontSizeMin;
            float fontSizeMax = oldTextMeshPro.fontSizeMax;
            float characterSpacing = oldTextMeshPro.characterSpacing;
            float wordSpacing = oldTextMeshPro.wordSpacing;
            float lineSpacing = oldTextMeshPro.lineSpacing;
            float paragraphSpacing = oldTextMeshPro.paragraphSpacing;
            float lineSpacingAdjustment = oldTextMeshPro.lineSpacingAdjustment;
            bool isRichText = oldTextMeshPro.richText;
            bool enableWordWrapping = oldTextMeshPro.enableWordWrapping;
            float wordWrappingRatios = oldTextMeshPro.wordWrappingRatios;
            bool enableKerning = oldTextMeshPro.enableKerning;
            bool extraPadding = oldTextMeshPro.extraPadding;
            bool overrideColorTags = oldTextMeshPro.overrideColorTags;
            bool parseCtrlCharacters = oldTextMeshPro.parseCtrlCharacters;
            float characterWidthAdjustment = oldTextMeshPro.characterWidthAdjustment;
            VertexSortingOrder geometrySortingOrder = oldTextMeshPro.geometrySortingOrder;
            bool useMaxVisibleDescender = oldTextMeshPro.useMaxVisibleDescender;
            int firstVisibleCharacter = oldTextMeshPro.firstVisibleCharacter;
            bool enableCulling = oldTextMeshPro.enableCulling;

            // Xóa component TextMeshProUGUI cũ
            Object.DestroyImmediate(oldTextMeshPro);

            // Thêm component ObservableLocalizeTextMeshProUGUI mới
            ObservableLocalizeTextMeshProUGUI newTextMeshPro = gameObject.AddComponent<ObservableLocalizeTextMeshProUGUI>();

            // Gán lại các thuộc tính từ TextMeshProUGUI cũ sang ObservableLocalizeTextMeshProUGUI mới
            newTextMeshPro.text = text;
            newTextMeshPro.color = color;
            newTextMeshPro.font = font;
            newTextMeshPro.fontSharedMaterial = fontMaterial;
            newTextMeshPro.fontSize = fontSize;
            newTextMeshPro.fontStyle = fontStyle;
            newTextMeshPro.alignment = alignment;
            newTextMeshPro.raycastTarget = raycastTarget;
            newTextMeshPro.enableAutoSizing = enableAutoSizing;
            newTextMeshPro.fontSizeMin = fontSizeMin;
            newTextMeshPro.fontSizeMax = fontSizeMax;
            newTextMeshPro.characterSpacing = characterSpacing;
            newTextMeshPro.wordSpacing = wordSpacing;
            newTextMeshPro.lineSpacing = lineSpacing;
            newTextMeshPro.paragraphSpacing = paragraphSpacing;
            newTextMeshPro.lineSpacingAdjustment = lineSpacingAdjustment;
            newTextMeshPro.richText = isRichText;
            newTextMeshPro.enableWordWrapping = enableWordWrapping;
            newTextMeshPro.wordWrappingRatios = wordWrappingRatios;
            newTextMeshPro.enableKerning = enableKerning;
            newTextMeshPro.extraPadding = extraPadding;
            newTextMeshPro.overrideColorTags = overrideColorTags;
            newTextMeshPro.parseCtrlCharacters = parseCtrlCharacters;
            newTextMeshPro.characterWidthAdjustment = characterWidthAdjustment;
            newTextMeshPro.geometrySortingOrder = geometrySortingOrder;
            newTextMeshPro.useMaxVisibleDescender = useMaxVisibleDescender;
            newTextMeshPro.firstVisibleCharacter = firstVisibleCharacter;
            newTextMeshPro.enableCulling = enableCulling;

            Debug.Log("Successfully converted TextMeshProUGUI to ObservableTextMeshProUGUI.");
        }

#endif
    }
}