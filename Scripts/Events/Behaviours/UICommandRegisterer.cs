using System.Collections.Generic;

namespace HegaCore.Events.Commands
{
    public sealed class UICommandRegisterer : CommandRegisterer
    {
        protected override IEnumerable<ICommand> GetCommands()
        {
            return GetComponentsInChildren<UICommand>();
        }
    }
}