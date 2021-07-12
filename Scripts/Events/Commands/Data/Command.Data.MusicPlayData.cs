using System;
using System.Collections.Generic;

namespace HegaCore.Events.Commands.Data
{
    [Serializable]
    public sealed class MusicPlayData : DataCommand
    {
        public override string Key => "music_play_data";

        public override bool Ignorable => true;

        public override void Invoke(in Segment<object> parameters)
        {
            var value = EventManager.Instance.BaseDataContainer.Music;
            AudioManager.Instance.Player.PlayMusic(value);
            Log(value);
        }
    }
}
