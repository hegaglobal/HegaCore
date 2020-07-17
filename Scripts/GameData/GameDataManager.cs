using System;
using System.Collections.Generic;
using UnityEngine;

namespace HegaCore
{
    public abstract class GameDataManager<T> : Singleton<T> where T : GameDataManager<T>, new()
    {
        public ReadList<ScreenResolution> Resolutions
            => this.resolutions;

        public ListSegment<string> Languages { get; private set; }

        private readonly List<ScreenResolution> resolutions;

        public GameDataManager()
        {
            this.resolutions = new List<ScreenResolution>();
        }

        protected void Initialize(in ListSegment<string> languages, in SizeInt resolution)
        {
            this.Languages = languages;

            GameSettings.DefaultResolution = new ScreenResolution(resolution.Width, resolution.Height);
            GetResolutions(GameSettings.DefaultResolution);
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
    }
}