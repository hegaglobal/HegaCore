using System;
using System.Collections.Generic;

namespace HegaCore
{
    public abstract class CommandManager<TKey, TManager> : Singleton<TManager>, IOnUpdate
        where TKey : unmanaged
        where TManager : CommandManager<TKey, TManager>, new()
    {
        private readonly TManager mananger;
        private readonly Dictionary<TKey, ICommand> commands;

        public CommandManager()
        {
            this.mananger = this as TManager;
            this.commands = new Dictionary<TKey, ICommand>();
        }

        public TManager Register(TKey key, ICommand command)
        {
            if (command != null)
                this.commands[key] = command;

            return this.mananger;
        }

        public TManager Register<T>(TKey key) where T : ICommand, new()
        {
            this.commands[key] = new T();
            return this.mananger;
        }

        public TManager Remove(TKey key)
        {
            this.commands.Remove(key);
            return this.mananger;
        }

        public void OnUpdate(float deltaTime)
        {
            var keys = Pool.Provider.List<TKey>();
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
