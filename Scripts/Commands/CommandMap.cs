using System.Collections.Generic;

namespace HegaCore
{
    public class CommandMap : ICommandMap
    {
        private readonly Dictionary<string, ICommand> map = new Dictionary<string, ICommand>();

        public void Register(string key, ICommand command)
        {
            if (command != null)
                this.map[key] = command;
        }

        public void Register<T>(string key) where T : ICommand, new()
        {
            this.map[key] = new T();
        }

        public bool Contains(string key)
            => this.map.ContainsKey(key);

        public bool TryGetCommand(string commandKey, out ICommand command)
            => this.map.TryGetValue(commandKey, out command);

        public void Remove(string key)
        {
            this.map.Remove(key);
        }

        public void Remove(params string[] keys)
        {
            foreach (var key in keys)
            {
                this.map.Remove(key);
            }
        }

        public void Remove(IEnumerable<string> keys)
        {
            foreach (var key in keys)
            {
                this.map.Remove(key);
            }
        }

        public static CommandMap Default { get; } = new CommandMap();
    }
}
