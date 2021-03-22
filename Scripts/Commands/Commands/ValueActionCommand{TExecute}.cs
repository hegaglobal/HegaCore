using System.Delegates;

namespace HegaCore
{
    public struct ValueActionCommand<TExecute> : ICommand
        where TExecute : struct, IAction
    {
        private TExecute? execute;

        public ValueActionCommand(TExecute execute)
        {
            this.execute = execute;
        }

        public void Deactivate() { }

        public void Execute()
            => this.execute?.Invoke();
    }
}