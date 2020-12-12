using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace HegaCore
{
    [Serializable, InlineProperty]
    public struct Vector3Range
    {
        [VerticalGroup, LabelText(nameof(Min)), LabelWidth(50)]
        public Vector3 Min;

        [VerticalGroup, LabelText(nameof(Max)), LabelWidth(50)]
        public Vector3 Max;

        public Vector3Range(in Vector3 min, in Vector3 max)
        {
            this.Min = min;
            this.Max = max;
        }

        public void Deconstruct(out Vector3 min, out Vector3 max)
        {
            min = this.Min;
            max = this.Max;
        }

        public Vector3Range With(in Vector3? Min = null, in Vector3? Max = null)
            => new Vector3Range(
                Min ?? this.Min,
                Max ?? this.Max
            );

        public static Vector3Range Zero { get; } = new Vector3Range(Vector3.zero, Vector3.zero);
    }
}