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
        {
            if(this.audioReference != null) 
                AudioManager.Instance.Player.PlayAsync(this.audioReference, this.type, true);
        }

        public void StopAudio()
            => AudioManager.Instance.Player.Stop(this.type);
    }
}