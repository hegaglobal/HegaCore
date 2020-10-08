using System;
using UnityEngine;
using UnuGames.MVVM;

namespace HegaCore.MVVM
{
    using UnityObject = UnityEngine.Object;

    public abstract class EnumToBoolAdapter<T> : BoolAdapter where T : struct, Enum
    {
        [SerializeField]
        private T trueValue = default;

        public override bool Convert(object value, UnityObject context)
            => Convert(value, this.trueValue, context);

        public override object Convert(bool value, UnityObject context)
            => Convert(value, this.trueValue);

        public static bool Convert(object value, T trueValue, UnityObject context)
        {
            if (!(value is T val))
            {
                if (!Enum.TryParse(value.ToString(), out val))
                {
                    UnuLogger.LogError($"Cannot convert '{value}' to {typeof(T)}.", context);
                    val = default;
                }
            }

            return trueValue.Equals(val);
        }

        public static object Convert(bool value, T trueValue)
            => value ? trueValue : default;
    }
}