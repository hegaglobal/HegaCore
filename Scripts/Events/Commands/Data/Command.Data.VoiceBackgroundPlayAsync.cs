using System;
using System.Collections.Generic;

namespace HegaCore.Events.Commands.Data
{
    [Serializable]
    public sealed class VoiceBackgroundPlayAsync : DataCommand
    {
        public override string Key => "void_bg_play_async";

        public override bool Ignorable => true;

        public override void Invoke(in Segment<object> parameters)
        {
            if (!ValidateParameters(parameters, 1, nameof(VoiceBackgroundPlayAsync)))
                return;

            if (this.converter.TryConvert(parameters[0], out string value))
            {
                AudioManager.Instance.Player.PlayVoiceBGAsync(value).Forget();
                Log(value);
            }
        }
    }
}
