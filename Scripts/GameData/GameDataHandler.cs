using System;
using System.IO;
using UnityEngine;

namespace HegaCore
{
    [Serializable]
    public abstract class GameDataHandler<TGameData, TPlayerData>
        where TGameData : GameData<TPlayerData>, new()
        where TPlayerData : PlayerData<TPlayerData>, new()
    {
        [SerializeField, HideInInspector]
        private string folderPath = default;

        [SerializeField, HideInInspector]
        private string filePath = default;

        protected string FolderPath => this.folderPath;

        public string FilePath => this.filePath;

        public void Initialize(string folderPath, string filePath)
        {
            this.folderPath = folderPath ?? string.Empty;
            this.filePath = filePath ?? string.Empty;
        }

        public void EnsureFileExisting()
        {
            if (!FileSystem.DirectoryExists(this.folderPath))
                FileSystem.CreateDirectory(this.folderPath);

            if (!FileSystem.FileExists(this.filePath))
                Save(new TGameData());
        }

        public abstract TGameData Load();

        public abstract void Save(TGameData data);
    }
}