using System;
using System.Collections.Generic;

namespace HegaCore.Commands.Data
{
    [Serializable]
    public sealed class SoundStop : DataCommand
    {
        public override string Key => "sound_stop";

        public override void Invoke(in Segment<object> parameters)
        {
            if (!ValidateParameters(parameters, 1, nameof(SoundStop)))
                return;

            if (this.converter.TryConvert(parameters[0], out string value))
            {
                AudioManager.Instance.Player.StopSound();
                Log();
            }
        }
    }
}
