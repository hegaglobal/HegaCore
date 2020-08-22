using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace HegaCore
{
    [Serializable]
    public struct TextEmitterParams
    {
        public Vector3 OffsetPosition;
        public Color Color;

        [HorizontalGroup]
        public TextSize Size;

        public TextEmitterParams(in Color color)
            : this(Vector3.zero, color, false, 0f)
        { }

        public TextEmitterParams(in Color color, float size)
            : this(Vector3.zero, color, true, size)
        { }

        public TextEmitterParams(in Color color, float? size)
            : this(Vector3.zero, color, size.HasValue, size.Value)
        { }

        public TextEmitterParams(in Vector3 offsetPosition, in Color color, float size)
            : this(offsetPosition, color, true, size)
        { }

        public TextEmitterParams(in Vector3 offsetPosition, in Color color, float? size)
            : this(offsetPosition, color, size.HasValue, size.Value)
        { }

        private TextEmitterParams(in Vector3 offsetPosition, in Color color, bool customSize, float size)
        {
            this.OffsetPosition = offsetPosition;
            this.Color = color;
            this.Size = new TextSize(customSize, size);
        }

        public static TextEmitterParams Default { get; } = new TextEmitterParams(Color.white, 1f);
    }
}