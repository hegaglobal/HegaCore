using System.Collections.Generic;

namespace HegaCore.Events.Commands
{
    public sealed class UICommandRegisterer : CommandRegisterer
    {
        protected override IEnumerable<IEventCommand> GetCommands()
        {
            return GetComponentsInChildren<UICommand>();
        }
    }
}