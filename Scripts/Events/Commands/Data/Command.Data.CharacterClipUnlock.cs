using System;
using System.Collections.Generic;

namespace HegaCore.Events.Commands.Data
{
    [Serializable]
    public sealed class CharacterClipUnlock : DataCommand
    {
        public override string Key => "character_clip_unlock";

        public override void Invoke(in Segment<object> parameters)
        {
            if (!ValidateParameters(parameters, 1, nameof(CharacterClipUnlock)))
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
            data.UnlockCharacterClip(new CharacterId(id, variant));
            Log(variant);
        }
    }
}
