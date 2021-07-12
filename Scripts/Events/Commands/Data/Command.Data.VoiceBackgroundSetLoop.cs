using System;
using System.Collections.Generic;

namespace HegaCore.Events.Commands.Data
{
    [Serializable]
    public sealed class VoiceBackgroundSetLoop : DataCommand
    {
        public override string Key => "void_bg_set_loop";

        public override bool Ignorable => true;

        public override void Invoke(in Segment<object> parameters)
        {
            if (!ValidateParameters(parameters, 1, nameof(VoiceBackgroundSetLoop)))
                return;

            if (this.converter.TryConvert(parameters[0], out bool value))
            {
                AudioManager.Instance.Player.SetVoiceBGLoop(value);
                Log(value);
            }
        }
    }
}
