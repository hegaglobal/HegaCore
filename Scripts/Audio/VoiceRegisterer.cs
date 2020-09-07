using UnityEngine;
using Cysharp.Threading.Tasks;

namespace HegaCore
{
    public sealed class VoiceRegisterer : MonoBehaviour
    {
        [SerializeField]
        private AssetReferenceAudioClip[] clips = null;

        public async UniTask RegisterAsync(AudioManager manager)
            => await manager.PrepareVoiceAsync(false, this.clips);
    }
}