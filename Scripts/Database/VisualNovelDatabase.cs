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
            Load<LanguageEntry, LanguageEntry.Mapping>
                 (this.Tables.Language, nameof(this.Tables.Language), true);

            PrepareLanguages();

            Load<L10nData, L10nParser>
                 (this.Tables.L10nData, nameof(this.Tables.L10nData), this.Languages);

            Load<NovelData, NovelParser>
                 (this.Tables.NovelData, nameof(this.Tables.NovelData), this.Languages);

            Load<CharacterData, CharacterParser>
                 (this.Tables.CharacterData, nameof(this.Tables.CharacterData), this.Languages);

            Load<EventData, EventParser>
                 (this.Tables.EventData, nameof(this.Tables.EventData), this.Languages);
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