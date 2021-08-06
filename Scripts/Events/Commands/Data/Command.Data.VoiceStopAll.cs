using System;
using System.Collections.Generic;

namespace HegaCore.Events.Commands.Data
{
    [Serializable]
    public sealed class VoiceStopAll : DataCommand
    {
        public override string Key => "voice_stop_all";

        public override bool Ignorable => false;

        public override void Invoke(in Segment<object> parameters)
        {
            AudioManager.Instance.Player.StopAllVoices();
            Log();
        }
    }
}
