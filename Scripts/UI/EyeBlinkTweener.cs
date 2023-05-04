using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class EyeBlinkTweener : MonoBehaviour
{
    private Image _image;
    private Tweener eyeTweener;
    public Ease EaseType;
    public List<BlinkData> BlinkDatas;
    private int blinkStep = 0;
    void Awake()
    {
        _image = GetComponent<Image>();
        if (_image == null)
        {
            UnuLogger.Log($"<color=red>{gameObject.name}</color> -- EyeBlink Tweener doesn't has Image");
            Destroy(this);
        }
    }

    [ContextMenu("Do Eye Blink")]
    public void DoEyeBlink()
    {
        #if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            UnuLogger.Log("<color=red>EyeBlinkTweener Only Run in PlayMode</color>");
            return;
        }
        #endif
        
        if (BlinkDatas == null || BlinkDatas.Count == 0)
        {
            return;
        }
        
        EyeClose();

        NextBlink();
    }

    public void EyeClose()
    {
        _image.rectTransform.sizeDelta = new Vector2(1000,0);
        _image.color = new Color(1,1,1,1);
        blinkStep = 0;
    }

    public void EyeOpen()
    {
        _image.rectTransform.DOSizeDelta(new Vector2(3840, 2160), 1f).SetEase(EaseType);
        _image.DOFade(0,1f).SetEase(EaseType);
    }

    public void NextBlink()
    {
        if (blinkStep >= BlinkDatas.Count)
        {
            EyeOpen();
            return;
        }
        
        var stepData = BlinkDatas[blinkStep];
        blinkStep++;
        eyeTweener = _image.rectTransform.DOSizeDelta(stepData.eyeOpenSize, stepData.duration)
            .OnComplete(NextBlink)
            .SetEase(EaseType);
        if (stepData.delay > 0)
        {
            eyeTweener.SetDelay(stepData.delay);
        }
    }
}

[System.Serializable]
public struct BlinkData
{
    public float duration;
    public Vector2 eyeOpenSize;
    public float delay;
}
