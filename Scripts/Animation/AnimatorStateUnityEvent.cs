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

        public override void Enter(in AnimatorStateInfo info)
            => this.onEnter.Invoke(info);

        public override void Update(in AnimatorStateInfo info)
            => this.onUpdate.Invoke(info);

        public override void Exit(in AnimatorStateInfo info)
            => this.onExit.Invoke(info);

        public override void Invoke(in AnimatorStateInfo info)
            => this.onInvoke.Invoke(info);

        public void Clear()
        {
            this.onEnter.RemoveAllListeners();
            this.onExit.RemoveAllListeners();
            this.onInvoke.RemoveAllListeners();
            this.onUpdate.RemoveAllListeners();
        }

        [Serializable]
        public sealed class StateEvent : UnityEvent<AnimatorStateInfo> { }
    }
}
