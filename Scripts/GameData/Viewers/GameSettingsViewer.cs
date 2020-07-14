#if UNITY_EDITOR

using System;
using UnityEngine;

namespace HegaCore.Editor
{
    [Serializable]
    public sealed class GameSettingsViewer : MonoBehaviour
    {
        [SerializeField]
        private int Revision;

        [SerializeField]
        private string Language;

        [SerializeField]
        private float MusicVolume;

        [SerializeField]
        private float SoundVolume;

        [SerializeField]
        private float VoiceVolume;

        [SerializeField]
        private ScreenResolution Resolution;

        [SerializeField]
        private bool Fullscreen;

        public void Set(GameSettings data)
        {
            this.Revision = data.Revision;
            this.Language = data.Language;
            this.MusicVolume = data.MusicVolume;
            this.SoundVolume = data.SoundVolume;
            this.VoiceVolume = data.VoiceVolume;
            this.Resolution = data.Resolution;
            this.Fullscreen = data.Fullscreen;
        }

        public void CopyTo(GameSettings data)
        {
            data.Revision = this.Revision;
            data.Language = this.Language;
            data.MusicVolume = this.MusicVolume;
            data.SoundVolume = this.SoundVolume;
            data.VoiceVolume = this.VoiceVolume;
            data.Resolution = this.Resolution;
            data.Fullscreen = this.Fullscreen;
        }
    }
}

#endif