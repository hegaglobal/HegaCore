using QuaStateMachine;

namespace HegaCore
{
    public static class StateMachineExtensions
    {
        public static bool Validate(this IStateMachine machine)
        {
            if (machine == null)
            {
                UnuLogger.LogError("State machine is null.");
                return false;
            }

            if (!machine.Initialized)
            {
                UnuLogger.LogError("State machine has not been initialized.");
                return false;
            }

            return true;
        }
    }
}