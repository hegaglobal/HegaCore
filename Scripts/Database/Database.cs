using System.Collections.Generic;
using System.Table;
using UnityEngine;
using Cysharp.Threading.Tasks;
using TinyCsvParser.Mapping;
using VisualNovelData.Data;
using VisualNovelData.Parser;

namespace HegaCore
{
    using Database;

    public abstract partial class Database<TDatabase, TTables> : SingletonBehaviour<TDatabase>
        where TDatabase : Database<TDatabase, TTables>
        where TTables : Tables, new()
    {
        [SerializeField]
        private DatabaseConfig config = null;

        public DatabaseConfig Config
            => this.config;

        public bool Initialized { get; private set; }

        public bool Daemon { get; private set; }

        public TTables Tables { get; }

        public ListSegment<string> Languages
            => this.languages;

        public ListSegment<string> UsedLanguages
            => this.usedLanguages;

        private readonly CsvDataLoader csvLoader;
        private readonly List<string> languages;
        private readonly List<string> usedLanguages;

        public Database()
        {
            this.languages = new List<string>();
            this.usedLanguages = new List<string>();

            this.csvLoader = new CsvDataLoader();
            this.Tables = new TTables();
        }

        public void Initialize()
        {
            Unload();

            this.csvLoader.Initialize(this.config);
            this.Initialized = true;
        }

        public UniTask Load()
        {
            if (!this.Initialized)
            {
                UnuLogger.LogError($"{GetType().Name} is not initialized");
                return UniTask.FromResult(false);
            }

            this.Daemon = this.config.CheckDaemon();

            Load<LanguageEntry, LanguageEntry.Mapping>
                 (this.Tables.Language, nameof(this.Tables.Language), true);

            PrepareLanguages();

            Load<L10nData, L10nParser>
                 (this.Tables.L10nData, nameof(this.Tables.L10nData), this.Languages);

            Load<NovelData, NovelParser>
                 (this.Tables.NovelData, nameof(this.Tables.NovelData), this.Languages);

            Load<CharacterData, CharacterParser>
                 (this.Tables.CharacterData, nameof(this.Tables.CharacterData), this.Languages);

            Load<QuestData, QuestParser>
                 (this.Tables.QuestData, nameof(this.Tables.QuestData), this.Languages);

            OnLoad();

            return UniTask.FromResult(true);
        }

        protected abstract void OnLoad();

        protected void Unload()
        {
            this.Tables.Clear();

            OnUnload();
        }

        protected abstract void OnUnload();

        private void PrepareLanguages()
        {
            this.languages.Clear();
            this.usedLanguages.Clear();

            foreach (var entry in this.Tables.Language.Entries)
            {
                this.languages.Add(entry.Key);

                if (entry.IsUsed)
                    this.usedLanguages.Add(entry.Key);
            }
        }

        private TextAsset GetCsv(string key)
            => this.config.CsvFiles[key];

        protected void Load<TEntity, TMapping>(ITable<TEntity> table, string file, bool autoIncrement = false)
            where TEntity : class, IEntry, new()
            where TMapping : CsvMapping<TEntity>, new()
        {
            this.csvLoader.Load<TEntity, TMapping>(table, GetCsv(file), autoIncrement);
        }

        protected void Load<TEntity, TMapping>(ITable<TEntity> table, string file, IGetId<TEntity> idGetter)
            where TEntity : class, IEntry, new()
            where TMapping : CsvMapping<TEntity>, new()
        {
            this.csvLoader.Load<TEntity, TMapping>(table, GetCsv(file), idGetter);
        }

        protected void Load<TEntity, TMapping, TIdGetter>(ITable<TEntity> table, string file)
            where TEntity : class, IEntry, new()
            where TMapping : CsvMapping<TEntity>, new()
            where TIdGetter : IGetId<TEntity>, new()
        {
            this.csvLoader.Load<TEntity, TMapping>(table, GetCsv(file));
        }

        protected void Load<TData, TParser>(TData data, string file, in Segment<string> languages)
            where TData : class
            where TParser : VisualNovelData.Parser.ICsvParser<TData>, new()
        {
            this.csvLoader.Load<TData, TParser>(data, GetCsv(file), languages);
        }
    }
}
