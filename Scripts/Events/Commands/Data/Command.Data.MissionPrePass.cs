using System;
using System.Collections.Generic;

namespace HegaCore.Events.Commands.Data
{
    [Serializable]
    public sealed class MissionPrePass : DataCommand
    {
        public override string Key => "mission_pre_pass";

        public override void Invoke(in Segment<object> parameters)
        {
            if (!ValidateParameters(parameters, 1, nameof(MissionPrePass)))
                return;

            if (!this.converter.TryConvert(parameters[0], out int value))
                return;

            var data = EventManager.Instance.BaseDataContainer;

            if (data.PrePassMission(value))
                Log(value);
        }
    }
}
