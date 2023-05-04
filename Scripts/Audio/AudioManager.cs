using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Cysharp.Threading.Tasks;

namespace HegaCore
{
    using AudioMap = Dictionary<string, AsyncOperationHandle<AudioClip>>;
    using KeyMap = Dictionary<string, string>;
    using KeyPairList = List<KeyValuePair<string, string>>;
    using IKeyPairCollection = IEnumerable<KeyValuePair<string, string>>;

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
        private AudioSource voiceBGSource = null;

        [SerializeField]
        private MusicRegisterer musicRegisterer = null;

        [SerializeField]
        private SoundRegisterer soundRegisterer = null;

        [SerializeField]
        private VoiceRegisterer voiceRegisterer = null;

        [SerializeField]
        private VoiceBGRegisterer voiceBGRegisterer = null;

        [Space]
        [SerializeField]
        private float soundBufferInterval = 0.1f;

        public AudioPlayer Player { get; private set; }

        private readonly AudioMap musicMap;
        private readonly AudioMap soundMap;
        private readonly AudioMap voiceMap;
        private readonly AudioMap voiceBGMap;
        private readonly KeyMap musicKeyMap;
        private readonly KeyMap soundKeyMap;
        private readonly KeyMap voiceKeyMap;
        private readonly KeyMap voiceBGKeyMap;
        private readonly RefValue<float> soundBufferIntervalRef;

        public AudioSource VoiceSource => voiceSource;
        
        public AudioManager()
        {
            this.musicMap = new AudioMap();
            this.soundMap = new AudioMap();
            this.voiceMap = new AudioMap();
            this.voiceBGMap = new AudioMap();
            this.musicKeyMap = new KeyMap();
            this.soundKeyMap = new KeyMap();
            this.voiceKeyMap = new KeyMap();
            this.voiceBGKeyMap = new KeyMap();
            this.soundBufferIntervalRef = RefValue.Create(() => this.soundBufferInterval, value => this.soundBufferInterval = value);
        }

        private void Awake()
        {
            this.Player = new AudioPlayer(this, this.audioMixer, this.musicSource, this.soundSource,
                                          this.voiceSource,this.voiceBGSource, this.soundBufferIntervalRef);
        }

        private void Update()
        {
            this.Player?.OnUpdate(GameTime.Provider.UnscaledDeltaTime);
        }

        public async UniTask InitializeAsync(float musicFadeTime, float musicVolume, float soundVolume, float voiceVolume)
        {
            UnuLogger.Log("Initializing music...");
            await this.musicRegisterer.RegisterAsync(this);

            UnuLogger.Log("Initializing sound...");
            await this.soundRegisterer.RegisterAsync(this);

            UnuLogger.Log("Initializing voice...");
            await this.voiceRegisterer.RegisterAsync(this);

            UnuLogger.Log("Initializing voice background...");
            await this.voiceBGRegisterer.RegisterAsync(this);

            this.Player.Initialize(musicFadeTime, musicVolume, soundVolume, voiceVolume);
        }

        public async UniTask PrepareAudioAsync(ReadDictionary<AudioType, KeyPairList> map, AudioType type)
        {
            if (!map.TryGetValue(type, out var keyPairs))
                return;

            await PrepareAudioAsync(type, keyPairs);
        }

        public async UniTask PrepareAudioAsync(ReadDictionary<AudioType, KeyPairList> map, params AudioType[] types)
        {
            if (types == null)
                return;

            foreach (var type in types)
            {
                if (!map.TryGetValue(type, out var keyPairs))
                    return;

                await PrepareAudioAsync(type, keyPairs);
            }
        }

        public async UniTask PrepareAudioAsync(ReadDictionary<AudioType, KeyPairList> map, IEnumerable<AudioType> types)
        {
            if (types == null)
                return;

            foreach (var type in types)
            {
                if (!map.TryGetValue(type, out var keyPairs))
                    return;

                await PrepareAudioAsync(type, keyPairs);
            }
        }

        public async UniTask PrepareAudioAsync(ReadDictionary<AudioType, KeyPairList> map)
        {
            foreach (var kv in map)
            {
                await PrepareAudioAsync(kv.Key, kv.Value);
            }
        }

