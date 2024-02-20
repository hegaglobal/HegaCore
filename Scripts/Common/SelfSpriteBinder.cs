using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.AddressableAssets;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SelfSpriteBinder : MonoBehaviour
{
    [InlineButton("GetCurrent", "Current")]
    public string key;

    public bool nameBySprite = false;
    protected SpriteRenderer spriteRenderer;
    private string lastBindValue;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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
            if (nameBySprite)
                spriteRenderer.gameObject.name = key + " (address)";
            
            spriteRenderer.sprite = newSprite;
            SetColorNormal();
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
        spriteRenderer.enabled = false;
    }

    void SetColorNormal()
    {
        spriteRenderer.enabled = true;
    }

    void GetCurrent()
    {
        var sprite = GetComponent<SpriteRenderer>();
        if (sprite != null && sprite.sprite != null)
        {
            key = sprite.sprite.name;
        }
        else
        {
            key = String.Empty;
        }
    }
}
