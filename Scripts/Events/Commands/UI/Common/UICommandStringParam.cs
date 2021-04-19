using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HegaCore.Events.Commands.UI
{
    public class UICommandStringParam : UICommand
    {
        [Serializable]
        private class StringParamEvent : UnityEvent<string> { }

        [Space]
        [SerializeField]
        private StringParamEvent stringParamEvent = new StringParamEvent();

        public override void Invoke(in Segment<object> parameters)
        {
            if (!ValidateParams(parameters, 1, nameof(UICommandStringParam)))
                return;

            if (this.converter.TryConvert(parameters[0], out string value))
            {
                this.stringParamEvent.Invoke(value);
                Log(value);
            }
        }
    }
}
