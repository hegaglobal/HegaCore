using System;
using UnityEngine;

namespace HegaCore
{
    [Serializable]
    public abstract class GameSettings : ILanguage
    {
        public const int CurrentRevision = 1;

        public const string DefaultLanguage = "en";

        public static ScreenResolution DefaultResolution { get; set; }

        public int Revision;

        public string Language;

        public float MusicVolume;

        public float SoundVolume;

        public float VoiceVolume;

        public float InterfaceVolume;

        public ScreenResolution Resolution;

        public bool Fullscreen;

        public int Framerate;

        public bool allowDragPreview;
        
        protected GameSettings()
        {
            this.Revision = 1;
            this.Language = DefaultLanguage;
            this.MusicVolume = 0.65f;
            this.SoundVolume = 1f;
            this.VoiceVolume = 1f;
            this.InterfaceVolume = 1f;
            this.Resolution = default;
            this.Fullscreen = true;
            this.Framerate = 200;
            this.allowDragPreview = true;
        }

        public void CopyFrom(GameSettings data)
        {
            if (data != null)
            {
                var isOld = data.Revision < CurrentRevision;
                this.Revision = isOld ? CurrentRevision : data.Revision;

                this.Language = string.IsNullOrWhiteSpace(data.Language) ? DefaultLanguage : data.Language;
                this.MusicVolume = data.MusicVolume;
                this.SoundVolume = data.SoundVolume;
                this.VoiceVolume = data.VoiceVolume;
                this.InterfaceVolume = data.InterfaceVolume;
                this.Resolution = data.Resolution;
                this.Fullscreen = data.Fullscreen;
                this.Framerate = data.Framerate;
                this.allowDragPreview = data.allowDragPreview;
            }

            if (string.IsNullOrEmpty(this.Language))
                this.Language = DefaultLanguage;

            if (this.Resolution.Width <= 0 || this.Resolution.Height <= 0)
                this.Resolution = DefaultResolution;
        }

        public string GetLanguage()
            => this.Language;
    }
}