using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using UnuGames.MVVM;
using DG.Tweening;

namespace UnuGames.MVVM
{
    [RequireComponent(typeof(Image))]
    [DisallowMultipleComponent]
    public class AddressableImageBinder : BinderBase
    {
        public void ScaleToFitSize(Image img, Vector2 targetSize)
        {
            img.SetNativeSize();
            var tex = img.sprite.texture;  
            float scaleX = tex.width * 1f / targetSize.x;
            float scaleY = tex.height * 1f / targetSize.y;
            float realScale = 1;
            realScale = 1f / (scaleX < scaleY ? scaleY : scaleX);
            img.rectTransform.localScale = new Vector3(realScale,realScale,1f);
        }
        
        
        protected Image image;
        [HideInInspector] public BindingField imageValue = new BindingField("Image");
        [HideInInspector] public BindingField imageColor = new BindingField("Color");

        [Space] public bool autoCorrectSize;
        public bool autoScaleRatio;
        public Vector2 maxSize;

        [Space] public bool bDisableRaycastOnNull = false;
        public bool useTween = true;
        
        private string bindingAddress = default;
        private string bindedAddress = default;
        private bool isBinding = false;

        private Tweener colorTweener;

        public override void Initialize(bool forceInit)
        {
            if (CheckInitialize(forceInit))
            {
                image = GetComponent<Image>();

                SubscribeOnChangedEvent(imageValue, OnUpdateImage);
                SubscribeOnChangedEvent(imageColor, OnUpdateColor);
            }
        }

        public void OnUpdateImage(object newImage)
        {
            string key = newImage.ToString();
            if (string.IsNullOrEmpty(key))
            {
                isBinding = false;
                bindedAddress = string.Empty;
                bindingAddress = string.Empty;
                SetColorBlank();
                ClearOldSpriteRef();
                //Debug.Log(this.gameObject.name +  "  Bind empty");
            }
            else
            {
                bool needNewBind = false;
                if (isBinding)
                {
                    needNewBind = !string.Equals(key, bindingAddress);
                }
                else
                {
                    needNewBind = !string.Equals(key, bindedAddress);
                }

                if (needNewBind)
                {
                    //Debug.Log($"{gameObject.name} - Try to bind new: <color=green>{key}{key}</color>");
                    SetColorBlank();
                    isBinding = true;
                    bindingAddress = key;
                    AddressablesManager.LoadAsset<Sprite>(key, OnLoadedCallback, OnLoadFailedCallback);
                }

// #if UNITY_EDITOR
//             else
//             {
//                 Debug.Log($"{gameObject.name} - useless recall to bind: <color=red>{key}</color>");
//             }
// #endif
            }
        }


        void OnLoadedCallback(string key, Sprite newSprite)
        {
            if (isBinding) // !isBinding mean called to bind a empty string before this callback.
            {
                if (string.Equals(key, bindingAddress))
                {
                    image.sprite = newSprite;
                    SetColorNormal();

                    if (autoCorrectSize)
                        image.SetNativeSize();
                    else
                    {
                        if (autoScaleRatio && maxSize.x > 0 && maxSize.y > 0)
                        {
                            ScaleToFitSize(image,maxSize);
                        }
                    }

                    if (bDisableRaycastOnNull)
                    {
                        image.raycastTarget = true;
                    }

                    isBinding = false;
                    bindedAddress = key;
                    bindingAddress = string.Empty;
                }
            }
        }

        void OnLoadFailedCallback(string key)
        {
            if (isBinding) // !isBinding mean called to bind a empty string before this callback.
            {
                if (string.Equals(key, bindingAddress))
                {
                    UnuLogger.LogError("Addressable Image Binding FAILED:: " + key);
                    SetColorBlank();
                    isBinding = false;
                    bindedAddress = key;
                    bindingAddress = string.Empty;
                }
            }
        }

        public void OnUpdateColor(object newColor)
        {
            if (newColor == null)
                return;
            try
            {
                image.color = (Color) newColor;
            }
            catch
            {
                //UnuLogger.LogWarning ("Binding field is not a color!");
            }
        }

        void SetColorBlank()
        {
            colorTweener?.Kill();
            image.color = new Color(1f, 1f, 1f, 0f);
            if (bDisableRaycastOnNull)
            {
                image.raycastTarget = false;
            }
        }

        void SetColorNormal()
        {
            if (useTween)
                colorTweener = image.DOColor(new Color(1f, 1f, 1f, 1f), 0.5f);
            else
                image.color = Color.white;
        }

        void ClearOldSpriteRef()
        {
            image.sprite = null;
        }
    }
}
