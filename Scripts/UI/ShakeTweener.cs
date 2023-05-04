using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class ShakeTweener : MonoBehaviour
{
    private RectTransform _rectTransform;
    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        if (_rectTransform == null)
        {
            UnuLogger.Log($"{gameObject.name} -- SHAKE Tweener doesn't has RectTransform");
            Destroy(this);
        }
    }
    
    private Tweener shakeTweener;
    private Tweener shakeZoomTweener;
    private Vector2 shakeFrom;
    private Vector2 shakeTo;
    private float shakeFrequency;
    private float shakeZoomTo;
    
    public void Shake(Vector2 from, Vector2 to, float frequency, float zoomTo = 1f)
    {
        StopShake();
        UnuLogger.Log($"<color=red>Shake</color> [{from}] - [{to}] - {frequency} - {zoomTo}");
        shakeFrom = from;
        shakeTo = to;
        shakeFrequency = frequency;
        shakeZoomTo = zoomTo;
		
        shakeTweener = _rectTransform.DOAnchorPos(shakeFrom, 0.25f, true).OnComplete(() =>
            shakeTweener = _rectTransform.DOAnchorPos(shakeTo, shakeFrequency, true).SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine));

        if (shakeZoomTo > 1)
        {
            shakeZoomTweener = _rectTransform.DOScale(1, 0.2f).OnComplete(() =>
                shakeZoomTweener = _rectTransform.DOScale(shakeZoomTo, shakeFrequency).SetLoops(-1, LoopType.Yoyo)
                    .SetEase(Ease.InOutSine)
            );
        }
    }
	
    public void StopShake(bool reset = false)
    {
        shakeTweener?.Kill();
        shakeTweener = null;
        shakeZoomTweener?.Kill();
        shakeZoomTweener = null;
        if (reset)
        {
            _rectTransform.anchoredPosition = Vector2.zero;
            _rectTransform.localScale = Vector2.one;
        }
    }
    
    void OnDisable()
    {
        shakeTweener?.Kill();
        shakeZoomTweener?.Kill();
    }

    void OnDestroy()
    {
        shakeTweener?.Kill();
        shakeZoomTweener?.Kill();
    }
}
