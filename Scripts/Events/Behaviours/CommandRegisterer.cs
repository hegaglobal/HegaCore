using System.Collections.Generic;

namespace HegaCore.Events.Commands
{
    public abstract class CommandRegisterer : OnAwakeRegisterer
    {
        protected override void Register(bool @override)
        {
            if (UnityEngine.SingletonBehaviour.Quitting)
                return;

            Register(GetCommands(), @override);
        }

        protected void Register(IEnumerable<IEventCommand> commands, bool @override)
        {
            if (commands == null)
                return;

            var system = EventManager.Instance.CommandSystem;

            foreach (var command in commands)
            {
                system.Register(command.Key, command, @override);
            }
        }

        protected virtual IEnumerable<IEventCommand> GetCommands()
            => null;
    }
}