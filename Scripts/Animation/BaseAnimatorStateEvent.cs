using System;
using UnityEngine;

namespace HegaCore
{
    [Serializable]
    public abstract class BaseAnimatorStateEvent
    {
        [SerializeField, Range(0f, 1f)]
        private float invokeTime = 0f;

        /// <summary>
        /// Normalized time to trigger
        /// </summary>
        public float InvokeTime
        {
            get => this.invokeTime;
            set => this.invokeTime = Mathf.Clamp(value, 0f, 1f);
        }

        public BaseAnimatorStateEvent() { }

        public BaseAnimatorStateEvent(float invokeTime)
        {
            this.InvokeTime = invokeTime;
        }

        public virtual void Enter(in AnimatorStateInfo info) { }

        public virtual void Update(in AnimatorStateInfo info) { }

        public virtual void Exit(in AnimatorStateInfo info) { }

        public virtual void Invoke(in AnimatorStateInfo info) { }
    }
}
