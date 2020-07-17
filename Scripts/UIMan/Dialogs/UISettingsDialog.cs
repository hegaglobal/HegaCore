
using UnuGames;
using UnuGames.MVVM;

// This code is generated automatically by UIMan - UI Generator, please do not modify!

namespace HegaCore
{
    public sealed partial class UISettingsDialog
    {
        private float m_music = default;

        [UIManAutoProperty]
        public float Music
        {
            get { return this.m_music; }
            set { this.m_music = value; OnPropertyChanged(nameof(this.Music), value); }
        }

        private float m_sound = default;

        [UIManAutoProperty]
        public float Sound
        {
            get { return this.m_sound; }
            set { this.m_sound = value; OnPropertyChanged(nameof(this.Sound), value); }
        }

        private float m_voice = default;

        [UIManAutoProperty]
        public float Voice
        {
            get { return this.m_voice; }
            set { this.m_voice = value; OnPropertyChanged(nameof(this.Voice), value); }
        }

        private int m_selectedResolution = default;

        [UIManAutoProperty]
        public int SelectedResolution
        {
            get { return this.m_selectedResolution; }
            set { this.m_selectedResolution = value; OnPropertyChanged(nameof(this.SelectedResolution), value); }
        }

        private bool m_fullscreen = default;

        [UIManAutoProperty]
        public bool Fullscreen
        {
            get { return this.m_fullscreen; }
            set { this.m_fullscreen = value; OnPropertyChanged(nameof(this.Fullscreen), value); }
        }

        private int m_selectedLanguage = default;

        [UIManAutoProperty]
        public int SelectedLanguage
        {
            get { return this.m_selectedLanguage; }
            set { this.m_selectedLanguage = value; OnPropertyChanged(nameof(this.SelectedLanguage), value); }
        }
    }
}
