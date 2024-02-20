using DG.Tweening;
using Live2D.Cubism.Core;
using Live2D.Cubism.Framework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace HegaCore
{
    [RequireComponent(typeof(CubismParameter))]
    public class CubismParameterCustomControl : MonoBehaviour
    {
        public string customParamName;
        private CubismParameter cubismParameter;

        [ShowInInspector, ReadOnly] private float currentValue;
        private Tweener blendTweener;
        private bool isTweening = false;
        private bool finished = false;

        void Start()
        {
            isTweening = false;
            finished = false;
            cubismParameter = GetComponent<CubismParameter>();
            var controller = GetComponentInParent<CubismController>();
            controller?.AddParameterCustomControl(this);
        }

        void LateUpdate()
        {
            if (isTweening)
            {
                cubismParameter.BlendToValue(CubismParameterBlendMode.Override, currentValue);
            }
            else if (finished) // to syns last value
            {
                cubismParameter.BlendToValue(CubismParameterBlendMode.Override, currentValue);
                finished = false;
            }
        }

        public void BlendToValue(float targetValue, float duration, float delay)
        {
            blendTweener?.Kill(false);

            isTweening = true;
            finished = false;
            float converted = targetValue;
            if (targetValue > cubismParameter.MaximumValue)
            {
                converted = cubismParameter.MaximumValue;
            }
            else if (targetValue < cubismParameter.MinimumValue)
            {
                converted = cubismParameter.MinimumValue;
            }

            if (duration > 0)
            {
                blendTweener = DOTween.To(() => currentValue, x => currentValue = x, converted, duration);
                if (delay > 0)
                {
                    blendTweener.SetDelay(delay);
                }

                blendTweener.OnComplete(() => OnBlendCompleted(converted));
            }
            else
            {
                OnBlendCompleted(converted);
            }
        }

        void OnBlendCompleted(float value)
        {
            currentValue = value;
            isTweening = false;
            finished = true;
            blendTweener = null;
        }


        [Button("Blend To Min Value", ButtonSizes.Medium)]
        public void BlendToMinValue(float duration, float delay)
        {
            BlendToValue(cubismParameter.MinimumValue, duration, delay);
        }

        [Button("Blend To Max Value", ButtonSizes.Medium)]
        public void BlendToMaxValue(float duration, float delay)
        {
            BlendToValue(cubismParameter.MaximumValue, duration, delay);
        }
    }
}