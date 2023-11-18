using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Sirenix.OdinInspector;

namespace HegaCore
{
    using Database;

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
        private string externalCsvFolder = "../Database";

        [BoxGroup("Csv Files")]
        [DictionaryDrawerSettings(KeyLabel = "Name", ValueLabel = "File"), PropertySpace]
        [SerializeField, LabelText("Files")]
        private CsvFileMap csvFiles = new CsvFileMap();

        [BoxGroup("Csv Files")]
        [SerializeField, ReadOnly, LabelText("Internal Path")]
        private string internalCsvPath = "Game/Database";
        
        [BoxGroup("External Files")]
        [SerializeField, LabelText("Daemon") , Tooltip("IN-GAME CHEAT")]
        private string daemonFile = "daemon.dae";

        [BoxGroup("External Files")]
        [SerializeField, ReadOnly, LabelText("Path"), Indent]
        private string daemonFilePath = string.Empty;

        [BoxGroup("External Files")]
        [SerializeField, LabelText("Overlord"), Tooltip("ACHIEVEMENTS CHEAT")]
        private string overlordFile = "overlord.ove";

        [BoxGroup("External Files")]
        [SerializeField, ReadOnly, LabelText("Path"), Indent]
        private string overlordFilePath = string.Empty;

        [BoxGroup("Addressables Files")]
        [SerializeField, LabelText("DarkLord"), Tooltip("R18 DLC")]
        private string darkLordFile = "darklord.dlc";

        [BoxGroup("Addressables Files")]
        [SerializeField, LabelText("Not Found Log"), Indent]
        private string darkLordFileNotFoundLog = "DLC is not installed";

        public SaveDataConfig SaveData => this.saveDataConfig;

        public string InternalCsvFolder
        {
            get => this.internalCsvFolder;
            set => this.internalCsvFolder = value ?? string.Empty;
        }

        public string ExternalCsvFolder
        {
            get => this.externalCsvFolder;
            set => this.externalCsvFolder = value ?? string.Empty;
        }

        public ReadDictionary<string, TextAsset> CsvFiles => this.csvFiles;

        public string DaemonFile => this.daemonFile;

        public string OverlordFile => this.overlordFile;

        public string DarkLordFile => this.darkLordFile;

        public string DarkLordFileWarning => this.darkLordFileNotFoundLog;

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
            private string folder = "save_folder";

            [SerializeField]
            private string folderEditor = "../external_save_folder";

            [SerializeField]
            private string fileName = "file_name";

            [SerializeField]
            private string extension = "ext";

            [SerializeField]
            private string bakExtension = "bak.ext";

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