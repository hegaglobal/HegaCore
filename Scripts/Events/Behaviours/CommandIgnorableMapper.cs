using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HegaCore.Events.Commands
{
    public sealed class CommandIgnorableMapper : MonoBehaviour, IReadOnlyDictionary<string, bool>
    {
        public IEnumerable<string> Keys => this.map.Keys;

        public IEnumerable<bool> Values => this.map.Values;

        public int Count => this.map.Count;

        public bool this[string key] => this.map[key];

        private readonly Dictionary<string, bool> map = new Dictionary<string, bool>();

        private void Awake()
        {
            for (var i = 0; i < CoreDataCommands.Commands.Length; i++)
            {
                var command = CoreDataCommands.Commands[i];
                this.map[command.Key] = command.Ignorable;
            }

            var getter = GetComponentsInChildren<ICommandIgnorableGetter>().OrEmpty();

            for (var i = 0; i < getter.Length; i++)
            {
                var commands = getter[i]?.GetCommands();

                if (commands == null)
                    continue;

                foreach (var command in commands)
                {
                    this.map[command.Key] = command.Ignorable;
                }
            }
        }

        public bool CanIgnore(string key)
            => this.map.TryGetValue(key, out var ignorable) && ignorable;

        public bool ContainsKey(string key)
            => this.map.ContainsKey(key);

        public bool TryGetValue(string key, out bool value)
            => this.map.TryGetValue(key, out value);

        public Dictionary<string, bool>.Enumerator GetEnumerator()
            => this.map.GetEnumerator();

        IEnumerator<KeyValuePair<string, bool>> IEnumerable<KeyValuePair<string, bool>>.GetEnumerator()
            => this.map.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => this.map.GetEnumerator();
    }
}