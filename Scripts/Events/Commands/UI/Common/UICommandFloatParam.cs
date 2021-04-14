using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HegaCore.Events.Commands;
using VisualNovelData;
using UnityEngine.Events;


public class UICommandFloatParam : UICommand
{
    [System.Serializable]
    private class FloatParamEvent : UnityEvent<float> { }
    
    [Space]
    [SerializeField]
    private FloatParamEvent floatParamEvent = null;

    public override void Invoke(in Segment<object> parameters)
    {
        if (!ValidateParams(parameters, 1, nameof(UICommandFloatParam)))
            return;
        
        if (this.converter.TryConvert(parameters[0], out float delay))
        {
            this.floatParamEvent?.Invoke(delay);
            Log(delay);
        }
    }
}
