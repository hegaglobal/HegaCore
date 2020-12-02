using System;
using System.IO;
using UnityEngine;

namespace HegaCore
{
    [Serializable]
    public abstract class GameDataHandler<TPlayerData, TGameData>
        where TPlayerData : PlayerData<TPlayerData>, new()
        where TGameData : GameData<TPlayerData>
    {
        [SerializeField, HideInInspector]
        private string folderPath = default;

        [SerializeField, HideInInspector]
        private string fileName = default;

        [SerializeField, HideInInspector]
        private string extension = default;

        [SerializeField, HideInInspector]
        private string bakExtension = default;

        [SerializeField, HideInInspector]
        private string filePath = default;

        [SerializeField, HideInInspector]
        private string bakPath = default;

        protected string FolderPath => this.folderPath;

        protected string FileName => this.fileName;

        protected string Extension => this.extension;

        protected string BakExtension => this.bakExtension;

        protected string FilePath => this.filePath;

        protected string BakPath => this.bakPath;

        public bool IsOverwritten { get; set; }

        public void Initialize(string folderPath, string fileName, string extension, string bakExtension)
        {
            this.folderPath = folderPath ?? string.Empty;
            this.fileName = fileName ?? string.Empty;
            this.extension = extension ?? string.Empty;
            this.bakExtension = bakExtension ?? string.Empty;

            this.filePath = Path.Combine(this.folderPath, $"{this.fileName}.{this.extension}");
            this.bakPath = Path.Combine(this.folderPath, $"{this.fileName}.{this.bakExtension}");
        }

        public void EnsureFileExisting()
        {
            if (!FileSystem.DirectoryExists(this.FolderPath))
                FileSystem.CreateDirectory(this.FolderPath);

            if (!FileSystem.FileExists(this.FilePath))
                Save(New());
        }

        public TGameData Load(bool shouldBackUp)
        {
            try
            {
                if (TryRead(this.FilePath, out var data))
                {
                    if (shouldBackUp)
                        BackUpFile();

                    return data;
                }

                return FromCorrupted();
            }
            catch
            {
                return FromCorrupted();
            }
        }

        public void Save(TGameData data)
            => Write(data, this.FilePath);

        public abstract TGameData New(bool corrupted = false);

        protected virtual bool TryRead(string filePath, out TGameData data)
            => throw new NotImplementedException();

        protected virtual void Write(TGameData data, string filePath)
            => throw new NotImplementedException();

        private void BackUpFile()
        {
            UnuLogger.Log("Back up saved data...");

            if (TryBackUp(this.FolderPath, this.FileName, this.Extension, this.BakExtension))
                UnuLogger.Log("Back up successfully");
            else
                UnuLogger.LogWarning("Cannot back up");
        }

        private void BackUpBak(string suffix)
        {
            UnuLogger.Log("Back up old backup...");

            if (TryBackUp(this.FolderPath, this.FileName, this.BakExtension, this.BakExtension, suffix))
                UnuLogger.Log("Back up successfully");
            else
                UnuLogger.LogWarning("Cannot back up");
        }

        private TGameData FromCorrupted()
        {
            if (!FileSystem.FileExists(this.BakPath))
                return Overwrite();

            var time = "00010101000000";

            try
            {
                var now = DateTime.Now;
                time = $"{now.Year}{now.Month}{now.Day}{now.Hour}{now.Minute}{now.Second}";
            }
            catch { }

            try
            {
                return TryLoadBackup(time);
            }
            catch (Exception ex)
            {
                UnuLogger.LogException(ex);
                return Overwrite();
            }
        }

        private TGameData TryLoadBackup(string suffix)
        {
            UnuLogger.Log("Try to load backup...");

            try
            {
                if (TryRead(this.BakPath, out var data))
                {
                    UnuLogger.Log($"Load successfully");
                    BackUpBak(suffix);
                    return data;
                }

                UnuLogger.LogError($"Cannot load");
                return Overwrite();
            }
            catch
            {
                UnuLogger.LogError($"Cannot load");
                return Overwrite();
            }
        }

        private TGameData Overwrite()
        {
            UnuLogger.LogError($"Overwrite corrupted saved data by new data");
            this.IsOverwritten = true;
            return New(true);
        }

        private static bool TryBackUp(string folderPath, string fileName, string extension, string bakExtension, string bakSuffix = "")
        {
            var filePath = Path.Combine(folderPath, $"{fileName}.{extension}");

            if (string.IsNullOrEmpty(filePath))
                return false;

            if (!FileSystem.FileExists(filePath))
                return false;

            try
            {
                var sourceFilePath = Path.GetFullPath(filePath);

                string bakFilePath;

                if (string.IsNullOrEmpty(bakSuffix))
                    bakFilePath = Path.Combine(Path.GetFullPath(folderPath), $"{fileName}.{bakExtension}");
                else
                    bakFilePath = Path.Combine(Path.GetFullPath(folderPath), $"{fileName}-{bakSuffix}.{bakExtension}");

                File.Copy(sourceFilePath, bakFilePath, true);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}