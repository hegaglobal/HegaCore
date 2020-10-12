using System;
using System.Collections.Generic;

namespace HegaCore.Events.Commands.Data
{
    [Serializable]
    public sealed class CharacterClipClear : DataCommand
    {
        public override string Key => "character_clip_clear";

        public override void Invoke(in Segment<object> parameters)
        {
            var data = EventManager.Instance.BaseDataContainer;
            data.ClearCharacterClips();
            Log();
        }
    }
}
