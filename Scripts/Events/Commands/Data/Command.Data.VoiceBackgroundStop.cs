using System;
using System.Collections.Generic;

namespace HegaCore.Events.Commands.Data
{
    [Serializable]
    public sealed class VoiceBackgroundStop : DataCommand
    {
        public override string Key => "voice_bg_stop";

        public override bool Ignorable => false;

        public override void Invoke(in Segment<object> parameters)
        {
            AudioManager.Instance.Player.StopVoiceBG();
            Log();
        }
    }
}
