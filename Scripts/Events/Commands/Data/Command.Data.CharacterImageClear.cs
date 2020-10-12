using System;
using System.Collections.Generic;

namespace HegaCore.Events.Commands.Data
{
    [Serializable]
    public sealed class CharacterImageClear : DataCommand
    {
        public override string Key => "character_image_clear";

        public override void Invoke(in Segment<object> parameters)
        {
            var data = EventManager.Instance.BaseDataContainer;
            data.ClearCharacterImages();
            Log();
        }
    }
}
