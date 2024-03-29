﻿using System;
using System.Collections.Generic;

namespace HegaCore.Events.Commands.Data
{
    [Serializable]
    public sealed class CharacterClipClear : DataCommand
    {
        public override string Key => "char_clip_clear";

        public override bool Ignorable => false;

        public override void Invoke(in Segment<object> parameters)
        {
            var data = EventManager.Instance.BaseDataContainer;
            data.ClearCharacterClips();
            Log();
        }
    }
}
