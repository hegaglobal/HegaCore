using System;
using System.Collections.Generic;

namespace HegaCore.Events.Commands.Data
{
    [Serializable]
    public sealed class SoundStop : DataCommand
    {
        public override string Key => "sound_stop";

        public override void Invoke(in Segment<object> parameters)
        {
            AudioManager.Instance.Player.StopSound();
            Log();
        }
    }
}
