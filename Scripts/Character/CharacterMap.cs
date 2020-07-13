using System;
using System.Collections.Generic;

namespace HegaCore
{
    public sealed class CharacterMap : Singleton<CharacterMap>
    {
        private readonly Dictionary<string, int> map = new Dictionary<string, int>();

        public int Total
            => this.map.Count;

        public void Register(string key, int value)
        {
            if (value < 0)
                return;

            if (string.IsNullOrEmpty(key) && value != 0)
                throw new InvalidOperationException("Character key empty is reserved.");

            if (!string.IsNullOrEmpty(key) && value == 0)
                throw new InvalidOperationException("Character value 0 is reserved.");

            this.map[key] = value;
        }

        public int GetValue (string key)
        {
            if (string.IsNullOrEmpty(key))
                return 0;

            if (this.map.ContainsKey(key))
                return this.map[key];

            UnuLogger.LogWarning($"Cannot find character with key {key}");
            return -1;
        }
    }
}