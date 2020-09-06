﻿using System;
using System.Collections.Generic;

namespace HegaCore.Commands.Data
{
    [Serializable]
    public sealed class MusicPlay : DataCommand
    {
        public override string Key => "music_play";

        public override void Invoke(in Segment<object> parameters)
        {
            if (!ValidateParameters(parameters, 1, nameof(MusicPlay)))
                return;

            if (this.converter.TryConvert(parameters[0], out string value))
            {
                AudioManager.Instance.Player.PlayMusic(value);
                Log(value);
            }
        }
    }
}