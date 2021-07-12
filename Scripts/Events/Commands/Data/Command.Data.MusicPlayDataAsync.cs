using System;
using System.Collections.Generic;

namespace HegaCore.Events.Commands.Data
{
    [Serializable]
    public sealed class MusicPlayDataAsync : DataCommand
    {
        public override string Key => "music_play_data_async";

        public override bool Ignorable => true;

        public override void Invoke(in Segment<object> parameters)
        {
            var value = EventManager.Instance.BaseDataContainer.Music;
            AudioManager.Instance.Player.PlayMusicAsync(value).Forget();
            Log(value);
        }
    }
}
