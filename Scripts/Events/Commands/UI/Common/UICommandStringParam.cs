using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HegaCore.Events.Commands;
using VisualNovelData;
using UnityEngine.Events;


public class UICommandStringParam : UICommand
{
    [System.Serializable]
    private class StringParamEvent : UnityEvent<string> { }
    
    [Space]
    [SerializeField]
    private StringParamEvent stringParamEvent = null;

    public override void Invoke(in Segment<object> parameters)
    {
        if (!ValidateParams(parameters, 1, nameof(UICommandStringParam)))
            return;
        
        if (this.converter.TryConvert(parameters[0], out string voiceKey))
        {
            this.stringParamEvent?.Invoke(voiceKey);
            Log(voiceKey);
        }
    }
}
