using System;
using UnityEngine;
using UnityEngine.Events;

namespace HegaCore
{
    [Serializable]
    public sealed class AnimatorStateUnityEvent : BaseAnimatorStateEvent
    {
        [SerializeField]
        private StateEvent onInvoke = new StateEvent();

        [SerializeField]
        private StateEvent onEnter = new StateEvent();

        [SerializeField]
        private StateEvent onUpdate = new StateEvent();

        [SerializeField]
        private StateEvent onExit = new StateEvent();

        public StateEvent OnInvoke => this.onInvoke;

        public StateEvent OnEnter => this.onEnter;

        public StateEvent OnUpdate => this.onUpdate;

        public StateEvent OnExit => this.onExit;

        public AnimatorStateUnityEvent() { }

        public AnimatorStateUnityEvent(float invokeTime) : base(invokeTime) { }

        public override void Enter(float time)
            => this.onEnter.Invoke(time);

        public override void Update(float time)
            => this.onUpdate.Invoke(time);

        public override void Exit(float time)
            => this.onExit.Invoke(time);

        public override void Invoke(float time)
            => this.onInvoke.Invoke(time);

        public void Clear()
        {
            this.onEnter.RemoveAllListeners();
            this.onExit.RemoveAllListeners();
            this.onInvoke.RemoveAllListeners();
            this.onUpdate.RemoveAllListeners();
        }

        [Serializable]
        public sealed class StateEvent : UnityEvent<float> { }
    }
}
