using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace HegaCore
{
    [Serializable, InlineProperty]
    public class RefValue<T> : IEquatable<T>, IEquatable<RefValue<T>>
        where T : struct
    {
        [SerializeField, HideLabel]
        private T value;

        public RefValue()
        {
            this.value = default;
        }

        public RefValue(T value)
        {
            this.value = value;
        }

        public override string ToString()
            => this.value.ToString();

        public override int GetHashCode()
            => this.value.GetHashCode();

        public override bool Equals(object obj)
        {
            if (obj is T otherValue)
                return this.value.Equals(otherValue);

            if (obj is RefValue<T> other)
                return this.value.Equals(other.value);

            return false;
        }

        public bool Equals(T other)
            => this.value.Equals(other);

        public bool Equals(RefValue<T> other)
            => other != null && this.value.Equals(other.value);

        public static implicit operator RefValue<T>(T value)
            => new RefValue<T>(value);

        public static implicit operator T(RefValue<T> value)
            => value == null ? default : value.value;
    }

    public static class RefValueExtensions
    {
        public static T GetValue<T>(this RefValue<T> self) where T : struct
            => self ?? default;
    }
}
