using System;
using System.Collections.Generic;

namespace HegaCore.Events.Commands.Data
{
    [Serializable]
    public sealed class VoiceStop : DataCommand
    {
        public override string Key => "voice_stop";

        public override void Invoke(in Segment<object> parameters)
        {
            AudioManager.Instance.Player.StopVoice();
            Log();
        }
    }
}
