using System;
using System.Collections.Generic;

namespace HegaCore
{
    public class CommandManager : Singleton<CommandManager>
    {
        private readonly Dictionary<string, ICommand> map = new Dictionary<string, ICommand>();

        public CommandManager Register(string key, ICommand command)
        {
            if (command != null)
                this.map[key] = command;

            return this;
        }

        public CommandManager Register<T>(string key) where T : ICommand, new()
        {
            this.map[key] = new T();
            return this;
        }

        public bool Contains(string key)
            => this.map.ContainsKey(key);

        public bool TryGetCommand(string commandKey, out ICommand command)
            => this.map.TryGetValue(commandKey, out command);

        public CommandManager Remove(string key)
        {
            this.map.Remove(key);
            return this;
        }

        public void Invoke(string commandKey)
        {
            if (TryGetCommand(commandKey, out var command))
            {
                command.PreExecute();
                command.Execute();
                command.PostExecute();
            }
        }
    }
}
