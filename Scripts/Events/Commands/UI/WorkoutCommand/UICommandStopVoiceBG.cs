using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HegaCore.Events.Commands;
using VisualNovelData;
using UnityEngine.Events;


public class UICommandStopVoiceBG : UICommand
{
    [System.Serializable]
    private class StopVoiceBGEvent : UnityEvent<float> { }
    
    [Space]
    [SerializeField]
    private StopVoiceBGEvent tStopVoiceBgEvent = null;

    public override void Invoke(in Segment<object> parameters)
    {
        if (!ValidateParams(parameters, 1, nameof(UICommandPlayVoice)))
            return;
        
        if (this.converter.TryConvert(parameters[0], out float delay))
        {
            this.tStopVoiceBgEvent?.Invoke(delay);
            Log(delay);
        }
    }
}
