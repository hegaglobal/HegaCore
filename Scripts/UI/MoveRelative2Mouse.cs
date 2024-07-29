using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveRelative2Mouse : MonoBehaviour
{
    public Vector3 pz;
    public Vector3 startPos;
    public Vector3 minPos;
    public Vector3 maxPos;

    public float moveModifier = 1;
    

    private Camera mainCam;
    
    // Use this for initialization
    void Start()
    {
        startPos = transform.localPosition;
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (mainCam == null) return;
        
        Vector3 pz = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        pz.z = 0;
        Vector3 toPos = new Vector3(
            Mathf.Lerp(minPos.x * moveModifier, maxPos.x * moveModifier, pz.x),
            Mathf.Lerp(minPos.y * moveModifier, maxPos.y * moveModifier, pz.y),
            0
        );
        transform.localPosition = startPos + toPos;
        //gameObject.transform.position = pz;
        ////Debug.Log("Mouse Position: " + pz);

        //transform.position = new Vector3(StartPos.x + (pz.x * moveModifier), StartPos.y + (pz.y * moveModifier), 0);
        ////move based on the starting position and its modified value.
        //Debug.Log(pz);
    }

}