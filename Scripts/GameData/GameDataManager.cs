using System;
using System.Collections.Generic;
using UnityEngine;

namespace HegaCore
{
    public abstract class GameDataManager<TPlayerData, TGameData, THandler, TContainer, TManager> : Singleton<TManager>
        where TPlayerData : PlayerData<TPlayerData>, new()
        where TGameData : GameData<TPlayerData>, new()
        where THandler : GameDataHandler<TPlayerData, TGameData>, new()
        where TContainer : GameDataContainer<TPlayerData, TGameData, THandler>, new()
        where TManager : GameDataManager<TPlayerData, TGameData, THandler, TContainer, TManager>, new()
    {
        public TContainer Container { get; }

        public TGameData Data => this.Container.Data;

        public THandler Handler => this.Container.Handler;

        public ReadList<ScreenResolution> Resolutions
            => this.resolutions;

        public ListSegment<string> Languages { get; private set; }

        private readonly List<ScreenResolution> resolutions;

        public GameDataManager()
        {
            this.resolutions = new List<ScreenResolution>();
            this.Container = new TContainer();
        }

        public void Initialize(DatabaseConfig config, in ListSegment<string> languages, in SizeInt resolution, bool daemon, bool darkLord)
        {
            this.Languages = languages;
            GameSettings.DefaultResolution = new ScreenResolution(resolution.Width, resolution.Height);
            GetResolutions(GameSettings.DefaultResolution);
            EnsureDefaultResolutions();

#if UNITY_EDITOR
            this.Handler.Initialize(config.SaveData.FolderFullPathEditor, config.SaveData.FileFullPathEditor, config.SaveData.Extension);
#else
            this.Handler.Initialize(config.SaveData.FolderFullPath, config.SaveData.FileFullPath, config.SaveData.Extension);
#endif
            this.Handler.EnsureFileExisting();

            this.Container.Daemon = daemon;
            this.Container.DarkLord = darkLord;
            this.Container.Load();

            EnsureResolution();
        }

        private void GetResolutions(in ScreenResolution designedRes)
        {
            var resolutions = Screen.resolutions;
            var defaultRatio = designedRes.Height * 1f / designedRes.Width;

            this.resolutions.Clear();

            for (var i = resolutions.Length - 1; i >= 0; i--)
            {
                var resolution = resolutions[i];
                var height = (int)(resolution.width * defaultRatio);

                if (resolution.height != height)
                    continue;

                if (this.resolutions.Contains(resolution))
                    continue;

                this.resolutions.Add(resolution);
            }
        }

        private void EnsureDefaultResolutions()
        {
            if (this.resolutions.Count > 0)
                return;

            this.resolutions.Add(new ScreenResolution(1920, 1080));
            this.resolutions.Add(new ScreenResolution(1600, 900));
            this.resolutions.Add(new ScreenResolution(1366, 768));
            this.resolutions.Add(new ScreenResolution(1280, 720));
        }

        private void EnsureResolution()
        {
            var settings = this.Data.Settings;
            var index = this.Resolutions.FindIndex(x => x == settings.Resolution);

            if (index < 0)
            {
                settings.Resolution = this.Resolutions[0];
                this.Container.Save();
            }

            settings.Resolution.Apply(settings.Fullscreen);
        }
    }
}