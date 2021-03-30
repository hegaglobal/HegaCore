using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using HegaCore.Events.Commands;
using VisualNovelData;

public class UICommandChangeHscenePhase : UICommand
{
    [System.Serializable]
    private class ChangeHscenePhaseEvent : UnityEvent<int> { }
    
    [Space]
    [SerializeField]
    private ChangeHscenePhaseEvent changeHscenePhaseEvent = null;
    public override void Invoke(in Segment<object> parameters)
    {
        if (!ValidateParams(parameters, 1, nameof(UICommandChangeHscenePhase)))
            return;
        
        if (this.converter.TryConvert(parameters[0], out int phase))
        {
            this.changeHscenePhaseEvent?.Invoke(phase);
            Log(phase);
        }
    }
}
