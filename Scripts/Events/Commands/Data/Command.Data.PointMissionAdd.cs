using System;
using System.Collections.Generic;
using UnityEngine;

namespace HegaCore.Events.Commands.Data
{
    [Serializable]
    public sealed class PointMissionAdd : DataCommand
    {
        public override string Key => "point_mission_add";

        public override bool Ignorable => false;

        public override void Invoke(in Segment<object> parameters)
        {
            var data = EventManager.Instance.BaseDataContainer;

            if (ValidateParameters(parameters, 6, nameof(MissionUnlock), true))
            {
                if (!this.converter.TryConvert(parameters[0], out int missionFrom) ||
                    !this.converter.TryConvert(parameters[1], out int missionTo) ||
                    !this.converter.TryConvert(parameters[2], out int addPoint) ||
                    !this.converter.TryConvert(parameters[3], out int startPoint) ||
                    !this.converter.TryConvert(parameters[4], out int increasePoint) ||
                    !this.converter.TryConvert(parameters[5], out int constraintPoint))
                    return;

                var min = Mathf.Min(missionFrom, missionTo);
                var max = Mathf.Max(missionFrom, missionTo);
                var currentPoint = data.GetPlayerProgressPoint();

                for (var mission = min; mission <= max; mission++)
                {
                    var point = startPoint + (increasePoint * (mission - 1));
                    var maxConstraint = point + constraintPoint;

                    if (point > currentPoint)
                        break;

                    if (currentPoint > maxConstraint)
                        continue;

                    data.ChangePlayerProgressPoint(mission, addPoint);
                }

                Log(missionFrom, missionTo, addPoint, startPoint, increasePoint, constraintPoint);
                return;
            }

            if (!ValidateParameters(parameters, 2, nameof(PointMissionAdd)))
                return;

            if (!this.converter.TryConvert(parameters[0], out int missionId) ||
                !this.converter.TryConvert(parameters[1], out int value))
                return;

            if (data.ChangePlayerProgressPoint(missionId, value))
            {
                data.SavePlayer();
                Log(missionId, value);
            }
        }
    }
}
