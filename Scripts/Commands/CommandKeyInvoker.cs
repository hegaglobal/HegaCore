using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HegaCore
{
    public abstract class CommandKeyInvoker<T> : Singleton<T>, IOnUpdate
        where T : CommandKeyInvoker<T>, new()
    {
        private readonly Dictionary<KeyCode, string> map = new Dictionary<KeyCode, string>();
        private readonly HashSet<KeyCode> executed = new HashSet<KeyCode>();

        public T Register(KeyCode inputKey, string commandKey)
        {
            this.map[inputKey] = commandKey;
            return Instance;
        }

        public T Register(string commandKey, params KeyCode[] inputKeys)
        {
            foreach (var inputKey in inputKeys)
            {
                this.map[inputKey] = commandKey;
            }

            return Instance;
        }

        public bool Contains(KeyCode inputKey)
            => this.map.ContainsKey(inputKey);

        public bool Contains(string commandKey)
            => this.map.ContainsValue(commandKey);

        public bool TryGetCommandKey(KeyCode inputKey, out string commandKey)
            => this.map.TryGetValue(inputKey, out commandKey);

        public T Remove(KeyCode inputKey)
        {
            this.map.Remove(inputKey);
            return Instance;
        }

        public T Remove(params KeyCode[] inputKeys)
        {
            foreach (var key in inputKeys)
            {
                this.map.Remove(key);
            }

            return Instance;
        }

        public T Remove(string commandKey)
        {
            var query = this.map.Where(x => string.Equals(x.Value, commandKey)).Select(x => x.Key);
            var keys = Pool.Provider.List<KeyCode>();
            keys.AddRange(query);

            foreach (var key in keys)
            {
                this.map.Remove(key);
            }

            Pool.Provider.Return(keys);

            return Instance;
        }

        public T Remove(params string[] commandKeys)
        {
            foreach (var commandKey in commandKeys)
            {
                Remove(commandKey);
            }

            return Instance;
        }

        public void OnUpdate(float deltaTime)
        {
            var keys = Pool.Provider.List<KeyCode>();
            keys.AddRange(this.map.Keys);

            OnUpdate(deltaTime, this.executed, keys);

            Pool.Provider.Return(keys);
        }

        protected abstract void OnUpdate(float deltaTime, ISet<KeyCode> executed, IEnumerable<KeyCode> keys);
    }
}
