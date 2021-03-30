using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HegaCore.Events.Commands;
using VisualNovelData;
using UnityEngine.Events;


public class UICommandPlayVoiceLoop : UICommand
{
    [System.Serializable]
    private class PlayVoiceEvent : UnityEvent<string> { }
    
    [Space]
    [SerializeField]
    private PlayVoiceEvent playvoiceEvents = null;

    public override void Invoke(in Segment<object> parameters)
    {
        if (!ValidateParams(parameters, 1, nameof(UICommandPlayVoice)))
            return;
        
        if (this.converter.TryConvert(parameters[0], out string voiceKey))
        {
            this.playvoiceEvents?.Invoke(voiceKey);
            Log(voiceKey);
        }
    }
}
