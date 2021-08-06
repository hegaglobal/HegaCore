using System;
using System.Collections.Generic;

namespace HegaCore.Events.Commands.Data
{
    [Serializable]
    public sealed class PrefabHideAll : DataCommand
    {
        public override string Key => "prefab_hide_all";

        public override bool Ignorable => false;

        public override void Invoke(in Segment<object> parameters)
        {
            Log();
        }
    }
}
