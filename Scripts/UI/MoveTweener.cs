using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(RectTransform))]
public class MoveTweener : MonoBehaviour
{
    private RectTransform _rectTransform;
    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        if (_rectTransform == null)
        {
            UnuLogger.Log($"{gameObject.name} -- MOVE Tweener doesn't has RectTransform");
            Destroy(this);
        }
    }
    
    private Tweener moveTweener;
    public void Move(Vector2 target, float duration = 0)
    {
        UnuLogger.Log($"Move: <color=yellow>{gameObject.name}</color> - [{target.x} - {target.y}] -- {duration}");
        moveTweener?.Kill();
        if (duration <= 0)
        {
            _rectTransform.anchoredPosition = target;
        }
        else
        {
            moveTweener = _rectTransform.DOAnchorPos(target, duration, true).SetEase(Ease.Linear)
                .OnKill(()=> moveTweener = null)
                .OnComplete(()=> moveTweener = null);
        }
    }
    
    void OnDisable()
    {
        moveTweener?.Kill();
    }
    
    void OnDestroy()
    {
        moveTweener?.Kill();
    }
}
