using UnityEngine;
using Cysharp.Threading.Tasks;

namespace HegaCore
{
    public sealed class MusicRegisterer : MonoBehaviour
    {
        [SerializeField]
        private AssetReferenceAudioClip[] clips = null;

        public async UniTask Register(AudioManager manager)
            => await manager.PrepareMusic(this.clips);
    }
}