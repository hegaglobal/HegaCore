using System;
using System.Collections.Generic;

namespace HegaCore.Events.Commands.Data
{
    [Serializable]
    public sealed class CharacterProgressSet : DataCommand
    {
        public override string Key => "character_progress_set";

        public override void Invoke(in Segment<object> parameters)
        {
            if (!ValidateParameters(parameters, 2, nameof(CharacterProgressSet)))
                return;

            if (!this.converter.TryConvert(parameters[0], out string character))
                return;

            if (!this.converter.TryConvert(parameters[1], out int value))
                return;

            if (!CharacterDataset.Map.TryGetValue(character, out var id))
            {
                UnuLogger.LogWarning($"Cannot find any character id by key={character}");
                return;
            }

            var data = EventManager.Instance.BaseDataContainer;

            if (data.SetPlayerCharacterProgress(id, value))
            {
                data.SavePlayer();
                Log(id, value);
            }
        }
    }
}
