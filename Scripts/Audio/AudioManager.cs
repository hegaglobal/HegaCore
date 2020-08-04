using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Cysharp.Threading.Tasks;

namespace HegaCore
{
    using AudioClipOp = AsyncOperationHandle<AudioClip>;
    using AudioMap = Dictionary<string, AsyncOperationHandle<AudioClip>>;

    public sealed class AudioManager : SingletonBehaviour<AudioManager>
    {
        [SerializeField]
        private AudioMixer audioMixer = null;

        [SerializeField]
        private AudioSource musicSource = null;

        [SerializeField]
        private AudioSource soundSource = null;

        [SerializeField]
        private AudioSource voiceSource = null;

        [SerializeField]
        private MusicRegisterer musicRegisterer = null;

        [SerializeField]
        private SoundRegisterer soundRegisterer = null;

        [SerializeField]
        private VoiceRegisterer voiceRegisterer = null;

        public AudioPlayer Player { get; private set; }

        private readonly AudioMap musicMap;
        private readonly AudioMap soundMap;
        private readonly AudioMap voiceMap;

        public AudioManager()
        {
            this.musicMap = new AudioMap();
            this.soundMap = new AudioMap();
            this.voiceMap = new AudioMap();
        }

        private void Awake()
        {
            this.Player = new AudioPlayer(this, this.audioMixer, this.musicSource, this.soundSource, this.voiceSource);
        }

        public async UniTask InitializeAsync(float musicFadeTime, float musicVolume, float soundVolume, float voiceVolume)
        {
            UnuLogger.Log("Initializing music...");
            await this.musicRegisterer.RegisterAsync(this);

            UnuLogger.Log("Initializing sound...");
            await this.soundRegisterer.RegisterAsync(this);

            UnuLogger.Log("Initializing voice...");
            await this.voiceRegisterer.RegisterAsync(this);

            this.Player.Initialize(musicFadeTime, musicVolume, soundVolume, voiceVolume);
        }

        public async UniTask PrepareMusicAsync(params AssetReferenceAudioClip[] references)
            => await PrepareAsync(this.musicMap, references);

        public async UniTask PrepareSoundAsync(params AssetReferenceAudioClip[] references)
            => await PrepareAsync(this.soundMap, references);

        public async UniTask PrepareVoiceAsync(params AssetReferenceAudioClip[] references)
            => await PrepareAsync(this.voiceMap, references);

        private async UniTask PrepareAsync(AudioMap map, params AssetReferenceAudioClip[] references)
        {
            foreach (var reference in references)
            {
                var key = reference.RuntimeKey.ToString();

                if (map.ContainsKey(key))
                    continue;

                var handle = reference.LoadAssetAsync();
                await handle.Task;

                map.Add(key, handle);
            }
        }

        public async UniTask PrepareMusicAsync(params string[] keys)
            => await PrepareAsync(this.musicMap, keys);

        public async UniTask PrepareSoundAsync(params string[] keys)
            => await PrepareAsync(this.soundMap, keys);

        public async UniTask PrepareVoiceAsync(params string[] keys)
            => await PrepareAsync(this.voiceMap, keys);

        private async UniTask PrepareAsync(AudioMap map, params string[] keys)
        {
            foreach (var key in keys)
            {
                if (string.IsNullOrEmpty(key))
                    continue;

                if (map.ContainsKey(key))
                    continue;

                var handle = Addressables.LoadAssetAsync<AudioClip>(key);
                await handle.Task;

                map.Add(key, handle);
            }
        }

        public void ReleaseMusic(params AssetReferenceAudioClip[] references)
            => Release(this.musicMap, references);

        public void ReleaseSound(params AssetReferenceAudioClip[] references)
            => Release(this.soundMap, references);

        public void ReleaseVoice(params AssetReferenceAudioClip[] references)
            => Release(this.voiceMap, references);

        private void Release(AudioMap map, params AssetReferenceAudioClip[] references)
        {
            foreach (var reference in references)
            {
                var key = reference.RuntimeKey.ToString();

                if (!map.ContainsKey(key))
                    continue;

                map.Remove(key);
                reference.ReleaseAsset();
            }
        }

        public void ReleaseMusic(params string[] keys)
            => Release(this.musicMap, keys);

        public void ReleaseSound(params string[] keys)
            => Release(this.soundMap, keys);

        public void ReleaseVoice(params string[] keys)
            => Release(this.voiceMap, keys);

        private void Release(AudioMap map, params string[] keys)
        {
            foreach (var key in keys)
            {
                if (!map.TryGetValue(key, out var op))
                    continue;

                map.Remove(key);
                Addressables.Release(op);
            }
        }

        public bool TryGetMusic(AssetReferenceAudioClip reference, out AudioClip music)
            => TryGetAudio(reference, this.musicMap, out music);

        public bool TryGetSound(AssetReferenceAudioClip reference, out AudioClip sound)
            => TryGetAudio(reference, this.soundMap, out sound);

        public bool TryGetVoice(AssetReferenceAudioClip reference, out AudioClip voice)
            => TryGetAudio(reference, this.voiceMap, out voice);

        private bool TryGetAudio(AssetReferenceAudioClip reference, AudioMap map, out AudioClip clip)
        {
            var key = reference.RuntimeKey.ToString();
            clip = null;

            if (map.TryGetValue(key, out var op))
                clip = op.Result;

            return clip;
        }

        public bool TryGetMusic(string key, out AudioClip music)
            => TryGetAudio(key, this.musicMap, out music);

        public bool TryGetSound(string key, out AudioClip sound)
            => TryGetAudio(key, this.soundMap, out sound);

        public bool TryGetVoice(string key, out AudioClip voice)
            => TryGetAudio(key, this.voiceMap, out voice);

        private bool TryGetAudio(string key, AudioMap map, out AudioClip clip)
        {
            clip = null;

            if (map.TryGetValue(key, out var op))
                clip = op.Result;

            return clip;
        }
    }
}