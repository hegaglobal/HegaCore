using System;
using UnityEngine;

namespace HegaCore
{
    [Serializable]
    public sealed class AnimatorStateEvent : BaseAnimatorStateEvent
    {
        public event StateEvent OnInvoke;
        public event StateEvent OnEnter;
        public event StateEvent OnUpdate;
        public event StateEvent OnExit;

        public AnimatorStateEvent() { }

        public AnimatorStateEvent(float invokeTime) : base(invokeTime) { }

        public override void Enter(in AnimatorStateInfo info)
            => this.OnEnter?.Invoke(info);

        public override void Update(in AnimatorStateInfo info)
            => this.OnUpdate?.Invoke(info);

        public override void Exit(in AnimatorStateInfo info)
            => this.OnExit?.Invoke(info);

        public override void Invoke(in AnimatorStateInfo info)
            => this.OnInvoke?.Invoke(info);

        public void Clear()
        {
            this.OnEnter = null;
            this.OnExit = null;
            this.OnInvoke = null;
            this.OnUpdate = null;
        }

        public delegate void StateEvent(in AnimatorStateInfo info);
    }
}
