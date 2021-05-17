using System;
using System.Collections.Generic;
using UnityEngine;

namespace HegaCore.Events.Commands.Data
{
    [Serializable]
    public sealed class MissionUnlock : DataCommand
    {
        public override string Key => "mission_unlock";

        public override void Invoke(in Segment<object> parameters)
        {
            var data = EventManager.Instance.BaseDataContainer;

            if (ValidateParameters(parameters, 4, nameof(MissionUnlock), true))
            {
                if (!this.converter.TryConvert(parameters[0], out int missionFrom) ||
                    !this.converter.TryConvert(parameters[1], out int missionTo) ||
                    !this.converter.TryConvert(parameters[2], out int startPoint) ||
                    !this.converter.TryConvert(parameters[3], out int increasePoint))
                    return;

                var min = Mathf.Min(missionFrom, missionTo);
                var max = Mathf.Max(missionFrom, missionTo);
                var currentPoint = data.GetPlayerProgressPoint();

                for (var mission = min; mission <= max; mission++)
                {
                    var point = startPoint + (increasePoint * (mission - 1));

                    if (point > currentPoint)
                        break;

                    data.UnlockMission(mission);
                }

                Log(missionFrom, missionTo, startPoint, increasePoint);
                return;
            }

            if (!ValidateParameters(parameters, 1, nameof(MissionUnlock)))
                return;

            if (!this.converter.TryConvert(parameters[0], out int value))
                return;

            if (data.UnlockMission(value))
                Log(value);
        }
    }
}
