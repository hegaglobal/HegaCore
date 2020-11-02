using System;
using System.Collections.Generic;
using System.Table;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Cysharp.Threading.Tasks;
using HegaCore.Database;

namespace HegaCore
{
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

        public async UniTask PrepareAudioAsync(ReadTable<AudioEntry> table, AudioType filterType)
        {
            foreach (var entry in table.Entries)
            {
                if (entry == null || entry.Type != filterType)
                    return;

                await PrepareAudioAsync(entry);
            }
        }

        public async UniTask PrepareAudioAsync(ReadTable<AudioEntry> table)
        {
            foreach (var entry in table.Entries)
            {
                if (entry == null)
                    return;

                await PrepareAudioAsync(entry);
            }
        }

        private async UniTask PrepareAudioAsync(AudioEntry entry)
        {
            var key = entry.Key;
            var assetKey = entry.Key;

            if (string.IsNullOrEmpty(assetKey) || !AddressablesManager.ContainsKey(assetKey))
            {
                assetKey = entry.SecondKey;

                if (string.IsNullOrEmpty(assetKey) || !AddressablesManager.ContainsKey(assetKey))
                {
                    UnuLogger.LogError($"Cannot find any {entry.Type} with either key={entry.Key} or key={entry.SecondKey}");
                    return;
                }
            }

            var map = GetMap(entry.Type);

            if (map == null)
                return;

            if (map.ContainsKey(key))
            {
                UnuLogger.LogWarning($"Duplicate {entry.Type} key={key}");
                return;
            }

            try
            {
                var handle = Addressables.LoadAssetAsync<AudioClip>(key);
                await handle.Task;

                map.Add(key, handle);
            }
            catch (Exception ex)
            {
                UnuLogger.LogException(ex, this);
            }
        }

        private AudioMap GetMap(AudioType type)
        {
            switch (type)
            {
                case AudioType.Music: return this.musicMap;
                case AudioType.Sound: return this.soundMap;
                case AudioType.Voice: return this.voiceMap;
                default: return null;
            }
        }

        public async UniTask PrepareMusicAsync(bool silent, params AssetReferenceAudioClip[] references)
            => await PrepareAsync(this.musicMap, AudioType.Music, silent, references);

        public async UniTask PrepareSoundAsync(bool silent, params AssetReferenceAudioClip[] references)
            => await PrepareAsync(this.soundMap, AudioType.Sound, silent, references);

        public async UniTask PrepareVoiceAsync(bool silent, params AssetReferenceAudioClip[] references)
            => await PrepareAsync(this.voiceMap, AudioType.Voice, silent, references);

        private async UniTask PrepareAsync(AudioMap map, AudioType type, bool silent, params AssetReferenceAudioClip[] references)
        {
            foreach (var reference in references)
            {
                var key = reference.RuntimeKey.ToString();

                if (map.ContainsKey(key))
                {
                    if (!silent)
                        UnuLogger.LogWarning($"Duplicate {type} key={key}");

                    continue;
                }

                var handle = reference.LoadAssetAsync();
                await handle.Task;

                map.Add(key, handle);
            }
        }

        public async UniTask PrepareMusicAsync(bool silent, params string[] keys)
            => await PrepareAsync(this.musicMap, AudioType.Music, silent, keys);

        public async UniTask PrepareSoundAsync(bool silent, params string[] keys)
            => await PrepareAsync(this.soundMap, AudioType.Sound, silent, keys);

        public async UniTask PrepareVoiceAsync(bool silent, params string[] keys)
            => await PrepareAsync(this.voiceMap, AudioType.Voice, silent, keys);

        private async UniTask PrepareAsync(AudioMap map, AudioType type, bool silent, params string[] keys)
        {
            foreach (var key in keys)
            {
                if (string.IsNullOrEmpty(key))
                {
                    if (!silent)
                        UnuLogger.LogWarning($"{type} key is empty");

                    continue;
                }

                if (!AddressablesManager.ContainsKey(key))
                {
                    UnuLogger.LogError($"Cannot find any {type} with key={key}");
                    continue;
                }

                if (map.ContainsKey(key))
                {
                    if (!silent)
                        UnuLogger.LogWarning($"Duplicate {type} key={key}");

                    continue;
                }

                try
                {
                    var handle = Addressables.LoadAssetAsync<AudioClip>(key);
                    await handle.Task;

                    map.Add(key, handle);
                }
                catch (Exception ex)
                {
                    UnuLogger.LogException(ex, this);
                }
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