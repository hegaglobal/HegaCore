using System;
using System.Collections.Generic;

namespace HegaCore.Events.Commands.Data
{
    [Serializable]
    public sealed class PrefabDestroyAll : DataCommand
    {
        public override string Key => "prefab_destroy_all";

        public override bool Ignorable => true;

        public override void Invoke(in Segment<object> parameters)
        {
            Log();
        }
    }
}
