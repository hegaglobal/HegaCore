using System.Collections.Generic;
using System.Table;
using VisualNovelData.Data;

namespace HegaCore
{
    using Database;

    public static class CharacterDataset
    {
        public static ReadCharacterData Data { get; private set; }

        public static ReadTable<CharacterEntry> Table { get; private set; }

        public static ReadDictionary<string, int> Map { get; private set; }

        public static void Initialize(in ReadCharacterData data, in ReadTable<CharacterEntry> table, in ReadDictionary<string, int> map)
        {
            Data = data;
            Table = table;
            Map = map;
        }
    }
}