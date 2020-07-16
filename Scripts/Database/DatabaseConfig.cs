using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Sirenix.OdinInspector;

namespace HegaCore
{
    [CreateAssetMenu(fileName = nameof(DatabaseConfig), menuName = "Database Config")]
    public sealed partial class DatabaseConfig : ScriptableObject
    {
        [BoxGroup("Save Data")]
        [SerializeField, LabelText("Folder")]
        private string saveDataFolder = string.Empty;

        [BoxGroup("Save Data")]
        [SerializeField, LabelText("Folder (Editor Mode)")]
        private string saveDataEditorFolder = string.Empty;

        [BoxGroup("Save Data")]
        [SerializeField, LabelText("File")]
        private string saveDataFile = string.Empty;

        [BoxGroup("Csv Files")]
        [SerializeField]
        private string internalCsvFolder = "Database";

        [BoxGroup("Csv Files")]
        [SerializeField]
        private string externalCsvFolder = "Database";

        [BoxGroup("Csv Files")]
        [SerializeField, DictionaryDrawerSettings(KeyLabel = "Name", ValueLabel = "File"), PropertySpace]
        private CsvFileMap csvFiles = new CsvFileMap();

        [SerializeField, ReadOnly, BoxGroup("Csv Files")]
        private string internalCsvPath = string.Empty;

        [BoxGroup("Other Files")]
        [SerializeField, LabelText("Daemon")]
        private string daemonFile = null;

        public string SaveDataFolder => this.saveDataFolder;

        public string SaveDataEditorFolder => this.saveDataEditorFolder;

        public string SaveDataFile => this.saveDataFile;

        public string InternalCsvFolder => this.internalCsvFolder;

        public string ExternalCsvFolder => this.externalCsvFolder;

        public IReadOnlyDictionary<string, TextAsset> CsvFiles => this.csvFiles;

        public string DaemonFile => this.daemonFile;

        public string SaveDataFolderFullPath => Path.Combine(Application.dataPath, this.saveDataFolder);

        public string SaveDataEditorFolderFullPath => Path.Combine(Application.dataPath, this.saveDataEditorFolder);

        public string SaveDataFileFullPath => Path.Combine(this.SaveDataFolderFullPath, this.saveDataFile);

        public string SaveDataEditorFileFullPath => Path.Combine(this.SaveDataEditorFolderFullPath, this.saveDataFile);

        public string InternalCsvFolderFullPath => Path.Combine(Application.dataPath, this.internalCsvFolder);

        public string ExternalCsvFolderFullPath => Path.Combine(Application.dataPath, this.externalCsvFolder);

        public string DaemonFileFullPath => Path.Combine(this.ExternalCsvFolderFullPath, this.daemonFile);

        private void OnValidate()
        {
            this.internalCsvPath = $"Assets/{this.internalCsvFolder}";
        }

        public bool CheckDaemon()
            => File.Exists(this.DaemonFileFullPath);

        public string GetExternalCsvFileFullPath(string file)
            => Path.Combine(this.ExternalCsvFolderFullPath, file);
    }
}