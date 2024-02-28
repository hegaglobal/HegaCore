using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class RotateTweener : MonoBehaviour
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
    
    private Tweener rotateTween; 
    
    public void Rotate(Vector3 rotate, float duration = 0)
    {
        UnuLogger.Log($"Rotate: <color=yellow>{gameObject.name}</color>  {rotate} -- {duration}");
        rotateTween?.Kill();
        if (duration <= 0)
        {
            _rectTransform.eulerAngles = rotate;
        }
        else
        {
            rotateTween = _rectTransform.DORotate(rotate, duration)
                .OnKill(() => rotateTween = null)
                .OnComplete(()=> rotateTween = null);
        }
    }
    
    void OnDisable()
    {
        rotateTween?.Kill();
    }
    
    void OnDestroy()
    {
        rotateTween?.Kill();
    }
}
