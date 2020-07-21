using System.Collections.Generic;
using UnityEngine;

namespace HegaCore
{
    public abstract class PauseManager<TPauseState, TManager> : SingletonBehaviour<TManager>
        where TManager : PauseManager<TPauseState, TManager>
    {
        protected static IPause<TPauseState> None { get; } = new NonePause();

        public abstract TPauseState DefaultState { get; }

        private readonly Dictionary<TPauseState, IPause<TPauseState>> map;
        private TPauseState currentState;
        private IPause<TPauseState> current;

        public PauseManager()
        {
            this.map = new Dictionary<TPauseState, IPause<TPauseState>>();
        }

        public void Initialize()
        {
            this.map.Clear();

            Initialize(this.map);

            this.currentState = this.DefaultState;
            this.current = GetSubsystem(this.currentState);
        }

        private IPause<TPauseState> GetSubsystem(TPauseState type)
            => this.map.ContainsKey(type) ? this.map[type] : None;

        protected abstract void Initialize(Dictionary<TPauseState, IPause<TPauseState>> map);

        public void Pause(TPauseState state)
        {
            if (Equals(this.currentState, state))
                return;

            if (!this.map.ContainsKey(state))
                return;

            var previous = this.current;
            this.current = None;
            previous.Exit(this.currentState);

            var next = this.map[state];
            next.Enter(state, this.currentState);

            this.currentState = state;
            this.current = next;
        }

        public void OnUpdate()
            => this.current.Update();

        private readonly struct NonePause : IPause<TPauseState>
        {
            void IPause<TPauseState>.Enter(TPauseState current, TPauseState previous) { }

            void IPause<TPauseState>.Update() { }

            void IPause<TPauseState>.Exit(TPauseState current) { }
        }
    }
}
