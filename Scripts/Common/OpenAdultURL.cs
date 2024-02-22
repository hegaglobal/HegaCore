using System.Collections;
using System.Collections.Generic;
using HegaCore;
using UnityEngine;

public class OpenAdultURL : MonoBehaviour
{
    public string url;
    public string AO_url;
    
    public void OpenLink()
    {
        Application.OpenURL(DataManager.Instance.Steam_DLC ? AO_url : url);
    }
}