        public async UniTask PrepareAudioAsync(AudioType type, IKeyPairCollection keyPairs)
        {
            if (keyPairs == null)
                return;

            GetMap(type, out var audioMap, out var keyMap);

            if (audioMap == null || keyMap == null)
                return;

            foreach (var kv in keyPairs)
            {
                var key = kv.Key;
                var assetKey = kv.Value;

                if (keyMap.ContainsKey(key))
                {
                    UnuLogger.LogWarning($"Duplicate {type} key={key}");
                    continue;
                }

                if (string.IsNullOrWhiteSpace(assetKey) || !AddressablesManager.ContainsKey(assetKey))
                {
                    UnuLogger.LogError($"Cannot find any {type} with asset_key={assetKey}");
                    continue;
                }

                if (audioMap.ContainsKey(assetKey))
                {
                    UnuLogger.LogWarning($"Duplicate {type} asset_key={assetKey}");
                    continue;
                }

                try
                {
                    var handle = Addressables.LoadAssetAsync<AudioClip>(assetKey);
                    await handle.Task;

                    audioMap.Add(assetKey, handle);

                    if (string.IsNullOrWhiteSpace(key))
                        continue;

                    keyMap.Add(key, assetKey);
                }
                catch (Exception ex)
                {
                    UnuLogger.LogException(ex, this);
                }
            }
        }

        public async UniTask PrepareAudioAsync(AudioType type, string key, string assetKey)
        {
            if (string.IsNullOrWhiteSpace(key))
                return;

            if (string.IsNullOrWhiteSpace(assetKey) || !AddressablesManager.ContainsKey(assetKey))
            {
                UnuLogger.LogError($"Cannot find any {type} with asset_key={assetKey}");
                return;
            }

            GetMap(type, out var audioMap, out var keyMap);

            if (audioMap == null || keyMap == null)
                return;

            if (keyMap.ContainsKey(key))
            {
                UnuLogger.LogWarning($"Duplicate {type} key={key}");
                return;
            }

            if (audioMap.ContainsKey(assetKey))
            {
                UnuLogger.LogWarning($"Duplicate {type} asset_key={assetKey}");
                return;
            }

            try
            {
                var handle = Addressables.LoadAssetAsync<AudioClip>(assetKey);
                await handle.Task;

                audioMap.Add(assetKey, handle);
                keyMap.Add(key, assetKey);
            }
            catch (Exception ex)
            {
                UnuLogger.LogException(ex, this);
            }
        }

        private void GetMap(AudioType type, out AudioMap audioMap, out KeyMap keyMap)
        {
            switch (type)
            {
                case AudioType.Music: audioMap = this.musicMap; keyMap = this.musicKeyMap; break;
                case AudioType.Sound: audioMap = this.soundMap; keyMap = this.soundKeyMap; break;
                case AudioType.Voice: audioMap = this.voiceMap; keyMap = this.voiceKeyMap; break;
                case AudioType.VoiceBG: audioMap = this.voiceBGMap; keyMap = this.voiceBGKeyMap; break;
                default: audioMap = null; keyMap = null; break;
            }
        }

        public async UniTask PrepareAudioAsync(AudioType type, bool silent, params AssetReferenceAudioClip[] references)
        {
            GetMap(type, out var audioMap, out _);

            await PrepareAsync(audioMap, type, silent, references);
        }

        public async UniTask PrepareMusicAsync(bool silent, params AssetReferenceAudioClip[] references)
            => await PrepareAsync(this.musicMap, AudioType.Music, silent, references);

        public async UniTask PrepareSoundAsync(bool silent, params AssetReferenceAudioClip[] references)
            => await PrepareAsync(this.soundMap, AudioType.Sound, silent, references);

        public async UniTask PrepareVoiceAsync(bool silent, params AssetReferenceAudioClip[] references)
            => await PrepareAsync(this.voiceMap, AudioType.Voice, silent, references);

        public async UniTask PrepareVoiceBGAsync(bool silent, params AssetReferenceAudioClip[] references)
            => await PrepareAsync(this.voiceBGMap, AudioType.VoiceBG, silent, references);

