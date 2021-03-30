using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HegaCore.Events.Commands;
using UnityEngine.Events;


public class UICommandClearVoiceText : UICommand
{
    [Space]
    [SerializeField]
    private UnityEvent clearVoiceText = null;

    public override void Invoke(in Segment<object> parameters)
    {
        this.clearVoiceText?.Invoke();
        Log();
    }
}
