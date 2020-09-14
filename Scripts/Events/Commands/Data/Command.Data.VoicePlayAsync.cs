using System;
using System.Collections.Generic;

namespace HegaCore.Events.Commands.Data
{
    [Serializable]
    public sealed class VoicePlayAsync : DataCommand
    {
        public override string Key => "void_play_async";

        public override void Invoke(in Segment<object> parameters)
        {
            if (!ValidateParameters(parameters, 1, nameof(VoicePlayAsync)))
                return;

            if (this.converter.TryConvert(parameters[0], out string value))
            {
                AudioManager.Instance.Player.PlayVoiceAsync(value).Forget();
                Log(value);
            }
        }
    }
}
