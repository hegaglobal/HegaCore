using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnuGames;
using UnuGames.MVVM;

namespace HegaCore.UI
{
    public partial class UISettingsDialog : UIManDialog
    {
        public static void Show(Action onHideComplete = null, Action onGiveUp = null)
        {
            UIMan.Instance.ShowDialog<UISettingsDialog>(onHideComplete, onGiveUp);
        }

        public static void Hide(bool deactive = true)
        {
            UIMan.Instance.HideDialog<UISettingsDialog>(deactive);
        }

        [UIManProperty]
        public ObservableList<ScreenResolution> Resolutions { get; }
            = new ObservableList<ScreenResolution>();

        [UIManProperty]
        public ObservableList<Language> Languages { get; }
            = new ObservableList<Language>();

        [ShowIf("@UnityEngine.Application.isPlaying")]
        private IInitializable<UISettingsDialog>[] initializables;

        [ShowIf("@UnityEngine.Application.isPlaying")]
        private IDeinitializable<UISettingsDialog>[] deinitializables;

        private Action onHideCompleted;
        private Action onClickGiveUp;
        
        private void Awake()
        {
            this.initializables = GetComponentsInChildren<IInitializable<UISettingsDialog>>().OrEmpty();
            this.deinitializables = GetComponentsInChildren<IDeinitializable<UISettingsDialog>>().OrEmpty();
        }

        public override void OnShow(params object[] args)
        {
            base.OnShow(args);
            var index = 0;

            args.GetThenMoveNext(ref index, out this.onHideCompleted);
            args.GetThenMoveNext(ref index, out this.onClickGiveUp);
            CanGiveUp = (onClickGiveUp != null);
            Initialize();
        }

        public override void OnHideComplete()
        {
            base.OnHideComplete();
            this.onHideCompleted?.Invoke();
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

        public void UI_Button_GiveUp()
        {
            onClickGiveUp?.Invoke();
            UI_Button_Close();
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

            if (this.initializables != null && this.initializables.Length > 0)
            {
                for (var i = 0; i < this.initializables.Length; i++)
                {
                    this.initializables[i]?.Initialize(this);
                }
            }
        }

        private void Deinitialize()
        {
            UnsubscribeAction(nameof(this.Music), Music_OnChanged);
            UnsubscribeAction(nameof(this.Sound), Sound_OnChanged);
            UnsubscribeAction(nameof(this.Voice), Voice_OnChanged);
            UnsubscribeAction(nameof(this.SelectedResolution), SelectedResolution_OnChanged);
            UnsubscribeAction(nameof(this.Fullscreen), Fullscreen_OnChanged);
            UnsubscribeAction(nameof(this.SelectedLanguage), SelectedLanguage_OnChanged);

            if (this.deinitializables != null && this.deinitializables.Length > 0)
            {
                for (var i = 0; i < this.deinitializables.Length; i++)
                {
                    this.deinitializables[i]?.Deinitialize(this);
                }
            }
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

        private void SelectedLanguage_OnChanged(object value)
        {
            Settings.Data.Language = this.Languages[this.SelectedLanguage].Key;
            L10n.Relocalize();
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

        private void ApplyResolution()
            => Settings.Data.Resolution.Apply(Settings.Data.Fullscreen);
    }
}