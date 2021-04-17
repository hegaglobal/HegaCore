using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HegaCore.Events.Commands.UI
{
    public class UICommandIntParam : UICommand
    {
        [Serializable]
        private class IntParamEvent : UnityEvent<int> { }

        [Space]
        [SerializeField]
        private IntParamEvent intParamEvent = new IntParamEvent();

        public override void Invoke(in Segment<object> parameters)
        {
            if (!ValidateParams(parameters, 1, nameof(UICommandIntParam)))
                return;

            if (this.converter.TryConvert(parameters[0], out int value))
            {
                this.intParamEvent.Invoke(value);
                Log(value);
            }
        }
    }
}
