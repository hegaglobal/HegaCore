using System;
using System.Collections.Generic;

namespace HegaCore.Events.Commands.Data
{
    [Serializable]
    public sealed class PrefabHide : DataCommand
    {
        public override string Key => "prefab_hide";

        public override void Invoke(in Segment<object> parameters)
        {
            if (!ValidateParameters(parameters, 1, nameof(PrefabHide)))
                return;

            if (!this.converter.TryConvert(parameters[0], out string value))
                return;

            Log(value);
        }
    }
}
