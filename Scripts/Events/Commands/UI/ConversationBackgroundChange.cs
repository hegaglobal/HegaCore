using System.Collections.Generic;
using UnityEngine;
using HegaCore.UI;

namespace HegaCore.Commands.UI
{
    public sealed class ConversationBackgroundChange : UICommand
    {
        [Space]
        [SerializeField]
        private UIConversationDialog dialog = null;

        public override void Invoke(in Segment<object> parameters)
        {
            if (!ValidateParams(parameters, 2, nameof(ConversationBackgroundChange)))
                return;

            if (!this.dialog)
            {
                UnuLogger.LogWarning($"No target {nameof(UIConversationDialog)}", this);
                return;
            }

            if (this.converter.TryConvert(parameters[0], out string name) &&
                this.converter.TryConvert(parameters[1], out float duration))
            {
                this.dialog.UI_Event_Background_Change(name, duration);
                Log(name, duration);
            }
        }
    }
}
