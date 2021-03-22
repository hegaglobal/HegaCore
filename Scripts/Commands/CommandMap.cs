using System.Collections.Generic;

namespace HegaCore
{
    public class CommandMap : ICommandMap
    {
        private readonly Dictionary<string, ICommand> map = new Dictionary<string, ICommand>();

        public void Register(string id, ICommand command)
        {
            if (command != null)
                this.map[id] = command;
        }

        public void Register<T>(string id) where T : ICommand, new()
        {
            this.map[id] = new T();
        }

        public bool Contains(string id)
            => this.map.ContainsKey(id);

        public bool TryGetCommand(string id, out ICommand command)
            => this.map.TryGetValue(id, out command);

        public void Remove(string id)
        {
            this.map.Remove(id);
        }

        public void Remove(params string[] ids)
        {
            foreach (var id in ids)
            {
                this.map.Remove(id);
            }
        }

        public void Remove(IEnumerable<string> ids)
        {
            foreach (var id in ids)
            {
                this.map.Remove(id);
            }
        }

        public static CommandMap Default { get; } = new CommandMap();
    }
}
