using System;
using System.Collections.Generic;
using System.Collections.Pooling;
using System.Linq;

namespace HegaCore
{
    public abstract class CommandInvoker<TInputKey> : IOnUpdate
    {
        private readonly Dictionary<TInputKey, string> map = new Dictionary<TInputKey, string>();
        private readonly HashSet<TInputKey> executed = new HashSet<TInputKey>();
        private readonly IReadOnlyCommandMap commandMap;

        public CommandInvoker(IReadOnlyCommandMap commandMap)
        {
            this.commandMap = commandMap ?? throw new ArgumentNullException(nameof(commandMap));
        }

        public void Register(TInputKey inputKey, string commandKey)
        {
            this.map[inputKey] = commandKey;
        }

        public void Register(IEnumerable<TInputKey> inputKeys, string commandKey)
        {
            foreach (var inputKey in inputKeys)
            {
                this.map[inputKey] = commandKey;
            }
        }

        public void Register(string commandKey, params TInputKey[] inputKeys)
        {
            foreach (var inputKey in inputKeys)
            {
                this.map[inputKey] = commandKey;
            }
        }

        public bool ContainsKey(TInputKey inputKey)
            => this.map.ContainsKey(inputKey);

        public bool ContainsCommand(string commandKey)
            => this.map.ContainsValue(commandKey);

        public bool TryGetCommand(TInputKey inputKey, out string commandKey)
            => this.map.TryGetValue(inputKey, out commandKey);

        public void Remove(TInputKey inputKey)
        {
            this.map.Remove(inputKey);
        }

        public void Remove(IEnumerable<TInputKey> inputKeys)
        {
            foreach (var key in inputKeys)
            {
                this.map.Remove(key);
            }
        }

        public void Remove(params TInputKey[] inputKeys)
        {
            foreach (var key in inputKeys)
            {
                this.map.Remove(key);
            }
        }

        public void RemoveCommand(string commandKey)
        {
            var query = this.map.Where(x => string.Equals(x.Value, commandKey)).Select(x => x.Key);
            var keys = Pool.Provider.List<TInputKey>();
            keys.AddRange(query);

            foreach (var key in keys)
            {
                this.map.Remove(key);
            }

            Pool.Provider.Return(keys);
        }

        public void RemoveCommand(params string[] commandKeys)
        {
            foreach (var commandKey in commandKeys)
            {
                RemoveCommand(commandKey);
            }
        }

        public void RemoveCommand(IEnumerable<string> commandKeys)
        {
            foreach (var commandKey in commandKeys)
            {
                RemoveCommand(commandKey);
            }
        }

        public void OnUpdate(float deltaTime)
        {
            var inputKeys = Pool.Provider.List<TInputKey>();
            inputKeys.AddRange(this.map.Keys);

            foreach (var inputKey in inputKeys)
            {
                if (TryGetCommand(inputKey, out var commandKey))
                {
                    if (CanInvoke(inputKey))
                    {
                        executed.Add(inputKey);

                        if (this.commandMap.TryGetCommand(commandKey, out var command))
                            command.Execute();
                    }
                    else if (executed.Contains(inputKey))
                    {
                        executed.Remove(inputKey);

                        if (this.commandMap.TryGetCommand(commandKey, out var command))
                            command.Deactivate();
                    }
                }
            }

            Pool.Provider.Return(inputKeys);
        }

        protected abstract bool CanInvoke(TInputKey inputKey);
    }
}