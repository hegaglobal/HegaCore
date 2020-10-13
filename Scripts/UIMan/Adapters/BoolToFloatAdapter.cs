using UnityEngine;
using UnuGames.MVVM;

namespace HegaCore.MVVM
{
    [CreateAssetMenu(menuName = "UIMan/Adapters/Bool To Float Adapter")]
    public class BoolToFloatAdapter : FloatAdapter
    {
        [SerializeField]
        private float trueValue = default;

        [SerializeField]
        private float falseValue = default;

        public override float Convert(object value, Object context)
        {
            if (value is bool boolVal)
                return boolVal ? this.trueValue : this.falseValue;

            return base.Convert(value, context);
        }

        public override object Convert(float value, Object context)
        {
            if (Mathf.Approximately(value, this.trueValue))
                return true;

            if (Mathf.Approximately(value, this.falseValue))
                return false;

            return base.Convert(value, context);
        }

#if UNITY_EDITOR
        [UnityEditor.MenuItem("UIMan/Adapters/Bool To Float Adapter")]
        private static void CreateBoolAdapterAsset()
            => CreateAdapter<BoolToFloatAdapter>(nameof(BoolToFloatAdapter));
#endif
    }
}