using UnityEngine;

namespace HegaCore
{
    public sealed class AudioDefinition : MonoBehaviour
    {
        [SerializeField]
        private AssetReferenceAudioClip audioReference = null;

        [SerializeField]
        private AudioType type = AudioType.Sound;

        public void PlayAudio()
            => AudioManager.Instance.Player.Play(this.audioReference, this.type);

        public void StopAudio()
            => AudioManager.Instance.Player.Stop(this.type);
    }
}