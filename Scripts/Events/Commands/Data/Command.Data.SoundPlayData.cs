using System;
using System.Collections.Generic;

namespace HegaCore.Events.Commands.Data
{
    [Serializable]
    public sealed class SoundPlayData : DataCommand
    {
        public override string Key => "sound_play_data";

        public override bool Ignorable => true;

        public override void Invoke(in Segment<object> parameters)
        {
            var value = EventManager.Instance.BaseDataContainer.Music;
            AudioManager.Instance.Player.PlaySound(value);
            Log(value);
        }
    }
}
