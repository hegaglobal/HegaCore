using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HegaCore.Events.Commands.UI
{
    public class UICommandNoParam : UICommand
    {
        [Space]
        [SerializeField]
        private UnityEvent noParamEvent = new UnityEvent();

        public override void Invoke(in Segment<object> parameters)
        {
            this.noParamEvent.Invoke();
            Log();
        }
    }
}
