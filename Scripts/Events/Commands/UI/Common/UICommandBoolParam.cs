using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HegaCore.Events.Commands.UI
{
    public class UICommandBoolParam : UICommand
    {
        [Serializable]
        private class BoolParamEvent : UnityEvent<bool> { }

        [Space]
        [SerializeField]
        private BoolParamEvent boolParamEvent = new BoolParamEvent();

        public override void Invoke(in Segment<object> parameters)
        {
            if (!ValidateParams(parameters, 1, nameof(UICommandBoolParam)))
                return;

            if (this.converter.TryConvert(parameters[0], out bool value))
            {
                this.boolParamEvent.Invoke(value);
                Log(value);
            }
        }
    }
}