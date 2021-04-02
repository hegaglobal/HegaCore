using UnityEngine;
using Sirenix.OdinInspector;

namespace HegaCore
{
    public abstract class InputDetectorSimple<T> : MonoBehaviour, IInput<T>
    {
        [field: SerializeField, LabelText(nameof(AutoReset), true)]
        public bool AutoReset { get; set; }

        protected abstract T None { get; }

        protected T DetectedInput { get; private set; }

        protected InputDetectorSimple()
        {
            this.DetectedInput = this.None;
            this.AutoReset = true;
        }

        public virtual void SetDetectedInput(T value)
            => this.DetectedInput = value;

        public abstract bool Get(T input);

        public virtual void ResetInput()
            => this.DetectedInput = this.None;
    }
}
