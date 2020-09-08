using System;
using System.Collections.Generic;

namespace HegaCore.Commands.Data
{
    [Serializable]
    public sealed class MissionUnlock : DataCommand
    {
        public override string Key => "mission_unlock";

        public override void Invoke(in Segment<object> parameters)
        {
            if (!ValidateParameters(parameters, 1, nameof(MissionUnlock)))
                return;

            if (!this.converter.TryConvert(parameters[0], out int value))
                return;

            var data = EventManager.Instance.BaseDataContainer;
            data.UnlockMission(value);
            Log(value);
        }
    }
}
