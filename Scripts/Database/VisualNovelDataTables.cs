using System.Collections.Generic;
using System.Table;
using VisualNovelData.Data;

namespace HegaCore
{
    using Database;

    public class VisualNovelDataTables : Tables
    {
        public Table<LanguageEntry> Language { get; } = new Table<LanguageEntry>();

        public Table<AudioEntry> Audio { get; } = new Table<AudioEntry>();

        public L10nData L10nData { get; } = new L10nData();

        public NovelData NovelData { get; } = new NovelData();

        public CharacterData CharacterData { get; } = new CharacterData();

        public EventData EventData { get; } = new EventData();

        public Table<CharacterEntry> Character { get; } = new Table<CharacterEntry>();

        public ReadDictionary<string, int> CharacterMap => this.characterMap;

        private readonly Dictionary<string, int> characterMap;

        public VisualNovelDataTables()
        {
            this.characterMap = new Dictionary<string, int>();
        }

        public override void Clear()
        {
            this.Language.Clear();
            this.Audio.Clear();
            this.L10nData.Clear();
            this.NovelData.Clear();
            this.CharacterData.Clear();
            this.EventData.Clear();
            this.Character.Clear();
        }

        public void PrepareCharacterMap()
        {
            this.characterMap.Clear();

            foreach (var entry in this.Character.Entries)
            {
                if (this.characterMap.ContainsKey(entry.Name))
                    continue;

                this.characterMap[entry.Name] = entry.Id;
            }
        }
    }
}