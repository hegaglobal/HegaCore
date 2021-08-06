using System.Collections.Generic;

namespace HegaCore.Events.Commands
{
    public interface ICommandIgnorableGetter
    {
        IEnumerable<CommandIgnorable> GetCommands();
    }
}
