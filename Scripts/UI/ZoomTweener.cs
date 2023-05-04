using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class ZoomTweener : MonoBehaviour
{
    private RectTransform _rectTransform;
    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        if (_rectTransform == null)
        {
            UnuLogger.Log($"{gameObject.name} -- ZOOM Tweener doesn't has RectTransform");
            Destroy(this);
        }
    }
    
    private Tweener zoomTweener;
    
    public void Zoom(float zoom, float duration = 0)
    {
        UnuLogger.Log($"Zoom: <color=yellow>{gameObject.name}</color>  {zoom} -- {duration}");
        zoomTweener?.Kill();
        if (duration <= 0)
        {
            _rectTransform.localScale = new Vector3(zoom, zoom, zoom);
        }
        else
        {
            zoomTweener = _rectTransform.DOScale(zoom, duration)
                .OnKill(() => zoomTweener = null)
                .OnComplete(()=> zoomTweener = null);
        }
    }

    void OnDisable()
    {
        zoomTweener?.Kill();
    }
    
    void OnDestroy()
    {
        zoomTweener?.Kill();
    }
}
