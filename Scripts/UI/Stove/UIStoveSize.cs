using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStoveSize : MonoBehaviour
{
#if STOVE
    public Vector2 stoveSize;
    // Start is called before the first frame update
    void Start()
    {
        var rect = GetComponent<RectTransform>();
        rect.sizeDelta = stoveSize;
    }
#endif
}
