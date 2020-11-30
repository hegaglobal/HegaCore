using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace HegaCore
{
    [Serializable, InlineProperty]
    public struct EnergyColors
    {
        [HorizontalGroup(PaddingLeft = 6), LabelWidth(30)]
        public Color Min;

        [HorizontalGroup, LabelWidth(30)]
        public Color Max;

        public EnergyColors(in Color min, in Color max)
        {
            this.Min = min;
            this.Max = max;
        }

        public Color Get(float value)
            => LEBColor.Lerp(this.Min, this.Max, value);

        public static EnergyColors White { get; } = new EnergyColors(Color.white, Color.white);
    }
}
