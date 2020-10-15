using System.Collections.Generic;
using System.IO;
using System.Table;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;
using TinyCsvParser.Mapping;
using VisualNovelData.Parser;

namespace HegaCore
{
    using Database.Csv;

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

            await CheckDarkLordAsync();

            CheckDaemon();
            CheckOverlord();

            ContinueLoad();
        }

        protected abstract void ContinueLoad();

        protected void Unload()
        {
            this.Tables.Clear();

            ContinueUnload();
        }

        protected abstract void ContinueUnload();

        private async UniTask CheckDarkLordAsync()
        {
            try
            {
                var (isCanceled, result) = await AddressablesManager.LoadAssetAsync<TextAsset>(this.config.DarkLordFile)
                                                                    .SuppressCancellationThrow();
                this.DarkLord = !isCanceled && result.Succeeded;
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

        protected void ClearLanguages()
        {
            this.languages.Clear();
            this.usedLanguages.Clear();
        }

        protected void AddLanguage(string language, bool isUsed)
        {
            if (string.IsNullOrEmpty(language))
                return;

            if (!this.languages.Contains(language))
                this.languages.Add(language);

            if (isUsed && !this.usedLanguages.Contains(language))
                this.usedLanguages.Add(language);
        }

        public TextAsset GetCsv(string key)
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
            this.csvLoader.Load<TEntity, TMapping, TIdGetter>(table, GetCsv(file));
        }

        protected void Load<TData, TParser>(TData data, string file, in Segment<string> languages)
            where TData : class
            where TParser : ICsvParser<TData>, new()
        {
            this.csvLoader.Load<TData, TParser>(data, GetCsv(file), languages);
        }
    }
}
