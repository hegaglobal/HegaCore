using System;
using System.Collections.Generic;

namespace HegaCore.Events.Commands.Data
{
    [Serializable]
    public sealed class MusicSetLoop : DataCommand
    {
        public override string Key => "music_set_loop";

        public override bool Ignorable => true;

        public override void Invoke(in Segment<object> parameters)
        {
            if (!ValidateParameters(parameters, 1, nameof(MusicSetLoop)))
                return;

            if (this.converter.TryConvert(parameters[0], out bool value))
            {
                AudioManager.Instance.Player.SetMusicLoop(value);
                Log(value);
            }
        }
    }
}
