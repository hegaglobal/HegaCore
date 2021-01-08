using UnityEngine;
using UnuGames.MVVM;

namespace HegaCore.MVVM
{
    [CreateAssetMenu(menuName = "UIMan/Adapters/Int To Bool Adapter")]
    public class IntToBoolAdapter : BoolAdapter
    {
        [Space]
        [SerializeField]
        private int comparisonOperand = default;

        [SerializeField]
        private bool operandIsLeftHandSide = false;

        [SerializeField]
        private Comparison comparison = default;

        [Space]
        [SerializeField]
        private int trueValue = default;

        [SerializeField]
        private int falseValue = default;

        public override bool Convert(object value, Object context)
        {
            if (!(value is int intVal))
                return base.Convert(value, context);

            if (this.operandIsLeftHandSide)
            {
                switch (this.comparison)
                {
                    case Comparison.Equal: return this.comparisonOperand == intVal;
                    case Comparison.Lesser: return this.comparisonOperand < intVal;
                    case Comparison.LesserOrEqual: return this.comparisonOperand <= intVal;
                    case Comparison.Greater: return this.comparisonOperand > intVal;
                    case Comparison.GreaterOrEqual: return this.comparisonOperand >= intVal;
                }

                return false;
            }

            switch (this.comparison)
            {
                case Comparison.Equal: return intVal == this.comparisonOperand;
                case Comparison.Lesser: return intVal < this.comparisonOperand;
                case Comparison.LesserOrEqual: return intVal <= this.comparisonOperand;
                case Comparison.Greater: return intVal > this.comparisonOperand;
                case Comparison.GreaterOrEqual: return intVal >= this.comparisonOperand;
            }

            return false;
        }

        public override object Convert(bool value, Object context)
            => value ? this.trueValue : this.falseValue;

#if UNITY_EDITOR
        [UnityEditor.MenuItem("UIMan/Adapters/Int To Bool Adapter")]
        private static void CreateBoolAdapterAsset()
            => CreateAdapter<IntToBoolAdapter>(nameof(IntToBoolAdapter));
#endif

        private enum Comparison
        {
            Equal,
            Lesser,
            LesserOrEqual,
            Greater,
            GreaterOrEqual
        }
    }
}