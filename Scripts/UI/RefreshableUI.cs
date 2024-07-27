using UnityEngine;
using UnityEngine.UI;

public class RefreshableUI : MonoBehaviour
{
    private LayoutGroup _layoutGroup;
    private ContentSizeFitter _contentSizeFitter;
    public bool needRefresh = false;
    
    protected virtual void Start()
    {
        if (needRefresh)
        {
            _layoutGroup = transform.GetComponent<LayoutGroup>();
            _contentSizeFitter = transform.GetComponent<ContentSizeFitter>();
        }
    }

    protected void Refresh()
    {
        if (!needRefresh) return;
        
        var rectTransform = (RectTransform)transform;
        Refresh(rectTransform);
    }

    private void Refresh(RectTransform rectTransform)
    {
        if (rectTransform == null || !rectTransform.gameObject.activeSelf)
        {
            return;
        }

        foreach (RectTransform child in rectTransform)
        {
            Refresh(child);
        }
        
        if (_layoutGroup != null)
        {
            _layoutGroup.SetLayoutHorizontal();
            _layoutGroup.SetLayoutVertical();
        }

        if (_contentSizeFitter != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        }
    }
}
