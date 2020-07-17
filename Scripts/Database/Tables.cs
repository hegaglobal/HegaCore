using System.Table;
using VisualNovelData.Data;

namespace HegaCore
{
    using Database;

    public abstract class Tables
    {
        public Table<LanguageEntry> Language { get; }

        public L10nData L10nData { get; }

        public NovelData NovelData { get; }

        public CharacterData CharacterData { get; }

        public QuestData QuestData { get; }

        public Tables()
        {
            this.Language = new Table<LanguageEntry>();
            this.L10nData = new L10nData();
            this.NovelData = new NovelData();
            this.CharacterData = new CharacterData();
            this.QuestData = new QuestData();
        }

        public virtual void Clear()
        {
            this.Language.Clear();
            this.L10nData.Clear();
            this.NovelData.Clear();
            this.CharacterData.Clear();
            this.QuestData.Clear();
        }
    }
}