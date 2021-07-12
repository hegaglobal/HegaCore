﻿using System;
using System.Collections.Generic;

namespace HegaCore.Events.Commands.Data
{
    [Serializable]
    public sealed class CharacterProgressUnlock : DataCommand
    {
        public override string Key => "char_progress_unlock";

        public override bool Ignorable => false;

        public override void Invoke(in Segment<object> parameters)
        {
            if (!ValidateParameters(parameters, 2, nameof(CharacterProgressUnlock)))
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

            if (data.UnlockPlayerCharacterProgress(id, value))
            {
                data.SavePlayer();
                Log(id, value);
            }
        }
    }
}
