using System.Collections.Generic;

namespace HegaCore.Commands
{
    public abstract class CommandRegisterer : OnAwakeRegisterer
    {
        protected override void Register(bool @override)
        {
            if (UnityEngine.SingletonBehaviour.Quitting)
                return;

            Register(GetCommands(), @override);
        }

        protected void Register(IEnumerable<ICommand> commands, bool @override)
        {
            if (commands == null)
                return;

            var system = EventManager.Instance.CommandSystem;

            foreach (var command in commands)
            {
                system.Register(command.Key, command, @override);
            }
        }

        protected virtual IEnumerable<ICommand> GetCommands()
            => null;
    }
}