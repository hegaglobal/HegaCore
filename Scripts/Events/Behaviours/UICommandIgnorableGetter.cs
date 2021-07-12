using System.Collections.Generic;
using UnityEngine;

namespace HegaCore.Events.Commands
{
    public class UICommandIgnorableGetter : MonoBehaviour, ICommandIgnorableGetter
    {
        private UICommand[] commands;

        private void Awake()
        {
            Find();
        }

        private void Find()
            => this.commands = GetComponentsInChildren<UICommand>().OrEmpty();

        public IEnumerable<CommandIgnorable> GetCommands()
        {
            if (this.commands == null)
                Find();

            foreach (var command in this.commands)
            {
                yield return new CommandIgnorable(command.Key, command.Ignorable);
            }
        }
    }
}