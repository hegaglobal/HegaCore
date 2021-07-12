using System;
using System.Collections.Generic;

namespace HegaCore.Events.Commands.Data
{
    [Serializable]
    public sealed class CharacterImageClear : DataCommand
    {
        public override string Key => "char_image_clear";

        public override bool Ignorable => false;

        public override void Invoke(in Segment<object> parameters)
        {
            var data = EventManager.Instance.BaseDataContainer;
            data.ClearCharacterImages();
            Log();
        }
    }
}
