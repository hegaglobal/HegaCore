using VisualNovelData.Data;
using VisualNovelData.Parser;

namespace HegaCore
{
    using Database;

    public abstract class VisualNovelDatabase<TDatabase, TTables> : Database<TDatabase, TTables>
        where TDatabase : VisualNovelDatabase<TDatabase, TTables>
        where TTables : VisualNovelDataTables, new()
    {
        protected override void ContinueLoad()
        {
            base.ContinueLoad();

            Load<GameModeEntry, GameModeEntry.Mapping>
                 (this.Tables.GameMode, nameof(this.Tables.GameMode), true);

            Load<LanguageEntry, LanguageEntry.Mapping>
                 (this.Tables.Language, nameof(this.Tables.Language), true);

            Load<AudioEntry, AudioEntry.Mapping>
                 (this.Tables.Audio, nameof(this.Tables.Audio), true);

            PrepareLanguages();

            Load<L10nData, L10nParser>
                 (this.Tables.L10nData, nameof(this.Tables.L10nData), this.Languages);

            Load<NovelData, NovelParser>
                 (this.Tables.NovelData, nameof(this.Tables.NovelData), this.Languages);

            Load<CharacterData, CharacterParser>
                 (this.Tables.CharacterData, nameof(this.Tables.CharacterData), this.Languages);

            Load<EventData, EventParser>
                 (this.Tables.EventData, nameof(this.Tables.EventData), this.Languages);

            Load<CharacterEntry, CharacterEntry.Mapping, CharacterEntry.IdGetter>
                (this.Tables.Character, nameof(this.Tables.Character));

            this.Tables.PrepareCharacterMap();

            VisualNovelDataset.Initialize(this.Tables.CharacterData, this.Tables.NovelData);
            CharacterDataset.Initialize(this.Tables.CharacterData, this.Tables.Character, this.Tables.CharacterMap);
        }

        private void PrepareLanguages()
        {
            ClearLanguages();

            foreach (var entry in this.Tables.Language.Entries)
            {
                AddLanguage(entry.Key, entry.IsUsed);
            }
        }
    }
}