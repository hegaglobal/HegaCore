using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using HegaCore.Events.Commands;
using VisualNovelData;

public class UICommandWaitForNextPhase : UICommand
{
    [Space]
    [SerializeField]
    private UnityEvent waitForNextPhaseEvent = null;
    
    public override void Invoke(in Segment<object> parameters)
    {
        this.waitForNextPhaseEvent?.Invoke();
        Log();
    }
}
