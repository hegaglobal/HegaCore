using System;

namespace HegaCore
{
    public static class RefValue
    {
        public static RefValue<T> Create<T>(Func<T> getter, Action<T> setter)
            => new RefValue<T>(getter, setter);

        public static RefValue<T> Create<T>(Func<T> getter, ActionIn<T> setter)
            => new RefValue<T>(getter, setter);
    }

    public class RefValue<T>
    {
        private Action<T> setter;
        private ActionIn<T> setterIn;
        private Func<T> getter;

        public RefValue(Func<T> getter, Action<T> setter)
        {
            this.getter = getter ?? throw new ArgumentNullException(nameof(getter));
            this.setter = setter ?? throw new ArgumentNullException(nameof(setter));
        }

        public RefValue(Func<T> getter, ActionIn<T> setterIn)
        {
            this.getter = getter ?? throw new ArgumentNullException(nameof(getter));
            this.setterIn = setterIn ?? throw new ArgumentNullException(nameof(setterIn));
        }

        public T Get()
            => this.getter();

        public void Set(T value)
        {
            if (this.setter != null)
                this.setter(value);
            else if (this.setterIn != null)
                this.setterIn(in value);
        }

        public void Set(in T value)
        {
            if (this.setterIn != null)
                this.setterIn(in value);
            else if (this.setter != null)
                this.setter(value);
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
        public static T GetValue<T>(this RefValue<T> self)
            => self is null ? default : self.Get();

        /// <summary>
        /// Gets the value of an instance of <see cref="RefValue{T}"/> without invoking any exception
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="orValue">The value to return if <paramref name="self"/> is null</param>
        /// <returns></returns>
        public static T GetValueOr<T>(this RefValue<T> self, T orValue)
            => self is null ? orValue : self.Get();

        /// <summary>
        /// Gets the value of an instance of <see cref="RefValue{T}"/> without invoking any exception
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="orValue">The value to return if <paramref name="self"/> is null</param>
        /// <returns></returns>
        public static T GetValueOr<T>(this RefValue<T> self, in T orValue)
            => self is null ? orValue : self.Get();

        /// <summary>
        /// Sets the value of an instance of <see cref="RefValue{T}"/> without invoking any exception
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="value"></param>
        public static void SetValue<T>(this RefValue<T> self, T value)
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
        public static void SetValue<T>(this RefValue<T> self, in T value)
        {
            if (self is null)
                return;

            self.Set(in value);
        }
    }
}
