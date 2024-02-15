using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    public Vector3 rotateSpeed;

    // Update is called once per frame
    void Update()
    {
        this.transform.localEulerAngles += rotateSpeed;
    }
}
