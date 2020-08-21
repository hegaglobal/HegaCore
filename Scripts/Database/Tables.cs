using System.Table;
using VisualNovelData.Data;

namespace HegaCore
{
    using Database;

    public abstract class Tables
    {
        public Table<LanguageEntry> Language { get; } = new Table<LanguageEntry>();

        public L10nData L10nData { get; } = new L10nData();

        public NovelData NovelData { get; } = new NovelData();

        public CharacterData CharacterData { get; } = new CharacterData();

        public EventData EventData { get; } = new EventData();

        public virtual void Clear()
        {
            this.Language.Clear();
            this.L10nData.Clear();
            this.NovelData.Clear();
            this.CharacterData.Clear();
            this.EventData.Clear();
        }
    }
}