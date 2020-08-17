using System.Collections.Generic;

namespace HegaCore.Commands
{
    public sealed class UICommandRegisterer : CommandRegisterer
    {
        protected override IEnumerable<ICommand> GetCommands()
        {
            return GetComponentsInChildren<UICommand>();
        }
    }
}