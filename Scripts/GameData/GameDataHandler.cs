using System;
using System.IO;
using UnityEngine;

namespace HegaCore
{
    [Serializable]
    public abstract class GameDataHandler<TPlayerData, TGameData>
        where TPlayerData : PlayerData<TPlayerData>, new()
        where TGameData : GameData<TPlayerData>, new()
    {
        [SerializeField, HideInInspector]
        private string folderPath = default;

        [SerializeField, HideInInspector]
        private string filePath = default;

        [SerializeField, HideInInspector]
        private string extension = default;

        protected string FolderPath => this.folderPath;

        public string FilePath => this.filePath;

        public string Extension => this.extension;

        public void Initialize(string folderPath, string filePath, string extension)
        {
            this.folderPath = folderPath ?? string.Empty;
            this.filePath = filePath ?? string.Empty;
            this.extension = extension ?? string.Empty;
        }

        public void EnsureFileExisting()
        {
            if (!FileSystem.DirectoryExists(this.folderPath))
                FileSystem.CreateDirectory(this.folderPath);

            if (!FileSystem.FileExists(this.filePath))
                Save(new TGameData());
        }

        public TGameData Load(bool shouldBackup)
        {
            try
            {
                var data = Read();
                return data;
            }
            catch
            {
                UnuLogger.LogError($"Saved data is corrupted");
                return new TGameData();
            }
        }

        public void Save(TGameData data)
            => Write(data);

        protected abstract TGameData Read();

        protected abstract void Write(TGameData data);
    }
}