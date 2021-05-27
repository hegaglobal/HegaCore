using System;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace HegaCore
{
    public static class RefValue
    {
        public static RefValue<T> Create<T>(T value) where T : struct
            => new RefValue<T>(value);

        public static RefValue<T> Create<T>(in T value) where T : struct
            => new RefValue<T>(in value);
    }

    [Serializable, InlineProperty]
    public class RefValue<T> : IEquatable<T>, IEquatableIn<T>, IEquatable<RefValue<T>>
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

        public RefValue(in T value)
        {
            this.value = value;
        }

        public T Get()
            => this.value;

        public void Set(T value)
            => this.value = value;

        public void Set(in T value)
            => this.value = value;

        public override string ToString()
            => this.value.ToString();

        public override int GetHashCode()
            => this.value.GetHashCode();

        public override bool Equals(object obj)
        {
            if (obj is T otherValue)
                return EqualityComparer<T>.Default.Equals(this.value, otherValue);

            if (obj is RefValue<T> other)
                return EqualityComparer<T>.Default.Equals(this.value, other.value);

            return false;
        }

        public bool Equals(T other)
            => EqualityComparer<T>.Default.Equals(this.value, other);

        public bool Equals(in T other)
            => EqualityComparerIn<T>.Default.Equals(in this.value, in other);

        public bool Equals(RefValue<T> other)
            => !(other is null) && EqualityComparer<T>.Default.Equals(this.value, other);

        public static explicit operator RefValue<T>(T value)
            => new RefValue<T>(value);

        public static implicit operator T(RefValue<T> value)
            => value == null ? default : value.value;

        public static bool operator ==(RefValue<T> lhs, RefValue<T> rhs)
        {
            if (lhs is null && rhs is null)
                return true;

            if (lhs is null || rhs is null)
                return false;

            return EqualityComparer<T>.Default.Equals(lhs.value, rhs.value);
        }

        public static bool operator !=(RefValue<T> lhs, RefValue<T> rhs)
        {
            if (lhs is null && rhs is null)
                return false;

            if (lhs is null || rhs is null)
                return true;

            return !EqualityComparer<T>.Default.Equals(lhs.value, rhs.value);
        }

        public static bool operator ==(RefValue<T> lhs, T rhs)
        {
            if (lhs is null)
                return false;

            return EqualityComparer<T>.Default.Equals(lhs.value, rhs);
        }

        public static bool operator !=(RefValue<T> lhs, T rhs)
        {
            if (lhs is null)
                return true;

            return !EqualityComparer<T>.Default.Equals(lhs.value, rhs);
        }

        public static bool operator ==(T lhs, RefValue<T> rhs)
        {
            if (rhs is null)
                return false;

            return EqualityComparer<T>.Default.Equals(lhs, rhs.value);
        }

        public static bool operator !=(T lhs, RefValue<T> rhs)
        {
            if (rhs is null)
                return true;

            return !EqualityComparer<T>.Default.Equals(lhs, rhs.value);
        }
    }

    public static class RefValueExtensions
    {
        /// <summary>
        /// Gets the value of an instance of <see cref="RefValue{T}"/> without invoking any exception
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <returns>If null returns default(<typeparamref name="T"/>)</returns>
        public static T GetValue<T>(this RefValue<T> self) where T : struct
            => self is null ? default : self;

        /// <summary>
        /// Gets the value of an instance of <see cref="RefValue{T}"/> without invoking any exception
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="orValue">The value to return if <paramref name="self"/> is null</param>
        /// <returns></returns>
        public static T GetValueOr<T>(this RefValue<T> self, T orValue) where T : struct
            => self is null ? orValue : self;

        /// <summary>
        /// Gets the value of an instance of <see cref="RefValue{T}"/> without invoking any exception
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="orValue">The value to return if <paramref name="self"/> is null</param>
        /// <returns></returns>
        public static T GetValueOr<T>(this RefValue<T> self, in T orValue) where T : struct
            => self is null ? orValue : self;

        /// <summary>
        /// Sets the value of an instance of <see cref="RefValue{T}"/> without invoking any exception
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="value"></param>
        public static void SetValue<T>(this RefValue<T> self, T value) where T : struct
        {
            if (self is null)
                return;

            self.Set(value);
        }

        /// <summary>
        /// Sets the value of an instance of <see cref="RefValue{T}"/> without invoking any exception
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="value"></param>
        public static void SetValue<T>(this RefValue<T> self, in T value) where T : struct
        {
            if (self is null)
                return;

            self.Set(in value);
        }
    }
}
