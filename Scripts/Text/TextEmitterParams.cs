using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace HegaCore
{
    [Serializable]
    public struct TextEmitterParams
    {
        [BoxGroup("Position Offset")]
        public bool RandomInRange;

        [BoxGroup("Position Offset"), LabelText("Offset"), ShowIf(nameof(RandomInRange))]
        public Vector3Range OffsetRange;

        [BoxGroup("Position Offset"), LabelText("Offset"), HideIf(nameof(RandomInRange))]
        public Vector3 Offset;

        public Color Color;

        [HorizontalGroup]
        public TextSize Size;

        public TextEmitterParams(in Color color)
            : this(Vector3Range.Zero, color, false, 0f)
        { }

        public TextEmitterParams(in Color color, float size)
            : this(Vector3Range.Zero, color, true, size)
        { }

        public TextEmitterParams(in Color color, float? size)
            : this(Vector3Range.Zero, color, size.HasValue, size ?? 1f)
        { }

        public TextEmitterParams(in Vector3Range offsetRange, in Color color, float size)
            : this(offsetRange, color, true, size)
        { }

        public TextEmitterParams(in Vector3Range offsetRange, in Color color, float? size)
            : this(offsetRange, color, size.HasValue, size ?? 1f)
        { }

        private TextEmitterParams(in Vector3Range offsetRange, in Color color, bool customSize, float size)
        {
            this.RandomInRange = true;
            this.OffsetRange = offsetRange;
            this.Offset = Vector3.zero;
            this.Color = color;
            this.Size = new TextSize(customSize, size);
        }

        public TextEmitterParams(in Vector3 offset, in Color color, float size)
            : this(offset, color, true, size)
        { }

        public TextEmitterParams(in Vector3 offset, in Color color, float? size)
            : this(offset, color, size.HasValue, size ?? 1f)
        { }

        private TextEmitterParams(in Vector3 offset, in Color color, bool customSize, float size)
        {
            this.RandomInRange = false;
            this.OffsetRange = Vector3Range.Zero;
            this.Offset = offset;
            this.Offset = Vector3.zero;
            this.Color = color;
            this.Size = new TextSize(customSize, size);
        }

        /// <summary>
        /// TextEmitterParams(Vector3Range.Zero, Color.white, true, 1f)
        /// </summary>
        public static TextEmitterParams Default { get; } = new TextEmitterParams(Vector3Range.Zero, Color.white, true, 1f);
    }
}