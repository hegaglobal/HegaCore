using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using Cysharp.Threading.Tasks;

namespace HegaCore
{
    public sealed class AudioPlayer
    {
        private const string MusicVolume = nameof(MusicVolume);
        private const string InnerMusicVolume = nameof(InnerMusicVolume);
        private const string SoundVolume = nameof(SoundVolume);
        private const string VoiceVolume = nameof(VoiceVolume);
        private const string VoiceBGVolume = nameof(VoiceBGVolume);
        private const string InnerVoiceBGVolume = nameof(InnerVoiceBGVolume);

        private const float SoundBufferClearDelay = 0.04f;

        private readonly AudioManager manager;
        private readonly AudioMixer mixer;
        private readonly AudioSource music;
        private readonly AudioSource sound;
        private readonly AudioSource voice;
        private readonly AudioSource voiceBG;
        private readonly List<string> soundBuffer;

        private float musicFadeTime;
        private string currentMusicKey = string.Empty;
        private string currentVoiceKey = string.Empty;
        private string currentVoiceBGKey = string.Empty;
        private AssetReferenceAudioClip currentMusicRef = null;
        private AssetReferenceAudioClip currentVoiceRef = null;
        private AssetReferenceAudioClip currentVoiceBGRef = null;

        private AudioClip musicClip;
        private Tweener musicFadeOut;
        private Tweener musicFadeIn;

        private AudioClip voiceBGClip;
        private Tweener voiceBGFadeOut;
        private Tweener voiceBGFadeIn;

        public AudioPlayer(AudioManager manager, AudioMixer mixer, AudioSource music, AudioSource sound,
                           AudioSource voice, AudioSource voiceBG)
        {
            this.manager = manager;
            this.mixer = mixer;
            this.music = music;
            this.sound = sound;
            this.voice = voice;
            this.voiceBG = voiceBG;
            this.soundBuffer = new List<string>();
        }

        public void Initialize(float musicFadeTime, float musicVolume, float soundVolume, float voiceVolume,
                               float? bufferClearDelay = null)
        {
            this.musicFadeTime = musicFadeTime;

            ChangeMusicVolume(musicVolume);
            ChangeSoundVolume(soundVolume);
            ChangeVoiceVolume(voiceVolume);
            SoundBufferRoutine(bufferClearDelay ?? SoundBufferClearDelay).Forget();
        }

        private async UniTaskVoid SoundBufferRoutine(float bufferClearDelay)
        {
            var time = 0f;

            while (true)
            {
                time += GameTime.Provider.UnscaledDeltaTime;

                await UniTask.DelayFrame(1);

                if (time >= bufferClearDelay)
                {
                    this.soundBuffer.Clear();
                    time = 0f;
                }
            }
        }

        public void ChangeMusicVolume(float value)
            => this.mixer.SetFloat(InnerMusicVolume, ToVolume(value));

        public void ChangeSoundVolume(float value)
            => this.mixer.SetFloat(SoundVolume, ToVolume(value));

        public void ChangeVoiceVolume(float value)
        {
            var volume = ToVolume(value);
            this.mixer.SetFloat(VoiceVolume, volume);
            this.mixer.SetFloat(InnerVoiceBGVolume, volume);
        }

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
                    PlayVoice(key);
                    break;

                case AudioType.VoiceBG:
                    PlayVoiceBG(key);
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
                    PlayVoice(reference);
                    break;

