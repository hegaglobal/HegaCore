#if UNITY_EDITOR

using System.IO;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;

namespace HegaCore.Editor
{
    public abstract class GameDataEditor<TGameData, TPlayerData, TGameDataHandler, TPlayerDataEditor> : MonoBehaviour
        where TGameData : GameData<TPlayerData>, new()
        where TPlayerData : PlayerData<TPlayerData>, new()
        where TGameDataHandler : GameDataHandler<TGameData, TPlayerData>, new()
        where TPlayerDataEditor : PlayerDataEditor<TPlayerData>
    {
        [PropertyOrder(0), BoxGroup(GroupID = "Save Data")]
        [SerializeField]
        private DatabaseConfig config = null;

        [PropertyOrder(0), BoxGroup(GroupID = "Save Data")]
        [SerializeField, InlineButton(nameof(RevealFolder), "Open")]
        private string folderPath = string.Empty;

        [PropertyOrder(0), BoxGroup(GroupID = "Save Data")]
        [SerializeField, InlineButton(nameof(RevealFile), "Open")]
        private string filePath = string.Empty;

        [SerializeField, HideInInspector]
        private TGameDataHandler handler = new TGameDataHandler();

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
                this.folderPath = this.config.SaveDataEditorFolderFullPath;
                this.filePath = this.config.SaveDataEditorFileFullPath;
            }
            else
            {
                this.folderPath = string.Empty;
                this.filePath = string.Empty;
            }

            this.settings = GetComponentInChildren<GameSettingsEditor>();
            this.players = GetComponentsInChildren<TPlayerDataEditor>();
            this.handler.Initialize(this.folderPath, this.filePath);
        }

        [PropertySpace]
        [PropertyOrder(1)]
        [HorizontalGroup(GroupID = "Buttons")]
        [Button]
        public void Load()
        {
            this.handler.EnsureFileExisting();
            var data = this.handler.Load();

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
            this.handler.EnsureFileExisting();
            var data = this.handler.Load();

            this.settings.CopyTo(data.Settings);

            var length = Mathf.Min(this.players.Length, data.Players.Length);

            for (var i = 0; i < length; i++)
            {
                this.players[i].CopyTo(data.Players[i]);
            }

            this.handler.Save(data);
        }

        private void RevealFolder()
            => EditorUtility.RevealInFinder(this.folderPath);

        private void RevealFile()
            => EditorUtility.RevealInFinder(this.filePath);
    }
}

#endif