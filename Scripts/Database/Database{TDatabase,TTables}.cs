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

        /// <summary>
        /// DEV CHEAT
        /// </summary>
        public bool Daemon { get; private set; }

        /// <summary>
        /// Achievement
        /// </summary>
        public bool Overlord { get; private set; }

        /// <summary>
        /// R DLC
        /// </summary>
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

        protected virtual void ContinueLoad() { }

        protected void Unload()
        {
            this.Tables.Clear();

            ContinueUnload();
        }

        protected virtual void ContinueUnload() { }

        private async UniTask CheckDarkLordAsync()
        {
            try
            {
                var result = await AddressablesManager.LoadAssetAsync<TextAsset>(this.config.DarkLordFile);
                this.DarkLord = result.Succeeded;
            }
            catch
            {
                this.DarkLord = false;
            }

            if (!this.DarkLord && !string.IsNullOrWhiteSpace(this.config.DarkLordFileWarning))
                UnuLogger.LogWarning(this.config.DarkLordFileWarning);
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

        public bool TryGetCsv(string name, out TextAsset csv, bool silent = false)
        {
            if (!this.config.CsvFiles.TryGetValue(name, out csv))
            {
                csv = null;

                if (!silent)
                    UnuLogger.LogError($"Cannot find CSV file by name={name}", this.config);
            }

            return csv;
        }

        protected void Load<TEntity, TMapping>(ITable<TEntity> table, string file, bool autoIncrement = false)
            where TEntity : class, IEntry, new()
            where TMapping : CsvMapping<TEntity>, new()
        {
            if (!TryGetCsv(file, out var csv))
                return;

            this.csvLoader.Load<TEntity, TMapping>(table, csv, autoIncrement);
        }

        protected void Load<TEntity, TMapping>(ITable<TEntity> table, string file, IGetId<TEntity> idGetter)
            where TEntity : class, IEntry, new()
            where TMapping : CsvMapping<TEntity>, new()
        {
            if (!TryGetCsv(file, out var csv))
                return;

            this.csvLoader.Load<TEntity, TMapping>(table, csv, idGetter);
        }

        protected void Load<TEntity, TMapping, TIdGetter>(ITable<TEntity> table, string file)
            where TEntity : class, IEntry, new()
            where TMapping : CsvMapping<TEntity>, new()
            where TIdGetter : IGetId<TEntity>, new()
        {
            if (!TryGetCsv(file, out var csv))
                return;

            this.csvLoader.Load<TEntity, TMapping, TIdGetter>(table, csv);
        }

        protected void Load<TData, TParser>(TData data, string file, in Segment<string> languages)
            where TData : class
            where TParser : ICsvParser<TData>, new()
        {
            if (!TryGetCsv(file, out var csv))
                return;

            this.csvLoader.Load<TData, TParser>(data, csv, languages);
        }
    }
}
