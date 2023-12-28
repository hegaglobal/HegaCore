using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SelfImageBinder : MonoBehaviour
{
    public string key;
    public bool autoCorrectSize;
    protected Image image;
    private string lastBindValue;
    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        UpdateImage();
    }

    public void UpdateImage(string newKey)
    {
        key = newKey;
        UpdateImage();
    }
    
    private void UpdateImage()
    {
        if (!string.Equals(lastBindValue,key))
        {
            SetColorBlank();
            lastBindValue = key;
            image.gameObject.name = key + " (address)";
            if (!string.IsNullOrEmpty(key))
            {
                AddressablesManager.LoadAsset<Sprite>(key, OnLoadedCallback, OnLoadFailedCallback);
            }
        }
    }

    void OnLoadedCallback(string key, Sprite newSprite)
    {
        if (string.Equals(key,lastBindValue))
        {
            image.sprite = newSprite;
            SetColorNormal();
            if (autoCorrectSize)
                image.SetNativeSize();
        }
    }

    void OnLoadFailedCallback(string key)
    {
        if (string.Equals(key, lastBindValue))
        {
            SetColorBlank();
        }
    }
    
    void SetColorBlank()
    {
        image.color = new Color(1f, 1f, 1f, 0f);
    }

    void SetColorNormal()
    {
        image.DOColor(new Color(1f, 1f, 1f, 1f), 0.5f);
    }
}