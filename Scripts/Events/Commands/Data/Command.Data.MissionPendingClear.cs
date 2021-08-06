using System;
using System.Collections.Generic;

namespace HegaCore.Events.Commands.Data
{
    [Serializable]
    public sealed class MissionPendingClear : DataCommand
    {
        public override string Key => "mission_pending_clear";

        public override bool Ignorable => false;

        public override void Invoke(in Segment<object> parameters)
        {
            var data = EventManager.Instance.BaseDataContainer;
            data.ClearPendingMissions();
            Log();
        }
    }
}
