using System.Collections.Generic;
using UnityEngine;
using HegaCore.Events.Commands;
using UnityEngine.Events;

public class UICommandNoParam : UICommand
{
    [Space]
    [SerializeField]
    private UnityEvent noParamEvent = null;

    public override void Invoke(in Segment<object> parameters)
    {
        this.noParamEvent?.Invoke();
        Log(Key);
    }
}
