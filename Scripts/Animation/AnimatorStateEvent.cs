using System;

namespace HegaCore
{
    [Serializable]
    public sealed class AnimatorStateEvent : AnimatorStateEventBase
    {
        public event StateEvent OnInvoke;
        public event StateEvent OnEnter;
        public event StateEvent OnUpdate;
        public event StateEvent OnExit;

        public AnimatorStateEvent() { }

        public AnimatorStateEvent(float invokeTime) : base(invokeTime) { }

        public override void Enter(float time)
            => this.OnEnter?.Invoke(time);

        public override void Update(float time)
            => this.OnUpdate?.Invoke(time);

        public override void Exit(float time)
            => this.OnExit?.Invoke(time);

        public override void Invoke(float time)
            => this.OnInvoke?.Invoke(time);

        public void Clear()
        {
            this.OnEnter = null;
            this.OnExit = null;
            this.OnInvoke = null;
            this.OnUpdate = null;
        }

        public delegate void StateEvent(float time);
    }
}
