﻿#if UNITY_EDITOR

using System.IO;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;

namespace HegaCore.Editor
{
    public abstract class GameDataEditor<TPlayerData, TGameSettings, TGameData, THandler, TPlayerDataEditor, TGameSettingsEditor> : MonoBehaviour
        where TPlayerData : PlayerData<TPlayerData>, new()
        where TGameSettings : GameSettings<TGameSettings>, new()
        where TGameData : GameData<TPlayerData, TGameSettings>
        where THandler : GameDataHandler<TPlayerData, TGameSettings, TGameData>, new()
        where TPlayerDataEditor : PlayerDataEditor<TPlayerData>
        where TGameSettingsEditor : GameSettingsEditor<TGameSettings>
    {
        [PropertyOrder(0), BoxGroup(GroupID = "Save Data")]
        [SerializeField]
        private DatabaseConfig config = null;

        [PropertyOrder(0), BoxGroup(GroupID = "Save Data")]
        [SerializeField, InlineButton(nameof(RevealFolder), "Open")]
        private string folderPath = string.Empty;

        [PropertyOrder(0), BoxGroup(GroupID = "Save Data")]
        [SerializeField, InlineButton(nameof(RevealFile), "Open")]
        private string fileName = string.Empty;

        [PropertyOrder(0), BoxGroup(GroupID = "Save Data")]
        [SerializeField]
        private string extension = string.Empty;

        [SerializeField, HideInInspector]
        private THandler handler = new THandler();

        [PropertySpace]
        [PropertyOrder(2)]
        [SerializeField]
        private bool onceCorrupted = false;

        [PropertyOrder(2), InlineEditor]
        [SerializeField]
        private TGameSettingsEditor settings = null;

        [PropertyOrder(2), InlineEditor]
        [SerializeField]
        private TPlayerDataEditor[] players = null;

        public string FilePath => Path.Combine(this.folderPath, $"{this.fileName}.{this.extension}");

        private void Initialize()
        {
            if (this.config)
            {
                this.folderPath = this.config.SaveData.FolderFullPathEditor;
                this.fileName = this.config.SaveData.FileName;
                this.extension = this.config.SaveData.Extension;
            }
            else
            {
                this.folderPath = string.Empty;
                this.fileName = string.Empty;
                this.extension = string.Empty;
            }

            this.settings = GetComponentInChildren<TGameSettingsEditor>();
            this.players = GetComponentsInChildren<TPlayerDataEditor>().OrEmpty();
            this.handler.Initialize(this.folderPath, this.fileName, this.extension, string.Empty);
        }

        [PropertySpace]
        [PropertyOrder(1)]
        [HorizontalGroup(GroupID = "Buttons")]
        [Button]
        public void Load()
        {
            Initialize();

            this.handler.EnsureFileExisting();
            var data = this.handler.Load(false);

            if (data == null)
                return;

            this.onceCorrupted = data.OnceCorrupted;
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
            Initialize();

            this.handler.EnsureFileExisting();
            var data = this.handler.Load(false);

            if (data == null)
                return;

            data.OnceCorrupted = this.onceCorrupted;
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
            => EditorUtility.RevealInFinder(this.FilePath);
    }
}

#endif