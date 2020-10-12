using System;
using DG.Tweening;

namespace HegaCore
{
    [Serializable]
    public struct DOTweenData
    {
        public bool Enabled;
        public float Duration;
        public Ease Ease;

        public DOTweenData(bool enabled, float duration, Ease ease)
        {
            this.Enabled = enabled;
            this.Duration = duration;
            this.Ease = ease;
        }

        public static DOTweenData Enable { get; } = new DOTweenData(true, 1f, Ease.Linear);

        public static DOTweenData Disable { get; } = new DOTweenData(false, 0f, Ease.Linear);
    }
}