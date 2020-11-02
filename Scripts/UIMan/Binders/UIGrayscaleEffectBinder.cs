using UnityEngine;
using UnuGames;
using UnuGames.MVVM;

namespace HegaCore.MVVM
{
    using UI;

    [RequireComponent(typeof(UIGrayscaleEffect))]
    [DisallowMultipleComponent]
    public class UIGrayscaleEffectBinder : BinderBase
    {
        protected UIGrayscaleEffect effect;

        [HideInInspector]
        public BindingField valueField = new BindingField("float");

        public bool tweenValueChange;
        public float changeTime = 0.1f;

        public override void Initialize(bool forceInit)
        {
            if (!CheckInitialize(forceInit))
                return;

            this.effect = GetComponent<UIGrayscaleEffect>();
            SubscribeOnChangedEvent(this.valueField, OnUpdateValue);
        }

        private void OnUpdateValue(object val)
        {
            if (val == null)
                return;

            if (this.effect == null)
                return;

            var tempValue = val.ToString();

            float.TryParse(tempValue, out var valChange);
            var time = 0f;

            if (this.tweenValueChange)
            {
                time = this.changeTime;
            }

            UITweener.Value(this.gameObject, time, this.effect.GrayScaleAmount, valChange)
                     .SetOnUpdate(UpdateValue);
        }

        private void UpdateValue(float val)
        {
            this.effect.GrayScaleAmount = val;
        }
    }
}