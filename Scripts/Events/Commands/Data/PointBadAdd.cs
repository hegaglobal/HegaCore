using System;
using System.Collections.Generic;

namespace HegaCore.Commands.Data
{
    [Serializable]
    public sealed class PointBadAdd : DataCommand
    {
        public override string Key => "point_bad_add";

        public override void Invoke(in Segment<object> parameters)
        {
            if (!ValidateParameters(parameters, 1, nameof(PointBadAdd)))
                return;

            if (!this.converter.TryConvert(parameters[0], out int value))
                return;

            var data = EventManager.Instance.BaseDataContainer;
            data.ChangePlayerBadPoint(value);
            data.SavePlayer();
            Log(value);
        }
    }
}
