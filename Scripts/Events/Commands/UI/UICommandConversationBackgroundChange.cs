using System.Collections.Generic;
using UnityEngine;
using HegaCore.UI;

namespace HegaCore.Commands.UI
{
    public sealed class UICommandConversationBackgroundChange : UICommand
    {
        [Space]
        [SerializeField]
        private UIConversationDialog dialog = null;

        public override void Invoke(in Segment<object> parameters)
        {
            if (!this.dialog)
            {
                UnuLogger.LogWarning($"No target {nameof(UIConversationDialog)}", this);
                return;
            }

            string name;

            if (ValidateParams(parameters, 2, nameof(UICommandConversationBackgroundChange), true))
            {
                if (this.converter.TryConvert(parameters[0], out name) &&
                    this.converter.TryConvert(parameters[1], out float duration))
                {
                    this.dialog.UI_Event_Background_Change(name, duration);
                    Log(name, duration);
                }

                return;
            }

            if (!ValidateParams(parameters, 1, nameof(UICommandConversationBackgroundChange)))
                return;

            if (this.converter.TryConvert(parameters[0], out name))
            {
                this.dialog.UI_Event_Background_Change(name);
                Log(name);
            }
        }
    }
}
