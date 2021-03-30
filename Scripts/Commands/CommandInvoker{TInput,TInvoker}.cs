using System;
using System.Collections.Generic;
using System.Collections.Pooling;
using System.Linq;

namespace HegaCore
{
    public abstract class CommandInvoker<TInput> : IOnUpdate
    {
        private readonly Dictionary<TInput, string> map = new Dictionary<TInput, string>();
        private readonly HashSet<TInput> executed = new HashSet<TInput>();
        private readonly IReadOnlyCommandMap commandMap;

        protected CommandInvoker(IReadOnlyCommandMap commandMap)
        {
            this.commandMap = commandMap ?? throw new ArgumentNullException(nameof(commandMap));
        }

        public void Register(TInput input, string commandId)
        {
            this.map[input] = commandId;
        }

        public void Register(IEnumerable<TInput> inputs, string commandId)
        {
            foreach (var input in inputs)
            {
                this.map[input] = commandId;
            }
        }

        public void Register(string commandId, params TInput[] inputs)
        {
            foreach (var input in inputs)
            {
                this.map[input] = commandId;
            }
        }

        public bool ContainsKey(TInput input)
            => this.map.ContainsKey(input);

        public bool ContainsCommand(string commandId)
            => this.map.ContainsValue(commandId);

        public bool TryGetCommand(TInput input, out string commandId)
            => this.map.TryGetValue(input, out commandId);

        public void Remove(TInput input)
        {
            this.map.Remove(input);
        }

        public void Remove(IEnumerable<TInput> inputs)
        {
            foreach (var key in inputs)
            {
                this.map.Remove(key);
            }
        }

        public void Remove(params TInput[] inputs)
        {
            foreach (var key in inputs)
            {
                this.map.Remove(key);
            }
        }

        public void RemoveCommand(string commandId)
        {
            var query = this.map.Where(x => string.Equals(x.Value, commandId)).Select(x => x.Key);
            var keys = Pool.Provider.List<TInput>();
            keys.AddRange(query);

            foreach (var key in keys)
            {
                this.map.Remove(key);
            }

            Pool.Provider.Return(keys);
        }

        public void RemoveCommand(params string[] commandIds)
        {
            foreach (var commandId in commandIds)
            {
                RemoveCommand(commandId);
            }
        }

        public void RemoveCommand(IEnumerable<string> commandIds)
        {
            foreach (var commandId in commandIds)
            {
                RemoveCommand(commandId);
            }
        }

        public void OnUpdate(float deltaTime)
        {
            OnPreUpdate(deltaTime);

            var inputs = Pool.Provider.List<TInput>();
            inputs.AddRange(this.map.Keys);

            foreach (var input in inputs)
            {
                if (TryGetCommand(input, out var commandId))
                {
                    if (CanInvoke(input))
                    {
                        executed.Add(input);

                        if (this.commandMap.TryGetCommand(commandId, out var command))
                            command.Execute();
                    }
                    else if (executed.Contains(input))
                    {
                        executed.Remove(input);

                        if (this.commandMap.TryGetCommand(commandId, out var command))
                            command.Deactivate();
                    }
                }
            }

            Pool.Provider.Return(inputs);

            OnPostUpdate(deltaTime);
        }

        protected virtual void OnPreUpdate(float deltaTime) { }

        protected virtual void OnPostUpdate(float deltaTime) { }

        protected abstract bool CanInvoke(TInput input);
    }
}