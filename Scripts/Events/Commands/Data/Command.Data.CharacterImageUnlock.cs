using System;
using System.Collections.Generic;

namespace HegaCore.Events.Commands.Data
{
    [Serializable]
    public sealed class CharacterImageUnlock : DataCommand
    {
        public override string Key => "char_image_unlock";

        public override void Invoke(in Segment<object> parameters)
        {
            if (!ValidateParameters(parameters, 2, nameof(CharacterImageUnlock)))
                return;

            if (!this.converter.TryConvert(parameters[0], out string character))
                return;

            if (!this.converter.TryConvert(parameters[1], out int variant))
                return;

            if (!CharacterDataset.Map.TryGetValue(character, out var id))
            {
                UnuLogger.LogWarning($"Cannot find any character id by key={character}");
                return;
            }

            var data = EventManager.Instance.BaseDataContainer;

            if (data.UnlockCharacterImage(new CharacterId(id, variant)))
                Log(id, variant);
        }
    }
}
