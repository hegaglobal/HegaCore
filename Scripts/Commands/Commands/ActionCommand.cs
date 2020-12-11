using System;

namespace HegaCore
{
    public readonly struct ActionCommand : ICommand
    {
        private readonly Action execute;
        private readonly Action deactivate;

        public ActionCommand(Action execute) : this(execute, null) { }

        public ActionCommand(Action execute, Action deactivate)
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