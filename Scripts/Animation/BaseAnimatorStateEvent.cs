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

        public virtual void Enter(float time) { }

        public virtual void Update(float time) { }

        public virtual void Exit(float time) { }

        public virtual void Invoke(float time) { }
    }
}
