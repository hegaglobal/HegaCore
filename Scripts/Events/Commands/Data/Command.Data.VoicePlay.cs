using System;
using System.Collections.Generic;

namespace HegaCore.Events.Commands.Data
{
    [Serializable]
    public sealed class VoicePlay : DataCommand
    {
        public override string Key => "void_play";

        public override bool Ignorable => true;

        public override void Invoke(in Segment<object> parameters)
        {
            if (!ValidateParameters(parameters, 1, nameof(VoicePlay)))
                return;

            if (this.converter.TryConvert(parameters[0], out string value))
            {
                AudioManager.Instance.Player.PlayVoice(value);
                Log(value);
            }
        }
    }
}
