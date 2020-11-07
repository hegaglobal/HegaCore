using System;
using System.Collections.Generic;

namespace HegaCore.Events.Commands.Data
{
    [Serializable]
    public sealed class MissionPrePastClear : DataCommand
    {
        public override string Key => "mission_pre_past_clear";

        public override void Invoke(in Segment<object> parameters)
        {
            var data = EventManager.Instance.BaseDataContainer;
            data.ClearPrePastMissions();
            Log();
        }
    }
}
