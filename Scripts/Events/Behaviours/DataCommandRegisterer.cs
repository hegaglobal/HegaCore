namespace HegaCore.Events.Commands
{
    public class DataCommandRegisterer : CommandRegisterer
    {
        protected sealed override void Register(bool @override)
        {
            if (UnityEngine.SingletonBehaviour.Quitting)
                return;

            Register(CoreDataCommands.Commands, @override);
            Register(GetCommands(), @override);
        }
    }
}