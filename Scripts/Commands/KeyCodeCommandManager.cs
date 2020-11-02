using System;
using System.Collections.Generic;
using UnityEngine;

namespace HegaCore
{
    public class KeyCodeCommandManager<T> : Singleton<T>, IOnUpdate
        where T : KeyCodeCommandManager<T>, new()
    {
        private readonly Dictionary<KeyCode, string> map = new Dictionary<KeyCode, string>();

        public T Register(KeyCode inputKey, string commandKey)
        {
            this.map[inputKey] = commandKey;
            return Instance;
        }

        public bool Contains(KeyCode key)
            => this.map.ContainsKey(key);

        public bool TryGetCommandKey(KeyCode inputKey, out string commandKey)
            => this.map.TryGetValue(inputKey, out commandKey);

        public T Remove(KeyCode inputKey)
        {
            this.map.Remove(inputKey);
            return Instance;
        }

        protected List<KeyCode> GetKeys()
        {
            var keys = Pool.Provider.List<KeyCode>();
            keys.AddRange(this.map.Keys);

            return keys;
        }

        protected void Return(List<KeyCode> item)
            => Pool.Provider.Return(item);

        public virtual void OnUpdate(float deltaTime) { }
    }
}
