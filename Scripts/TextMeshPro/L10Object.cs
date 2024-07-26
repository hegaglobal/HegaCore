using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;
using SimpleLocalization;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Xml;

namespace HegaCore
{
    //[RequireComponent(typeof(TMP_Text))]
    public class L10Object : MonoBehaviour, IL10n
    {
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

#if UNITY_EDITOR
        private void OnValidate()
        {
            if(UnityEditor.EditorApplication.isPlaying) return;
            this.text = GetComponent<TMP_Text>();
            if(this.text == null)
            {
                return;
            }
            defaultFontSize = text.fontSize;
            defaultAutosize = text.enableAutoSizing;
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
                }
            }
            else
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
            }
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
#endif
    }
}