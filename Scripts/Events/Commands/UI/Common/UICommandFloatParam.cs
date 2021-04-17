using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HegaCore.Events.Commands.UI
{
    public class UICommandFloatParam : UICommand
    {
        [Serializable]
        private class FloatParamEvent : UnityEvent<float> { }

        [Space]
        [SerializeField]
        private FloatParamEvent floatParamEvent = new FloatParamEvent();

        public override void Invoke(in Segment<object> parameters)
        {
            if (!ValidateParams(parameters, 1, nameof(UICommandFloatParam)))
                return;

            if (this.converter.TryConvert(parameters[0], out float value))
            {
                this.floatParamEvent.Invoke(value);
                Log(value);
            }
        }
    }
}
