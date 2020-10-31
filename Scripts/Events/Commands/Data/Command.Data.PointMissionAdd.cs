using System;
using System.Collections.Generic;

namespace HegaCore.Events.Commands.Data
{
    [Serializable]
    public sealed class PointMissionAdd : DataCommand
    {
        public override string Key => "point_mission_add";

        public override void Invoke(in Segment<object> parameters)
        {
            if (!ValidateParameters(parameters, 2, nameof(PointMissionAdd)))
                return;

            if (!this.converter.TryConvert(parameters[0], out int missionId) ||
                !this.converter.TryConvert(parameters[1], out int value))
                return;

            var data = EventManager.Instance.BaseDataContainer;

            if (data.ChangePlayerProgressPoint(missionId, value))
            {
                data.SavePlayer();
                Log(missionId, value);
            }
        }
    }
}
