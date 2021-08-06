using System;
using System.Collections.Generic;

namespace HegaCore.Events.Commands.Data
{
    [Serializable]
    public sealed class VoiceBackgroundPlay : DataCommand
    {
        public override string Key => "void_bg_play";

        public override bool Ignorable => true;

        public override void Invoke(in Segment<object> parameters)
        {
            if (!ValidateParameters(parameters, 1, nameof(VoiceBackgroundPlay)))
                return;

            if (this.converter.TryConvert(parameters[0], out string value))
            {
                AudioManager.Instance.Player.PlayVoiceBG(value);
                Log(value);
            }
        }
    }
}
