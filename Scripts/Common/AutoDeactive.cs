using System.Collections;
using System.Collections.Generic;
using Live2D.Cubism.Core.Unmanaged;
using UnityEngine;

public class AutoDeactive : MonoBehaviour
{
    public float duration = 1f;

    void OnEnable()
    {
        StartCoroutine(DeactiveCO(duration));
    }

    IEnumerator DeactiveCO(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}
