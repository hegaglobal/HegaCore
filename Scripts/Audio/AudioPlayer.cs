using UnityEngine;
using UnityEngine.Audio;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace HegaCore
{
    public sealed class AudioPlayer
    {
        private const string MusicVolume = nameof(MusicVolume);
        private const string InnerMusicVolume = nameof(InnerMusicVolume);
        private const string SoundVolume = nameof(SoundVolume);
        private const string VoiceVolume = nameof(VoiceVolume);

        private readonly AudioManager manager;
        private readonly AudioMixer mixer;
        private readonly AudioSource music;
        private readonly AudioSource sound;
        private readonly AudioSource voice;

        private float musicFadeTime;
        private string currentMusicKey = string.Empty;
        private string currentVoiceKey = string.Empty;
        private AssetReferenceAudioClip currentMusicRef = null;
        private AssetReferenceAudioClip currentVoiceRef = null;

        private AudioClip musicClip;
        private Tweener musicFadeOut;
        private Tweener musicFadeIn;

        public AudioPlayer(AudioManager manager, AudioMixer mixer, AudioSource music, AudioSource sound, AudioSource voice)
        {
            this.manager = manager;
            this.mixer = mixer;
            this.music = music;
            this.sound = sound;
            this.voice = voice;
        }

        public void Initialize(float musicFadeTime, float musicVolume, float soundVolume, float voiceVolume)
        {
            this.musicFadeTime = musicFadeTime;

            ChangeMusicVolume(musicVolume);
            ChangeSoundVolume(soundVolume);
            ChangeVoiceVolume(voiceVolume);
        }

        public void ChangeMusicVolume(float value)
            => this.mixer.SetFloat(InnerMusicVolume, ToVolume(value));

        public void ChangeSoundVolume(float value)
            => this.mixer.SetFloat(SoundVolume, ToVolume(value));

        public void ChangeVoiceVolume(float value)
            => this.mixer.SetFloat(VoiceVolume, ToVolume(value));

        private static float ToVolume(float value)
        {
            const float min = -80f;
            const float middle = -40f;
            const float offset = 40f;

            var scale = Mathf.Clamp(value, 0f, 1f);
            var volume = (offset * scale) + offset + min;

            return volume > middle ? volume : min;
        }

        public void Play(string key, AudioType type)
        {
            switch (type)
            {
                case AudioType.Music:
                    PlayMusic(key);
                    break;

                case AudioType.Sound:
                    PlaySound(key);
                    break;

                case AudioType.Voice:
                    break;
            }
        }

        public void Play(AssetReferenceAudioClip reference, AudioType type)
        {
            switch (type)
            {
                case AudioType.Music:
                    PlayMusic(reference);
                    break;

                case AudioType.Sound:
                    PlaySound(reference);
                    break;

                case AudioType.Voice:
                    break;
            }
        }

        public void Stop(AudioType type)
        {
            switch (type)
            {
                case AudioType.Music:
                    StopMusic();
                    break;

                case AudioType.Sound:
                    StopSound();
                    break;

                case AudioType.Voice:
                    StopVoice();
                    break;
            }
        }

        public void PlayMusic(string key)
        {
            if (this.music.isPlaying &&
                string.Equals(this.currentMusicKey, key))
                return;

            if (this.manager.TryGetMusic(key, out this.musicClip))
            {
                this.currentMusicKey = key;
                FadeMusicPlay();
            }
            else
            {
                this.currentMusicKey = string.Empty;
                FadeMusicStop();
            }
        }

        public async UniTaskVoid PlayMusicAsync(string key)
        {
            await this.manager.PrepareMusicAsync(key);

            PlayMusic(key);
        }

        public async UniTaskVoid PlayMusicAsync(AssetReferenceAudioClip key)
        {
            await this.manager.PrepareMusicAsync(key);

            PlayMusic(key);
        }

        public void PlaySound(string key)
        {
            if (this.manager.TryGetSound(key, out var clip))
                this.sound.PlayOneShot(clip);
        }

        public async UniTaskVoid PlaySoundAsync(string key)
        {
            await this.manager.PrepareSoundAsync(key);

            PlaySound(key);
        }

        public async UniTaskVoid PlaySoundAsync(AssetReferenceAudioClip key)
        {
            await this.manager.PrepareSoundAsync(key);

            PlaySound(key);
        }

        public void PlayVoice(string key)
        {
            if (this.voice.isPlaying &&
                string.Equals(this.currentVoiceKey, key))
                return;

            this.voice.Stop();
            this.voice.clip = null;

            if (this.manager.TryGetVoice(key, out var voiceClip))
            {
                this.currentVoiceKey = key;
                this.voice.clip = voiceClip;
                this.voice.Play();
            }
            else
            {
                this.currentVoiceKey = string.Empty;
                this.voice.Stop();
            }
        }

        public async UniTaskVoid PlayVoiceAsync(string key)
        {
            await this.manager.PrepareVoiceAsync(key);

            PlayVoice(key);
        }

        public async UniTaskVoid PlayVoiceAsync(AssetReferenceAudioClip key)
        {
            await this.manager.PrepareVoiceAsync(key);

            PlayVoice(key);
        }

        public void PlayMusic(AssetReferenceAudioClip reference)
        {
            if (this.music.isPlaying &&
                this.currentMusicRef == reference)
                return;

            if (this.manager.TryGetMusic(reference, out this.musicClip))
            {
                this.currentMusicRef = reference;
                FadeMusicPlay();
            }
            else
            {
                this.currentMusicRef = null;
                FadeMusicStop();
            }
        }

        public void PlaySound(AssetReferenceAudioClip reference)
        {
            if (this.manager.TryGetSound(reference, out var clip))
            {
                this.sound.PlayOneShot(clip);
            }
        }

        public void PlayVoice(AssetReferenceAudioClip reference)
        {
            if (this.voice.isPlaying &&
                this.currentVoiceRef == reference)
                return;

            if (this.manager.TryGetVoice(reference, out var voiceClip))
            {
                this.currentVoiceRef = reference;
                this.voice.clip = voiceClip;
                this.voice.Play();
            }
            else
            {
                this.currentVoiceRef = null;
                this.voice.Stop();
            }
        }

        public void StopMusic()
            => FadeMusicStop();

        public void StopSound()
            => this.sound.Stop();

        public void StopVoice()
            => this.voice.Stop();

        private void FadeMusicPlay()
        {
            this.musicFadeIn?.Kill();

            FadeMusicStop();

            this.musicFadeOut.OnComplete(FadeMusicPlayNext);
        }

        private void FadeMusicPlayNext()
        {
            if (!this.musicClip)
                return;

            this.music.clip = this.musicClip;
            this.music.Play();

            this.musicFadeIn = this.mixer.DOSetFloat(MusicVolume, ToVolume(1f), this.musicFadeTime)
                                         .SetEase(Ease.Linear);
        }

        private void FadeMusicStop()
        {
            this.musicFadeOut?.Kill();
            this.musicFadeOut = this.mixer.DOSetFloat(MusicVolume, ToVolume(0f), this.musicFadeTime)
                                          .SetEase(Ease.Linear)
                                          .OnComplete(OnFadeMusicStopComplete);
        }

        private void OnFadeMusicStopComplete()
        {
            this.music.Stop();
        }
    }
}