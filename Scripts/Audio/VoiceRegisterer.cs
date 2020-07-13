using UnityEngine;
using Cysharp.Threading.Tasks;

namespace HegaCore
{
    public sealed class VoiceRegisterer : MonoBehaviour
    {
        [SerializeField]
        private AssetReferenceAudioClip[] clips = null;

        public async UniTask Register(AudioManager manager)
            => await manager.PrepareVoice(this.clips);
    }
}