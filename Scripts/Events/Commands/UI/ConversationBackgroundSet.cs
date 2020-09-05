using System.Collections.Generic;
using UnityEngine;
using HegaCore.UI;

namespace HegaCore.Commands.UI
{
    public sealed class ConversationBackgroundSet : UICommand
    {
        [Space]
        [SerializeField]
        private UIConversationDialog dialog = null;

        public override void Invoke(in Segment<object> parameters)
        {
            if (!ValidateParams(parameters, 1, nameof(ConversationBackgroundSet)))
                return;

            if (!this.dialog)
            {
                UnuLogger.LogWarning($"No target {nameof(UIConversationDialog)}", this);
                return;
            }

            if (this.converter.TryConvert(parameters[0], out string value))
            {
                this.dialog.UI_Event_Background_Set(value);
                Log(value);
            }
        }
    }
}
