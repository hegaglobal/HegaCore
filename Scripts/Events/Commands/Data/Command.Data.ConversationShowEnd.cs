using System;
using System.Collections.Generic;

namespace HegaCore.Events.Commands
{
    [Serializable]
    public sealed class ConversationShowEnd : DataCommand
    {
        public override string Key => "conv_show_end";

        public override void Invoke(in Segment<object> parameters)
        {
            if (!ValidateParameters(parameters, 1, nameof(ConversationShowEnd)))
                return;

            if (!this.converter.TryConvert(parameters[0], out bool value))
                return;

            EventManager.Instance.BaseDataContainer.ShowEndConversation = value;
            Log(value);
        }
    }
}
