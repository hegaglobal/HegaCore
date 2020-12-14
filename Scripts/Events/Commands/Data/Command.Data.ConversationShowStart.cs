using System;
using System.Collections.Generic;

namespace HegaCore.Events.Commands
{
    [Serializable]
    public sealed class ConversationShowStart : DataCommand
    {
        public override string Key => "conv_show_start";

        public override void Invoke(in Segment<object> parameters)
        {
            if (!ValidateParameters(parameters, 1, nameof(ConversationShowStart)))
                return;

            if (!this.converter.TryConvert(parameters[0], out bool value))
                return;

            EventManager.Instance.BaseDataContainer.ShowStartConversation = value;
            Log(value);
        }
    }
}
