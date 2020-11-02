using UnityEngine;
using UnuGames.MVVM;

namespace HegaCore.MVVM
{
    [CreateAssetMenu(menuName = "UIMan/Adapters/IToTemplatedString Adapter")]
    public class IToTemplatedStringAdapter : Adapter<IToTemplatedString>
    {
        [SerializeReference]
        private IToTemplatedString defaultValue = Data.None;

        public override IToTemplatedString Convert(object value, Object context)
            => Convert(value, this.defaultValue, context);

        public override object Convert(IToTemplatedString value, Object context)
            => Convert(value);

        public static IToTemplatedString Convert(object value, IToTemplatedString defaultValue, Object context)
        {
            if (!(value is IToTemplatedString val))
            {
                UnuLogger.LogError($"Cannot convert '{value}' to {nameof(IToTemplatedString)}.", context);
                val = defaultValue;
            }

            return val;
        }

        public static object Convert(IToTemplatedString value)
            => value;

#if UNITY_EDITOR
        [UnityEditor.MenuItem("UIMan/Adapters/IToTemplatedString Adapter")]
        private static void CreateIToTemplatedStringAdapterAsset()
            => CreateAdapter<IToTemplatedStringAdapter>(nameof(IToTemplatedStringAdapter));
#endif
    }
}