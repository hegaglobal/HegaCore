using UnityEngine;
using DG.Tweening;

namespace HegaCore
{
    public sealed class DOTweenRotator : MonoBehaviour
    {
        [SerializeField]
        private bool autoPlay = false;

        [SerializeField]
        private Vector3 euler = default;

        [SerializeField]
        private float duration = 1f;

        [SerializeField]
        private RotateMode mode = RotateMode.FastBeyond360;

        [SerializeField]
        private Ease ease = Ease.Linear;

        [SerializeField]
        private int loop = 0;

        [SerializeField]
        private LoopType loopType = LoopType.Incremental;

        private Tweener tween;

        private void Start()
        {
            if (this.autoPlay)
                Play();
        }

        public void Play()
        {
            this.tween = this.transform.DOLocalRotate(this.euler, this.duration, this.mode)
                                       .SetEase(this.ease).SetLoops(this.loop, this.loopType);
        }

        public void Stop()
            => this.tween?.Kill();
    }
}