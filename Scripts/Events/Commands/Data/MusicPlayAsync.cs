using System;
using System.Collections.Generic;

namespace HegaCore.Commands.Data
{
    [Serializable]
    public sealed class MusicPlayAsync : DataCommand
    {
        public override string Key => "music_play_async";

        public override void Invoke(in Segment<object> parameters)
        {
            if (!ValidateParameters(parameters, 1, nameof(MusicPlayAsync)))
                return;

            if (this.converter.TryConvert(parameters[0], out string value))
            {
                AudioManager.Instance.Player.PlayMusicAsync(value).Forget();
                Log(value);
            }
        }
    }
}
