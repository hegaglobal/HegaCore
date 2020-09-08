using System;
using System.Collections.Generic;

namespace HegaCore.Commands.Data
{
    [Serializable]
    public sealed class SoundPlay : DataCommand
    {
        public override string Key => "sound_play";

        public override void Invoke(in Segment<object> parameters)
        {
            if (!ValidateParameters(parameters, 1, nameof(SoundPlay)))
                return;

            if (this.converter.TryConvert(parameters[0], out string value))
            {
                AudioManager.Instance.Player.PlaySound(value);
                Log(value);
            }
        }
    }
}