        private async UniTask PrepareAsync(AudioMap map, AudioType type, bool silent, params AssetReferenceAudioClip[] references)
        {
            if (map == null)
                return;

            foreach (var reference in references)
            {
                var key = reference.RuntimeKey.ToString();

                if (string.IsNullOrEmpty(key))
                {
                    if (!silent)
                        UnuLogger.LogWarning($"{type} key is empty");

                    continue;
                }

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

        public async UniTask PrepareAsync(AudioType type, bool silent, params string[] keys)
        {
            GetMap(type, out var audioMap, out _);

            await PrepareAsync(audioMap, type, silent, keys);
        }

        public async UniTask PrepareMusicAsync(bool silent, params string[] keys)
            => await PrepareAsync(this.musicMap, AudioType.Music, silent, keys);

        public async UniTask PrepareSoundAsync(bool silent, params string[] keys)
            => await PrepareAsync(this.soundMap, AudioType.Sound, silent, keys);

        public async UniTask PrepareVoiceAsync(bool silent, params string[] keys)
            => await PrepareAsync(this.voiceMap, AudioType.Voice, silent, keys);

        public async UniTask PrepareVoiceBGAsync(bool silent, params string[] keys)
            => await PrepareAsync(this.voiceBGMap, AudioType.VoiceBG, silent, keys);

        private async UniTask PrepareAsync(AudioMap map, AudioType type, bool silent, params string[] keys)
        {
            if (map == null)
                return;

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

        public void ReleaseAudio(AudioType type, params AssetReferenceAudioClip[] references)
        {
            GetMap(type, out var map, out _);
            Release(map, references);
        }

        public void ReleaseMusic(params AssetReferenceAudioClip[] references)
            => Release(this.musicMap, references);

        public void ReleaseSound(params AssetReferenceAudioClip[] references)
            => Release(this.soundMap, references);

        public void ReleaseVoice(params AssetReferenceAudioClip[] references)
            => Release(this.voiceMap, references);

        public void ReleaseVoiceBG(params AssetReferenceAudioClip[] references)
            => Release(this.voiceBGMap, references);

        private void Release(AudioMap map, params AssetReferenceAudioClip[] references)
        {
            if (map == null)
                return;

            foreach (var reference in references)
            {
                var key = reference.RuntimeKey.ToString();

                if (string.IsNullOrEmpty(key))
                    continue;

                if (!map.ContainsKey(key))
                    continue;

                map.Remove(key);
                reference.ReleaseAsset();
            }
        }

        public void ReleaseAudio(AudioType type, params string[] keys)
        {
            GetMap(type, out var audioMap, out var keyMap);
            Release(keyMap, audioMap, keys);
        }

        public void ReleaseMusic(params string[] keys)
            => Release(this.musicKeyMap, this.musicMap, keys);

        public void ReleaseSound(params string[] keys)
            => Release(this.soundKeyMap, this.soundMap, keys);

        public void ReleaseVoice(params string[] keys)
            => Release(this.voiceKeyMap, this.voiceMap, keys);

        public void ReleaseVoicBG(params string[] keys)
            => Release(this.voiceBGKeyMap, this.voiceBGMap, keys);

        private void Release(KeyMap keyMap, AudioMap audioMap, params string[] keys)
        {
            if (keyMap == null || audioMap == null)
                return;

            foreach (var key in keys)
            {
                if (keyMap.TryGetValue(key, out var assetKey))
                {
                    keyMap.Remove(key);
                }
                else
                {
                    assetKey = key;
                }

                if (!audioMap.TryGetValue(assetKey, out var op))
                    continue;

                audioMap.Remove(assetKey);
                Addressables.Release(op);
            }
        }

        public bool TryGetAudio(AudioType type, AssetReferenceAudioClip reference, out AudioClip clip)
        {
            GetMap(type, out var map, out _);
            return TryGetAudio(reference, map, out clip);
        }

        public bool TryGetMusic(AssetReferenceAudioClip reference, out AudioClip music)
            => TryGetAudio(reference, this.musicMap, out music);

        public bool TryGetSound(AssetReferenceAudioClip reference, out AudioClip sound)
            => TryGetAudio(reference, this.soundMap, out sound);

        public bool TryGetVoice(AssetReferenceAudioClip reference, out AudioClip voice)
            => TryGetAudio(reference, this.voiceMap, out voice);

        public bool TryGetVoiceBG(AssetReferenceAudioClip reference, out AudioClip voice)
            => TryGetAudio(reference, this.voiceBGMap, out voice);

        private bool TryGetAudio(AssetReferenceAudioClip reference, AudioMap map, out AudioClip clip)
        {
            clip = null;

            if (map == null)
                return false;

            var key = reference.RuntimeKey.ToString();

            if (map.TryGetValue(key, out var op))
                clip = op.Result;

            return clip;
        }

        public bool TryGetAudio(AudioType type, string key, out AudioClip clip)
        {
            GetMap(type, out var audioMap, out var keyMap);
            return TryGetAudio(key, keyMap, audioMap, out clip);
        }

        public bool TryGetMusic(string key, out AudioClip music)
            => TryGetAudio(key, this.musicKeyMap, this.musicMap, out music);

        public bool TryGetSound(string key, out AudioClip sound)
            => TryGetAudio(key, this.soundKeyMap, this.soundMap, out sound);

        public bool TryGetVoice(string key, out AudioClip voice)
            => TryGetAudio(key, this.voiceKeyMap, this.voiceMap, out voice);

        public bool TryGetVoiceBG(string key, out AudioClip voice)
            => TryGetAudio(key, this.voiceBGKeyMap, this.voiceBGMap, out voice);

        private bool TryGetAudio(string key, KeyMap keyMap, AudioMap audioMap, out AudioClip clip)
        {
            clip = null;

            if (keyMap == null || audioMap == null)
                return false;

            if (keyMap.TryGetValue(key, out var assetKey))
            {
                if (audioMap.TryGetValue(assetKey, out var op))
                    clip = op.Result;
            }
            else
            {
                if (audioMap.TryGetValue(key, out var op))
                    clip = op.Result;
            }

            return clip;
        }
    }
}