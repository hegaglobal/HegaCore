using UnityEngine;
using Cysharp.Threading.Tasks;

namespace HegaCore
{
    public sealed class SoundRegisterer : MonoBehaviour
    {
        [SerializeField]
        private AssetReferenceAudioClip[] clips = null;

        public async UniTask Register(AudioManager manager)
            => await manager.PrepareSound(this.clips);
    }
}