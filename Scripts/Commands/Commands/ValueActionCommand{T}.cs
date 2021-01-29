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

    public struct ValueActionCommand<TExecute, TDeactivate> : ICommand
        where TExecute : struct, IAction
        where TDeactivate : struct, IAction
    {
        private TExecute? execute;
        private TDeactivate? deactivate;

        public ValueActionCommand(TExecute execute)
        {
            this.execute = execute;
            this.deactivate = null;
        }

        public ValueActionCommand(TExecute execute, TDeactivate deactivate)
        {
            this.execute = execute;
            this.deactivate = deactivate;
        }

        public ValueActionCommand(in TExecute execute)
        {
            this.execute = execute;
            this.deactivate = null;
        }

        public ValueActionCommand(in TExecute execute, TDeactivate deactivate)
        {
            this.execute = execute;
            this.deactivate = deactivate;
        }

        public ValueActionCommand(in TExecute execute, in TDeactivate deactivate)
        {
            this.execute = execute;
            this.deactivate = deactivate;
        }

        public void Deactivate()
            => this.deactivate?.Invoke();

        public void Execute()
            => this.execute?.Invoke();
    }
}