using UnityEngine;
using Cysharp.Threading.Tasks;

namespace HegaCore
{
    public sealed class MusicRegisterer : MonoBehaviour
    {
        [SerializeField]
        private AssetReferenceAudioClip[] clips = null;

        public async UniTask RegisterAsync(AudioManager manager)
            => await manager.PrepareMusicAsync(this.clips);
    }
}