using System;
using System.Collections.Generic;

namespace HegaCore.Events.Commands.Data
{
    [Serializable]
    public sealed class MissionPassedClear : DataCommand
    {
        public override string Key => "mission_passed_clear";

        public override void Invoke(in Segment<object> parameters)
        {
            var data = EventManager.Instance.BaseDataContainer;
            data.ClearPassedMissions();
            Log();
        }
    }
}
