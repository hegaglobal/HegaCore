using System;
using UnityEngine;
using UnuGames.MVVM;

namespace HegaCore.MVVM
{
    using UnityObject = UnityEngine.Object;

    public abstract class EnumToIntAdapter<T> : IntAdapter where T : unmanaged, Enum
    {
        [SerializeField]
        private bool ignoreCase = true;

        [SerializeField]
        private T defaultEnumValue = default;

        public override int Convert(object value, UnityObject context)
            => Convert(value, this.ignoreCase, this.defaultEnumValue, context);

        public override object Convert(int value, UnityObject context)
            => Convert(value, this.defaultEnumValue);

        public static int Convert(object value, bool ignoreCase, T defaultValue, UnityObject context)
        {
            if (!(value is T val))
            {
                if (!Enum.TryParse(value.ToString(), ignoreCase, out val))
                {
                    UnuLogger.LogError($"Cannot convert '{value}' to {typeof(T)}.", context);
                    val = defaultValue;
                }
            }

            return Enum<T>.ToInt(val);
        }

        public static object Convert(int value, T defaultValue)
        {
            try
            {
                return (T)(object)value;
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}