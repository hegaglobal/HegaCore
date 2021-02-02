using System.Collections.Generic;
using System.Table;
using VisualNovelData.Data;

namespace HegaCore
{
    using System.Collections.Pooling;
    using Database;

    public class VisualNovelDataTables : Tables
    {
        public Table<GameModeEntry> GameMode { get; } = new Table<GameModeEntry>();

        public Table<LanguageEntry> Language { get; } = new Table<LanguageEntry>();

        public Table<AudioEntry> Audio { get; } = new Table<AudioEntry>();

        public L10nData L10nData { get; } = new L10nData();

        public NovelData NovelData { get; } = new NovelData();

        public CharacterData CharacterData { get; } = new CharacterData();

        public EventData EventData { get; } = new EventData();

        public Table<CharacterEntry> Character { get; } = new Table<CharacterEntry>();

        public ReadDictionary<string, int> CharacterMap => this.characterMap;

        public ReadDictionary<AudioType, List<KeyValuePair<string, string>>> AudioMap => this.audioMap;

        private readonly Dictionary<string, int> characterMap;
        private readonly Dictionary<AudioType, List<KeyValuePair<string, string>>> audioMap;

        public VisualNovelDataTables()
        {
            this.characterMap = new Dictionary<string, int>();
            this.audioMap = new Dictionary<AudioType, List<KeyValuePair<string, string>>>();
        }

        public override void Clear()
        {
            this.GameMode.Clear();
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

        public void PrepareAudioMap()
        {
            foreach (var value in this.audioMap.Values)
            {
                Pool.Provider.Return(value);
            }

            this.audioMap.Clear();

            foreach (var entry in this.Audio.Entries)
            {
                if (!this.audioMap.TryGetValue(entry.Type, out var map))
                {
                    map = Pool.Provider.List<KeyValuePair<string, string>>();
                    this.audioMap[entry.Type] = map;
                }

                map.Add(new KeyValuePair<string, string>(entry.Key, entry.AssetKey));
            }
        }
    }
}