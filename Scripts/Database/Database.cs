using System;
using System.Collections.Generic;
using System.IO;
using System.Table;
using UnityEngine;
using Cysharp.Threading.Tasks;
using VisualNovelData.Data;
using VisualNovelData.Parser;
using Sirenix.OdinInspector;

namespace HegaCore
{
    public abstract partial class SingletonDatabase<T> : SingletonBehaviour<T> where T : SingletonDatabase<T>
    {
        [BoxGroup("Save Data")]
        [SerializeField, LabelText("Folder")]
        private string saveDataFolder = string.Empty;

        [BoxGroup("Save Data")]
        [SerializeField, LabelText("File")]
        private string saveDataFile = string.Empty;

        [BoxGroup("Csv Files")]
        [SerializeField]
        private string internalFolder = "Database";

        [BoxGroup("Csv Files")]
        [SerializeField]
        private string externalFolder = "Database";

        [BoxGroup("Csv Files")]
        [SerializeField, DictionaryDrawerSettings(KeyLabel = "Name", ValueLabel = "File"), PropertySpace]
        private CsvFileMap csvFiles = new CsvFileMap();

        [SerializeField, ReadOnly, BoxGroup("Csv Files")]
        private string internalPath = string.Empty;

        [BoxGroup("Other Files")]
        [SerializeField, LabelText("Daemon")]
        private string DaemonFile = null;

        public string ExternalCsvPath { get; private set; }

        public string SaveDataFolderPath { get; private set; }

        public string SaveDataFilePath { get; private set; }

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

        public SingletonDatabase()
        {
            this.languages = new List<string>();
            this.usedLanguages = new List<string>();

            this.CsvLoader = new CsvDataLoader();
            this.L10nData = new L10nData();
            this.NovelData = new NovelData();
            this.CharacterData = new CharacterData();
            this.QuestData = new QuestData();
        }

        protected virtual void OnValidate()
        {
            this.internalPath = $"Assets/{this.internalFolder}";
        }

        protected TextAsset GetCsv(string key)
            => this.csvFiles[key];

        public void Initialize()
        {
            this.ExternalCsvPath = Path.Combine(Application.dataPath, "..", this.externalFolder);

#if UNITY_EDITOR
            this.SaveDataFolderPath = Path.Combine(Application.dataPath, "..", this.saveDataFolder);
#else
            this.SaveDataFolderPath = Path.Combine(Application.dataPath, this.saveDataFolder);
#endif
            this.SaveDataFilePath = Path.Combine(this.SaveDataFolderPath, this.saveDataFile);

            this.CsvLoader.SetExternalPath(this.ExternalCsvPath);
        }

        public virtual UniTask Load()
        {
            this.Daemon = this.CsvLoader.Daemon(this.DaemonFile);

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

            return UniTask.FromResult(true);
        }

        public virtual void Unload()
        {
            this.L10nData.Clear();
            this.NovelData.Clear();
            this.CharacterData.Clear();
            this.QuestData.Clear();
        }

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
