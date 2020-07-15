#if UNITY_EDITOR

using System.IO;
using UnityEngine;
using Sirenix.OdinInspector;

namespace HegaCore.Editor
{
    public abstract class GameDataEditor<TGameData, TPlayerData, THandleGameData, TPlayerDataEditor> : MonoBehaviour
        where TGameData : GameData<TPlayerData>, new()
        where TPlayerData : PlayerData<TPlayerData>, new()
        where THandleGameData : IHandleGameData<TGameData, TPlayerData>, new()
        where TPlayerDataEditor : PlayerDataEditor<TPlayerData>
    {
        [PropertyOrder(0), BoxGroup(GroupID = "Save Data")]
        [SerializeField]
        private DatabaseConfig config = null;

        [PropertyOrder(0), BoxGroup(GroupID = "Save Data")]
        [SerializeField, ReadOnly, FolderPath(AbsolutePath = true, UseBackslashes = true)]
        private string folderPath = string.Empty;

        [PropertyOrder(0), BoxGroup(GroupID = "Save Data")]
        [SerializeField, ReadOnly, FilePath(AbsolutePath = true, UseBackslashes = true)]
        private string filePath = string.Empty;

        [SerializeField, HideInInspector]
        private THandleGameData handler = new THandleGameData();

        [PropertySpace]
        [PropertyOrder(2), InlineEditor]
        [SerializeField]
        private GameSettingsEditor settings = null;

        [PropertyOrder(2), InlineEditor]
        [SerializeField]
        private TPlayerDataEditor[] players = null;

        private void OnValidate()
        {
            if (this.config)
            {
                var parentPath = Directory.GetParent(Application.dataPath).FullName;
                this.folderPath = Path.Combine(parentPath, this.config.SaveDataEditorFolder);
                this.filePath = Path.Combine(this.folderPath, this.config.SaveDataFile);
            }

            this.settings = GetComponentInChildren<GameSettingsEditor>();
            this.players = GetComponentsInChildren<TPlayerDataEditor>();
        }

        [PropertySpace]
        [PropertyOrder(1)]
        [HorizontalGroup(GroupID = "Buttons")]
        [Button]
        public void Load()
        {
            EnsureDataFile();
            var data = LoadData();

            this.settings.Set(data.Settings);

            var length = Mathf.Min(this.players.Length, data.Players.Length);

            for (var i = 0; i < length; i++)
            {
                this.players[i].Set(data.Players[i]);
            }
        }

        [PropertySpace]
        [PropertyOrder(1)]
        [HorizontalGroup(GroupID = "Buttons")]
        [Button]
        public void Save()
        {
            EnsureDataFile();
            var data = LoadData();

            this.settings.CopyTo(data.Settings);

            var length = Mathf.Min(this.players.Length, data.Players.Length);

            for (var i = 0; i < length; i++)
            {
                this.players[i].CopyTo(data.Players[i]);
            }

            this.handler.Save(data);
        }

        public void EnsureDataFile()
        {
            if (!FileSystem.DirectoryExists(this.folderPath))
                FileSystem.CreateDirectory(this.folderPath);

            if (!FileSystem.FileExists(this.filePath))
                this.handler.Save(new TGameData());
        }

        private TGameData LoadData()
        {
            try
            {
                var data = FileSystem.ReadFromBinaryFile<TGameData>(this.filePath);
                return data;
            }
            catch
            {
                UnuLogger.LogWarning($"Failed to load data at {this.filePath}.");
                UnuLogger.LogWarning($"{this.filePath} will be overwrited with new data.");
                return null;
            }
        }
    }
}

#endif