using UnityEngine;
using Cysharp.Threading.Tasks;

namespace HegaCore
{
    public sealed class VoiceBGRegisterer : MonoBehaviour
    {
        [SerializeField]
        private AssetReferenceAudioClip[] clips = null;

        public async UniTask RegisterAsync(AudioManager manager)
            => await manager.PrepareVoiceBGAsync(false, this.clips);
    }
}