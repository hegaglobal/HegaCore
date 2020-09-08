using UnityEngine;
using Cysharp.Threading.Tasks;

namespace HegaCore
{
    public sealed class SoundRegisterer : MonoBehaviour
    {
        [SerializeField]
        private AssetReferenceAudioClip[] clips = null;

        public async UniTask RegisterAsync(AudioManager manager)
            => await manager.PrepareSoundAsync(false, this.clips);
    }
}