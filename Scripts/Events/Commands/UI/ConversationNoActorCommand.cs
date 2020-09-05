using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HegaCore.Commands.UI
{
    public sealed class ConversationNoActorCommand : UICommand
    {
        [Space]
        [SerializeField]
        private UnityEvent actorEvent = null;

        public override void Invoke(in Segment<object> parameters)
        {
            this.actorEvent?.Invoke();
            Log();
        }
    }
}
