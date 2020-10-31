using System;
using System.Collections.Generic;

namespace HegaCore.Events.Commands.Data
{
    [Serializable]
    public sealed class PointAdd : DataCommand
    {
        public override string Key => "point_add";

        public override void Invoke(in Segment<object> parameters)
        {
            if (!ValidateParameters(parameters, 1, nameof(PointAdd)))
                return;

            if (!this.converter.TryConvert(parameters[0], out int value))
                return;

            var data = EventManager.Instance.BaseDataContainer;

            if (data.ChangePlayerProgressPoint(value))
            {
                data.SavePlayer();
                Log(value);
            }
        }
    }
}