                case AudioType.VoiceBG:
                    PlayVoiceBG(reference);
                    break;
            }
        }

        public void PlayAsync(string key, AudioType type, bool silent = true)
        {
            switch (type)
            {
                case AudioType.Music:
                    PlayMusicAsync(key, silent).Forget();
                    break;

                case AudioType.Sound:
                    PlaySoundAsync(key, silent).Forget();
                    break;

                case AudioType.Voice:
                    PlayVoiceAsync(key, silent).Forget();
                    break;

                case AudioType.VoiceBG:
                    PlayVoiceBGAsync(key, silent).Forget();
                    break;
            }
        }

        public void PlayAsync(AssetReferenceAudioClip reference, AudioType type, bool silent = true)
        {
            switch (type)
            {
                case AudioType.Music:
                    PlayMusicAsync(reference, silent).Forget();
                    break;

                case AudioType.Sound:
                    PlaySoundAsync(reference, silent).Forget();
                    break;

                case AudioType.Voice:
                    PlayVoiceAsync(reference, silent).Forget();
                    break;

                case AudioType.VoiceBG:
                    PlayVoiceBGAsync(reference, silent).Forget();
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

                case AudioType.VoiceBG:
                    StopVoiceBG();
                    break;
            }
        }

        private bool CanPlayMusic(string key)
            => !this.music.isPlaying ||
               !string.Equals(this.currentMusicKey, key);

        public void PauseMusic()
        {
            if (!this.music.isPlaying)
                return;

            this.music.Pause();
        }

        public void ResumeMusic()
        {
            if (this.music.isPlaying)
                return;

            this.music.Play();
        }

        public void PlayMusic(string key)
        {
            if (!CanPlayMusic(key))
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

        public void PlayMusic(string key, AudioClip clip)
        {
            if (!clip)
            {
                PlayMusic(key);
                return;
            }

            if (!CanPlayMusic(key))
                return;

            this.currentMusicKey = key;
            this.musicClip = clip;
            FadeMusicPlay();
        }

        public async UniTaskVoid PlayMusicAsync(string key, bool silent = true)
        {
            await this.manager.PrepareMusicAsync(silent, key);

            PlayMusic(key);
        }

        public void PlayMusicAsync(string key, AudioClip clip, bool silent = true)
        {
            if (!clip)
            {
                PlayMusicAsync(key, silent).Forget();
                return;
            }

            if (!CanPlayMusic(key))
                return;

            this.currentMusicKey = key;
            this.musicClip = clip;
            FadeMusicPlay();
        }

        public async UniTaskVoid PlayMusicAsync(AssetReferenceAudioClip key, bool silent = true)
        {
            await this.manager.PrepareMusicAsync(silent, key);

            PlayMusic(key);
        }

        public void PlaySound(string key)
        {
            if (!this.manager.TryGetSound(key, out var clip))
                return;

            if (this.soundBuffer.Contains(key))
                return;

            this.soundBuffer.Add(key);
            this.sound.PlayOneShot(clip);
        }

        public void PlaySound(string key, AudioClip clip)
        {
            if (clip)
            {
                this.sound.PlayOneShot(clip);
                return;
            }

            PlaySound(key);
        }

        public async UniTaskVoid PlaySoundAsync(string key, bool silent = true)
        {
            await this.manager.PrepareSoundAsync(silent, key);

            PlaySound(key);
        }

        public void PlaySoundAsync(string key, AudioClip clip, bool silent = true)
        {
            if (!clip)
            {
                PlaySoundAsync(key, silent).Forget();
                return;
            }

            this.sound.PlayOneShot(clip);
        }

        public async UniTaskVoid PlaySoundAsync(AssetReferenceAudioClip key, bool silent = true)
        {
            await this.manager.PrepareSoundAsync(silent, key);

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
                this.voice.loop = false;
                this.voice.Play();
            }
            else
            {
                this.currentVoiceKey = string.Empty;
                this.voice.Stop();
            }
        }

        public void PlayVoiceLoop(string key)
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
                this.voice.loop = true;
                this.voice.Play();
            }
            else
            {
                this.currentVoiceKey = string.Empty;
                this.voice.Stop();
            }
        }

        public async UniTaskVoid PlayVoiceAsync(string key, bool silent = true)
        {
            await this.manager.PrepareVoiceAsync(silent, key);

            PlayVoice(key);
        }

        public async UniTaskVoid PlayVoiceAsync(AssetReferenceAudioClip key, bool silent = true)
        {
            await this.manager.PrepareVoiceAsync(silent, key);

            PlayVoice(key);
        }

        private bool CanPlayVoiceBG(string key)
            => !this.voiceBG.isPlaying ||
               !string.Equals(this.currentVoiceBGKey, key);

        public void PlayVoiceBG(string key)
        {
            if (!CanPlayVoiceBG(key))
                return;

            if (this.manager.TryGetVoiceBG(key, out this.voiceBGClip))
            {
                this.currentVoiceBGKey = key;
                FadeVoiceBGPlay();
            }
            else
            {
                this.currentVoiceBGKey = string.Empty;
                FadeVoiceBGStop();
            }
        }

        public void PlayVoiceBG(string key, AudioClip clip)
        {
            if (!clip)
            {
                PlayVoiceBG(key);
                return;
            }

            if (!CanPlayVoiceBG(key))
                return;

            this.currentVoiceBGKey = key;
            this.voiceBGClip = clip;
            FadeVoiceBGPlay();
        }

        public async UniTaskVoid PlayVoiceBGAsync(string key, bool silent = true)
        {
            await this.manager.PrepareVoiceBGAsync(silent, key);

            PlayVoiceBG(key);
        }

        public void PlayVoiceBGAsync(string key, AudioClip clip, bool silent = true)
        {
            if (!clip)
            {
                PlayVoiceBGAsync(key, silent).Forget();
                return;
            }

            if (!CanPlayVoiceBG(key))
                return;

            this.currentVoiceBGKey = key;
            this.voiceBGClip = clip;
            FadeVoiceBGPlay();
        }

        public async UniTaskVoid PlayVoiceBGAsync(AssetReferenceAudioClip key, bool silent = true)
        {
            await this.manager.PrepareVoiceBGAsync(silent, key);

            PlayVoiceBG(key);
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
            if (!this.manager.TryGetSound(reference, out var clip))
                return;

            var key = reference.RuntimeKey.ToString();

            if (this.soundBuffer.Contains(key))
                return;

            this.soundBuffer.Add(key);
            this.sound.PlayOneShot(clip);
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

        public void PlayVoiceBG(AssetReferenceAudioClip reference)
        {
            if (this.voiceBG.isPlaying &&
                this.currentVoiceBGRef == reference)
                return;

            if (this.manager.TryGetVoiceBG(reference, out this.voiceBGClip))
            {
                this.currentVoiceBGRef = reference;
                FadeVoiceBGPlay();
            }
            else
            {
                this.currentVoiceBGRef = null;
                FadeVoiceBGStop();
            }
        }

        public void StopMusic()
            => FadeMusicStop();

        public void StopSound()
            => this.sound.Stop();

        public void StopVoice()
            => this.voice.Stop();

        public void StopVoiceBG()
            => this.voiceBG.Stop();

        public void StopAllVoices()
        {
            StopVoice();
            StopVoiceBG();
        }

        public void SetMusicLoop(bool value)
            => this.music.loop = value;

        public void SetVoiceBGLoop(bool value)
            => this.voiceBG.loop = value;

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

        private void FadeVoiceBGPlay()
        {
            this.voiceBGFadeIn?.Kill();

            FadeVoiceBGStop();

            this.voiceBGFadeOut.OnComplete(FadeVoiceBGPlayNext);
        }

        private void FadeVoiceBGPlayNext()
        {
            if (!this.voiceBGClip)
                return;

            this.voiceBG.clip = this.voiceBGClip;
            this.voiceBG.Play();

            this.voiceBGFadeIn = this.mixer.DOSetFloat(VoiceBGVolume, ToVolume(1f), this.musicFadeTime)
                                           .SetEase(Ease.Linear);
        }

        private void FadeVoiceBGStop()
        {
            this.voiceBGFadeOut?.Kill();
            this.voiceBGFadeOut = this.mixer.DOSetFloat(VoiceBGVolume, ToVolume(0f), this.musicFadeTime)
                                            .SetEase(Ease.Linear)
                                            .OnComplete(OnFadeVoiceBGStopComplete);
        }

        private void OnFadeVoiceBGStopComplete()
        {
            this.voiceBG.Stop();
        }
    }
}