using System;
using System.Collections.Generic;

namespace HegaCore.Events.Commands.Data
{
    [Serializable]
    public sealed class MusicStop : DataCommand
    {
        public override string Key => "music_stop";

        public override bool Ignorable => false;

        public override void Invoke(in Segment<object> parameters)
        {
            AudioManager.Instance.Player.StopMusic();
            Log();
        }
    }
}
