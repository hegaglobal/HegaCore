using System;
using UnityEngine;

namespace HegaCore
{
    [Serializable]
    public class GameSettings : ILanguage
    {
        public const int CurrentRevision = 1;

#if STOVE
        public const string DefaultLanguage = "kr";
#else
        public const string DefaultLanguage = "en";
#endif

        public static ScreenResolution DefaultResolution { get; set; }

        public int Revision;

        public string Language;

        public float MusicVolume;

        public float SoundVolume;

        public float VoiceVolume;

        public ScreenResolution Resolution;

        public bool Fullscreen;

        public GameSettings()
        {
            this.Revision = 1;
            this.Language = DefaultLanguage;
            this.MusicVolume = 1f;
            this.SoundVolume = 1f;
            this.VoiceVolume = 1f;
            this.Resolution = default;
            this.Fullscreen = true;
        }

        public void Copy(GameSettings data)
        {
            if (data != null)
            {
                var isOld = data.Revision < CurrentRevision;

                this.Language = string.IsNullOrEmpty(data.Language) ? DefaultLanguage : data.Language;
                this.MusicVolume = isOld ? 1f : data.MusicVolume;
                this.SoundVolume = isOld ? 1f : data.SoundVolume;
                this.VoiceVolume = isOld ? 1f : data.VoiceVolume;
                this.Resolution = data.Resolution;
                this.Fullscreen = isOld || data.Fullscreen;
                this.Revision = isOld ? CurrentRevision : data.Revision;
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