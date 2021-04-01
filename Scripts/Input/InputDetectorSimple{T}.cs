using UnityEngine;

namespace HegaCore
{
    public abstract class InputDetectorSimple<T> : MonoBehaviour, IInput<T>
    {
        protected abstract T None { get; }

        protected T DetectedInput { get; private set; }

        protected InputDetectorSimple()
            => this.DetectedInput = this.None;

        public virtual void SetDetectedInput(T value)
            => this.DetectedInput = value;

        public abstract bool Get(T input);

        public void ResetInput()
            => this.DetectedInput = this.None;
    }
}
