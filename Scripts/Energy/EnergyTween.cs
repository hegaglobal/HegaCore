using System;
using Sirenix.OdinInspector;

namespace HegaCore
{
    [Serializable, InlineProperty]
    public struct EnergyTween
    {
        [HorizontalGroup(PaddingLeft = 6), LabelWidth(45)]
        public bool Enable;

        [HorizontalGroup, ShowIf(nameof(Enable)), LabelWidth(55)]
        public float Duration;

        public EnergyTween(bool use, float duration)
        {
            this.Enable = use;
            this.Duration = duration;
        }

        public bool CanTween()
            => this.Enable && this.Duration > 0f;

        public static EnergyTween Default { get; } = new EnergyTween(true, 0.25f);
    }
}
