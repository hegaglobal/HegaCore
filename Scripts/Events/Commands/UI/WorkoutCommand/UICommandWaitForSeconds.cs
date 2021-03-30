using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using HegaCore.Events.Commands;
using VisualNovelData;


public class UICommandWaitForSeconds : UICommand
{
    [System.Serializable]
    private class WaitForSecondsEvent : UnityEvent<float> { }
    
    [Space]
    [SerializeField]
    private WaitForSecondsEvent waitForSecondsEvent = null;
    
    public override void Invoke(in Segment<object> parameters)
    {
        if (!ValidateParams(parameters, 1, nameof(UICommandWaitForSeconds)))
            return;
        
        if (this.converter.TryConvert(parameters[0], out float delay))
        {
            this.waitForSecondsEvent?.Invoke(delay);
            Log(delay);
        }
    }
}
