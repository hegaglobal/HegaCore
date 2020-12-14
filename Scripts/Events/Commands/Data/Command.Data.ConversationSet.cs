using System;
using System.Collections.Generic;

namespace HegaCore.Events.Commands.Data
{
    [Serializable]
    public sealed class ConversationSet : DataCommand
    {
        public override string Key => "conv_set";

        public override void Invoke(in Segment<object> parameters)
        {
            if (!ValidateParameters(parameters, 1, nameof(ConversationSet)))
                return;

            if (!this.converter.TryConvert(parameters[0], out string value))
                return;

            EventManager.Instance.BaseDataContainer.Conversation = value;
            Log(value);
        }
    }
}
