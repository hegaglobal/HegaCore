using System.Collections.Generic;
using System.IO;
using System.Table;
using UnityEngine;
using UnityEngine.AddressableAssets;
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

        public bool Overlord { get; private set; }

        public bool DarkLord { get; private set; }

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

        public async UniTask LoadAsync()
        {
            if (!this.Initialized)
            {
                UnuLogger.LogError($"{GetType().Name} is not initialized");
                return;
            }

            await UniTask.DelayFrame(1);

            CheckDarkLord();
            CheckDaemon();
            CheckOverlord();

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

            ContinueLoad();
        }

        protected abstract void ContinueLoad();

        protected void Unload()
        {
            this.Tables.Clear();

            ContinueUnload();
        }

        protected abstract void ContinueUnload();

        private void CheckDarkLord()
        {
            try
            {
                this.DarkLord = AddressablesManager.ContainsKey(this.config.DarkLordFile);
            }
            catch
            {
                this.DarkLord = false;
            }
        }

        private void CheckDaemon()
        {
            try
            {
                this.Daemon = FileSystem.FileExists(this.config.DaemonFileFullPath);
            }
            catch
            {
                this.Daemon = false;
            }
        }

        private void CheckOverlord()
        {
            try
            {
                this.Overlord = FileSystem.FileExists(this.config.OverlordFileFullPath);
            }
            catch
            {
                this.Overlord = false;
            }
        }

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
