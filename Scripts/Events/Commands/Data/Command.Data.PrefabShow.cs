using System;
using System.Collections.Generic;

namespace HegaCore.Events.Commands.Data
{
    [Serializable]
    public sealed class PrefabShow : DataCommand
    {
        public override string Key => "prefab_show";

        public override void Invoke(in Segment<object> parameters)
        {
            if (!ValidateParameters(parameters, 1, nameof(PrefabShow)))
                return;

            if (!this.converter.TryConvert(parameters[0], out string value))
                return;

            Log(value);
        }
    }
}
