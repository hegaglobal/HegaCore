using System;
using UnityEngine;
using DG.Tweening;
using UnuGames;
using Sirenix.OdinInspector;

namespace HegaCore
{
    public abstract class BaseEnergyModule : UIManModule<EnergyModel>, IEnergyView
    {
        [SerializeField]
        private EnergyColors colors = EnergyColors.White;

        [SerializeField]
        private EnergyTween tween = EnergyTween.Default;

        [SerializeField]
        private EnergyProgressBar progressBar = new EnergyProgressBar();

        public int ActualValue { get; private set; }

        public int Value => this.DataInstance.Value;

        public int MaxValue => this.DataInstance.MaxValue;

        private Tweener tweener = null;
        private float slidingValue;

        public void Initialize(int maxValue, int value)
        {
            this.ActualValue = value;
            this.slidingValue = value;

            this.DataInstance.MaxValue = maxValue;
            this.DataInstance.Value = Mathf.Clamp(value, 0, maxValue);

            SetProgress(value * 1f / maxValue);
        }

        public void ChangeValue(int amount)
        {
            var value = this.ActualValue + amount;

            if (value > this.MaxValue)
            {
                amount = this.MaxValue - this.ActualValue;
                this.ActualValue = this.MaxValue;
            }
            else if (value < 0)
            {
                amount = this.ActualValue;
                this.ActualValue = 0;
            }
            else
            {
                this.ActualValue = value;
            }

            if (amount != 0)
                OnChange();
        }

        public void SetValue(int value)
        {
            this.ActualValue = value;
            OnChange();
        }

        private void OnChange()
        {
            this.tweener?.Kill(false);

            if (!this.tween.CanTween())
            {
                OnSlideCompleted();
                SetProgress(this.ActualValue * 1f / this.MaxValue);
                return;
            }

            var endValue = this.ActualValue;

            if (endValue <= 0)
            {
                endValue = -2;
            }
            else if (endValue >= this.MaxValue)
            {
                endValue = this.MaxValue + 2;
            }

            this.tweener = NewTween();
            this.tweener.ChangeStartValue(this.slidingValue)
                        .ChangeEndValue((float)endValue);

            if (!this.tweener.IsPlaying())
                this.tweener.Play();
        }

        private Tweener NewTween()
            => DOTween.To(GetSlidingValue, SetSlidingValue, this.MaxValue, this.tween.Duration)
                      .OnComplete(OnSlideCompleted)
                      .SetAutoKill(false).SetEase(Ease.Linear).Pause();

        private void OnSlideCompleted()
        {
            this.slidingValue = this.ActualValue;
            SetValue_Internal(this.ActualValue);
        }

        private float GetSlidingValue()
            => this.slidingValue;

        private void SetSlidingValue(float value)
        {
            this.slidingValue = value;
            SetValue_Internal((int)value);
            SetProgress(value / this.MaxValue);
        }

        private void SetProgress(float value)
        {
            if (this.progressBar.Manual && this.progressBar.Bar)
                this.progressBar.Bar.Value = value;
            else
                this.DataInstance.Progress = value;

            SetForegroundColor(this.colors.Get(value));
            OnSetProgress(value);
        }

        protected virtual void OnSetProgress(float value) { }

        protected abstract void SetForegroundColor(in Color color);

        private void SetValue_Internal(int value)
            => this.DataInstance.Value = Mathf.Clamp(value, 0, this.DataInstance.MaxValue);

        [Serializable, InlineProperty]
        private sealed class EnergyProgressBar
        {
            [HorizontalGroup(PaddingLeft = 6), HideLabel]
            public ProgressBar Bar = null;

            [HorizontalGroup, LabelWidth(45)]
            public bool Manual = false;
        }
    }
}