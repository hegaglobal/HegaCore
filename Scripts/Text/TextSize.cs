using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace HegaCore
{
    [Serializable, InlineProperty]
    public struct TextSize
    {
        [HorizontalGroup(PaddingLeft = 6), LabelWidth(47)]
        public bool Custom;

        [HorizontalGroup, ShowIf(nameof(Custom)), LabelText(" "), LabelWidth(12f), Min(0f), MinValue(0f), PropertySpace(0f, 4f)]
        public float Value;

        public TextSize(bool custom, float value)
        {
            this.Custom = custom;
            this.Value = value;
        }

        public static TextSize Default { get; } = new TextSize(false, 0f);
    }
}