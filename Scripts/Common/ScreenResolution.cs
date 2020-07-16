﻿using System;
using UnityEngine;

namespace HegaCore
{
    [Serializable]
    public readonly struct ScreenResolution : IEquatable<ScreenResolution>
    {
        public readonly int Width;
        public readonly int Height;

        public ScreenResolution(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }

        public override bool Equals(object obj)
            => obj is ScreenResolution other &&
               this.Width == other.Width &&
               this.Height == other.Height;

        public bool Equals(ScreenResolution other)
            => this.Width == other.Width && this.Height == other.Height;

        public override int GetHashCode()
        {
            var hashCode = 859600377;

            hashCode = hashCode * -1521134295 + this.Width.GetHashCode();
            hashCode = hashCode * -1521134295 + this.Height.GetHashCode();

            return hashCode;
        }

        public override string ToString()
            => $"{this.Width} × {this.Height}";

        public static implicit operator ScreenResolution(in Resolution value)
            => new ScreenResolution(value.width, value.height);

        public static bool operator ==(in ScreenResolution lhs, ScreenResolution rhs)
            => lhs.Width == rhs.Width && lhs.Height == rhs.Height;

        public static bool operator !=(in ScreenResolution lhs, ScreenResolution rhs)
            => lhs.Width != rhs.Width || lhs.Height != rhs.Height;
    }
}