using System;
using System.Collections.Generic;

namespace HegaCore.Commands.Data
{
    [Serializable]
    public sealed class VoiceStop : DataCommand
    {
        public override string Key => "voice_stop";

        public override void Invoke(in Segment<object> parameters)
        {
            if (!ValidateParameters(parameters, 1, nameof(VoiceStop)))
                return;

            if (this.converter.TryConvert(parameters[0], out string value))
            {
                AudioManager.Instance.Player.StopVoice();
                Log();
            }
        }
    }
}
