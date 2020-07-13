using System;
using Sirenix.OdinInspector;

namespace HegaCore
{
    public enum Horizontal { Left, Center, Right }

    public enum Vertical { Top, Middle, Bottom }

    [InlineProperty]
    [Serializable]
    public struct Direction
    {
        [LabelWidth(64f)]
        [HorizontalGroup]
        public Horizontal Horizontal;

        [LabelWidth(50f)]
        [HorizontalGroup]
        public Vertical Vertical;
    }
}