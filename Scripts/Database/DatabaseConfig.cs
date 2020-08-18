using System;
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
        [SerializeField, HideLabel]
        private SaveDataConfig saveDataConfig = new SaveDataConfig();

        [BoxGroup("Csv Files")]
        [SerializeField, LabelText("Internal Folder")]
        private string internalCsvFolder = "Database";

        [BoxGroup("Csv Files")]
        [SerializeField, LabelText("External Folder")]
        private string externalCsvFolder = "Database";

        [BoxGroup("Csv Files")]
        [DictionaryDrawerSettings(KeyLabel = "Name", ValueLabel = "File"), PropertySpace]
        [SerializeField, LabelText("Files")]
        private CsvFileMap csvFiles = new CsvFileMap();

        [BoxGroup("Csv Files")]
        [SerializeField, ReadOnly, LabelText("Internal Path")]
        private string internalCsvPath = string.Empty;

        [BoxGroup("Other Files")]
        [SerializeField, LabelText("Daemon")]
        private string daemonFile = null;

        [BoxGroup("Other Files")]
        [SerializeField, LabelText("Overlord")]
        private string overlordFile = null;

        [BoxGroup("Other Files")]
        [SerializeField, LabelText("DarkLord")]
        private string darkLordFile = null;

        public SaveDataConfig SaveData => this.saveDataConfig;

        public string InternalCsvFolder => this.internalCsvFolder;

        public string ExternalCsvFolder => this.externalCsvFolder;

        public ReadDictionary<string, TextAsset> CsvFiles => this.csvFiles;

        public string DaemonFile => this.daemonFile;

        public string OverlordFile => this.overlordFile;

        public string DarkLordFile => this.darkLordFile;

        public string InternalCsvFolderFullPath => Path.Combine(Application.dataPath, this.internalCsvFolder);

        public string ExternalCsvFolderFullPath => Path.Combine(Application.dataPath, this.externalCsvFolder);

        public string DaemonFileFullPath => Path.Combine(Application.dataPath, this.externalCsvFolder, this.daemonFile);

        public string OverlordFileFullPath => Path.Combine(Application.dataPath, this.externalCsvFolder, this.overlordFile);

        public string GetExternalCsvFileFullPath(string file)
            => Path.Combine(this.ExternalCsvFolderFullPath, file);

        [Serializable, InlineProperty]
        public sealed class SaveDataConfig
        {
            [SerializeField]
            private string folder = string.Empty;

            [SerializeField]
            private string folderEditor = string.Empty;

            [SerializeField]
            private string fileName = string.Empty;

            [SerializeField]
            private string extension = string.Empty;

            [SerializeField]
            private string bakExtension = string.Empty;

            public string Folder => this.folder;

            public string FolderEditor => this.folderEditor;

            public string FileName => this.fileName;

            public string BakExtension => this.bakExtension;

            public string Extension => this.extension;

            public string File => $"{this.fileName}.{this.extension}";

            public string BakFile => $"{this.fileName}.{this.bakExtension}";

            public string FolderFullPath => Path.Combine(Application.dataPath, this.folder);

            public string FolderFullPathEditor => Path.Combine(Application.dataPath, this.folderEditor);

            public string FileFullPath => Path.Combine(this.FolderFullPath, this.File);

            public string FileFullPathEditor => Path.Combine(this.FolderFullPathEditor, this.File);
        }
    }
}