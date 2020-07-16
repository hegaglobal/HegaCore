using System.Collections.Generic;
using System.Table;
using UnityEngine;
using Cysharp.Threading.Tasks;
using VisualNovelData.Data;
using VisualNovelData.Parser;

namespace HegaCore
{
    public abstract partial class Database<T> : SingletonBehaviour<T> where T : Database<T>
    {
        [SerializeField]
        private DatabaseConfig config = null;

        public bool Initialized { get; private set; }

        public bool Daemon { get; private set; }

        public Table<LanguageEntry> LanguageTable { get; }

        public L10nData L10nData { get; }

        public NovelData NovelData { get; }

        public CharacterData CharacterData { get; }

        public QuestData QuestData { get; }

        public ListSegment<string> Languages
            => this.languages;

        public ListSegment<string> UsedLanguages
            => this.usedLanguages;

        protected CsvDataLoader CsvLoader { get; }

        private readonly List<string> languages;
        private readonly List<string> usedLanguages;

        public Database()
        {
            this.languages = new List<string>();
            this.usedLanguages = new List<string>();

            this.CsvLoader = new CsvDataLoader();
            this.LanguageTable = new Table<LanguageEntry>();
            this.L10nData = new L10nData();
            this.NovelData = new NovelData();
            this.CharacterData = new CharacterData();
            this.QuestData = new QuestData();
        }

        protected TextAsset GetCsv(string key)
            => this.config.CsvFiles[key];

        public void Initialize()
        {
            Unload();

            this.CsvLoader.Initialize(this.config);
            this.Initialized = true;
        }

        public UniTask Load()
        {
            if (!this.Initialized)
            {
                UnuLogger.LogError($"{nameof(Database<T>)} is not initialized");
                return UniTask.FromResult(false);
            }

            this.Daemon = this.config.CheckDaemon();

            this.CsvLoader.Load<LanguageEntry,
                                LanguageEntry.Mapping>
                                (this.LanguageTable, GetCsv("Language"), true);

            PrepareLanguages();

            this.CsvLoader.Load<L10nData,
                                L10nParser>
                                (this.L10nData, GetCsv(nameof(this.L10nData)), this.Languages);

            this.CsvLoader.Load<NovelData,
                                NovelParser>
                                (this.NovelData, GetCsv(nameof(this.NovelData)), this.Languages);

            this.CsvLoader.Load<CharacterData,
                                CharacterParser>
                                (this.CharacterData, GetCsv(nameof(this.CharacterData)), this.Languages);

            this.CsvLoader.Load<QuestData,
                                QuestParser>
                                (this.QuestData, GetCsv(nameof(this.QuestData)), this.Languages);

            OnLoad();

            return UniTask.FromResult(true);
        }

        protected void Unload()
        {
            this.L10nData.Clear();
            this.NovelData.Clear();
            this.CharacterData.Clear();
            this.QuestData.Clear();

            OnUnload();
        }

        protected abstract void OnLoad();

        protected abstract void OnUnload();

        private void PrepareLanguages()
        {
            this.languages.Clear();
            this.usedLanguages.Clear();

            foreach (var entry in this.LanguageTable.Entries)
            {
                this.languages.Add(entry.Key);

                if (entry.IsUsed)
                    this.usedLanguages.Add(entry.Key);
            }
        }
    }
}
