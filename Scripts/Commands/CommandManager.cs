using System;
using System.Collections.Generic;

namespace HegaCore
{
    public class CommandManager : Singleton<CommandManager>, IOnUpdate
    {
        private readonly Dictionary<int, ICommand> commands = new Dictionary<int, ICommand>();

        public CommandManager Register(int key, ICommand command)
        {
            if (command != null)
                this.commands[key] = command;

            return this;
        }

        public CommandManager Register<T>(int key) where T : ICommand, new()
        {
            this.commands[key] = new T();
            return this;
        }

        public CommandManager Remove(int key)
        {
            this.commands.Remove(key);
            return this;
        }

        public void OnUpdate(float deltaTime)
        {
            var keys = Pool.Provider.List<int>();
            keys.AddRange(this.commands.Keys);

            foreach (var key in keys)
            {
                if (!this.commands.TryGetValue(key, out var command))
                    continue;

                if (command.Validate())
                {
                    command.PreExecute();
                    command.Execute();
                    command.PostExecute();
                }
            }

            Pool.Provider.Return(keys);
        }
    }
}
