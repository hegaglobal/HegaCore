using UnityEngine;
using QuaStateMachine;

namespace HegaCore
{
    using State = State<string, StateDirection<string>, string>;

    public abstract partial class BaseStateManager<T> : SingletonBehaviour<T> where T : MonoBehaviour
    {
        private StateMachine machine;

        protected void Start()
        {
            this.machine = GetStateMachine();
            this.machine.Build();
        }

        protected void Update()
        {
            this.machine.Tick();
        }

        protected void OnDestroy()
        {
            this.machine.Terminate();
        }

        public void TransitionTo(IState state, bool allowSameState = true)
            => TransitionTo(state.Name.ToString(), allowSameState);

        public void TransitionTo(string stateName, bool allowSameState = true)
        {
            if (string.IsNullOrEmpty(stateName))
                return;

            if (!this.machine.Validate())
                return;

            var nextState = GetState(stateName);

            if (nextState == null)
                return;

            var currentState = this.machine.CurrentState;

            if (!allowSameState && currentState.Equals(nextState))
                return;

            var direction = currentState.To(nextState);
            var signalName = direction.ToString();

            if (!this.machine.SignalMap.ContainsKey(signalName))
            {
                UnuLogger.LogError($"Cannot find any signal by direction={signalName}", this);
                return;
            }

            var signal = this.machine.GetSignalByName(signalName);
            signal.Emit();
        }

        public void ResetToDefault()
        {
            this.machine.Terminate();
            this.machine.Initialize();
        }

        private State GetState(string stateName)
        {
            if (string.IsNullOrEmpty(stateName))
            {
                UnuLogger.LogError("State name cannot be null or empty.", this);
                return default;
            }

            if (!this.machine.StateMap.ContainsKey(stateName))
            {
                UnuLogger.LogError($"Cannot find any state with name={stateName}.", this);
                return default;
            }

            return this.machine.GetStateByName(stateName);
        }

        protected abstract StateMachine GetStateMachine();
    }
}