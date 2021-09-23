using System.Collections.Generic;
using UnityEngine;
using HegaCore.UI;

namespace HegaCore.Events.Commands.UI
{
    public class UICommandControlActivity :UICommand
    {
        [Space]
        [SerializeField]
        private UIConversationDialog dialog = null;
        public bool show;
        public override void Invoke(in Segment<object> parameters)
        {
            if (show)
            {
                if (ValidateParams(parameters, 2, nameof(UICommandControlActivity), true))
                {
                    if (this.converter.TryConvert(parameters[0], out float showDuration) &&
                        this.converter.TryConvert(parameters[1], out float hideDuration))
                    {
                        this.dialog.UI_Event_ShowActivity(showDuration, hideDuration);
                        Log(this.gameObject.name , $"<color=yellow> - Show - {showDuration}-{hideDuration}");
                    }
                }
                else
                {
                    if (this.converter.TryConvert(parameters[0], out float showDuration))
                    {
                        this.dialog.UI_Event_ShowActivity(showDuration);
                        Log(this.gameObject.name , $"<color=green> - Show: - {showDuration}");
                    }
                }
            }
            else
            {
                if (ValidateParams(parameters, 1, nameof(UICommandControlActivity), true))
                {
                    if (this.converter.TryConvert(parameters[0], out float hide))
                    {
                        this.dialog.UI_Event_HideActivity(hide);
                        Log(this.gameObject.name, $"- Hide: - {hide}");
                    }
                }
                else
                {
                    this.dialog.UI_Event_HideActivity();
                    Log(this.gameObject.name, $"- Hide: - Default");
                }
            }
        }
    }    
}

