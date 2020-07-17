using System;
using System.Collections.Generic;
using UnuGames;
using UnuGames.MVVM;

namespace HegaCore
{
    public partial class UISettingsDialog : UIManDialog
    {
        public static class Settings
        {
            public static GameSettings Data { get; private set; }

            public static ListSegment<ScreenResolution> Resolutions { get; private set; }

            public static ListSegment<string> Languages { get; private set; }

            public static AudioPlayer AudioPlayer { get; private set; }

            public static Action Save { get; private set; }

            public static void Initialize(GameSettings data, in ListSegment<ScreenResolution> resolutions,
                                          in ListSegment<string> languages, AudioPlayer audioPlayer, Action save)
            {
                Data = data;
                Resolutions = resolutions;
                Languages = languages;
                AudioPlayer = audioPlayer;
                Save = save;
            }
        }

        public static void Show()
        {
            UIMan.Instance.ShowDialog<UISettingsDialog>();
        }

        public static void Hide()
        {
            UIMan.Instance.DestroyUI<UISettingsDialog>();
        }

        [UIManProperty]
        public ObservableList<ScreenResolution> Resolutions { get; }
            = new ObservableList<ScreenResolution>();

        [UIManProperty]
        public ObservableList<Language> Languages { get; }
            = new ObservableList<Language>();

        public override void OnShow(params object[] args)
        {
            base.OnShow(args);
            Initialize();
        }

        public override void OnHide()
        {
            base.OnHide();
            Deinitialize();
        }

        private void OnApplicationQuit()
        {
            Settings.Save?.Invoke();
        }

        public void UI_Button_Close()
        {
            Settings.Save?.Invoke();
            Hide();
        }

        private void Initialize()
        {
            this.Music = Settings.Data.MusicVolume;
            this.Sound = Settings.Data.SoundVolume;
            this.Voice = Settings.Data.VoiceVolume;
            this.Fullscreen = Settings.Data.Fullscreen;

            RefreshResolutions(Settings.Resolutions);
            RefreshLanguages(Settings.Languages);

            SubscribeAction(nameof(this.Music), Music_OnChanged);
            SubscribeAction(nameof(this.Sound), Sound_OnChanged);
            SubscribeAction(nameof(this.Voice), Voice_OnChanged);
            SubscribeAction(nameof(this.SelectedResolution), SelectedResolution_OnChanged);
            SubscribeAction(nameof(this.Fullscreen), Fullscreen_OnChanged);
            SubscribeAction(nameof(this.SelectedLanguage), SelectedLanguage_OnChanged);
        }

        private void Deinitialize()
        {
            UnsubscribeAction(nameof(this.Music), Music_OnChanged);
            UnsubscribeAction(nameof(this.Sound), Sound_OnChanged);
            UnsubscribeAction(nameof(this.Voice), Voice_OnChanged);
            UnsubscribeAction(nameof(this.SelectedResolution), SelectedResolution_OnChanged);
            UnsubscribeAction(nameof(this.Fullscreen), Fullscreen_OnChanged);
            UnsubscribeAction(nameof(this.SelectedLanguage), SelectedLanguage_OnChanged);
        }

        private void RefreshResolutions(in Segment<ScreenResolution> resolutions)
        {
            this.Resolutions.Clear();
            this.Resolutions.AddRange(resolutions);

            var index = this.Resolutions.FindIndex(x => x == Settings.Data.Resolution);

            if (index >= 0)
            {
                this.SelectedResolution = index;
                return;
            }

            Settings.Data.Resolution = this.Resolutions[0];
            this.SelectedResolution = 0;
        }

        private void RefreshLanguages(in Segment<string> languages)
        {
            this.Languages.Clear();

            foreach (var key in languages)
            {
                var name = L10n.Localize(key, key);
                this.Languages.Add(new Language(key, name));
            }

            var index = this.Languages.FindIndex(x => string.Equals(x.Key, Settings.Data.Language));

            if (index >= 0)
            {
                this.SelectedLanguage = index;
                return;
            }

            Settings.Data.Language = this.Languages[0].Key;
            this.SelectedLanguage = 0;
        }

        private void Music_OnChanged(object value)
        {
            Settings.Data.MusicVolume = this.Music;
            Settings.AudioPlayer.ChangeMusicVolume(this.Music);
        }

        private void Sound_OnChanged(object value)
        {
            Settings.Data.SoundVolume = this.Sound;
            Settings.AudioPlayer.ChangeSoundVolume(this.Sound);
        }

        private void Voice_OnChanged(object value)
        {
            Settings.Data.VoiceVolume = this.Voice;
            Settings.AudioPlayer.ChangeVoiceVolume(this.Voice);
        }

        private void SelectedResolution_OnChanged(object value)
        {
            Settings.Data.Resolution = this.Resolutions[this.SelectedResolution];
            ApplyResolution();
        }

        private void Fullscreen_OnChanged(object value)
        {
            Settings.Data.Fullscreen = this.Fullscreen;
            ApplyResolution();
        }

        private void SelectedLanguage_OnChanged(object value)
        {
            Settings.Data.Language = this.Languages[this.SelectedLanguage].Key;
            L10n.Relocalize();
        }

        private void ApplyResolution()
            => Settings.Data.Resolution.Apply(Settings.Data.Fullscreen);
    }
}