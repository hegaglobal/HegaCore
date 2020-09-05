using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VisualNovelData;

namespace HegaCore.Commands.UI
{
    public sealed class ConversationActorCommand : UICommand
    {
        [Space]
        [SerializeField]
        private ActorNumberEvent actorNumberEvent = null;

        public override void Invoke(in Metadata metadata, in Segment<object> parameters)
        {
            if (metadata.TryCast(out int actorNumber))
            {
                this.actorNumberEvent?.Invoke(actorNumber);
                Log(actorNumber);
            }
        }

        [System.Serializable]
        private class ActorNumberEvent : UnityEvent<int> { }
    }
}
