namespace HegaCore
{
    public abstract class CommandInvokerInput<T> : CommandInvoker<T>
    {
        private IInput<T> input;

        public CommandInvokerInput(IReadOnlyCommandMap commandMap) : base(commandMap) { }

        public void SetInput(IInput<T> value)
            => this.input = value;

        protected override bool CanInvoke(T input)
        {
            if (this.input == null)
                return false;

            return this.input.Get(input);
        }

        protected override void Consume(T input)
        {
            if (this.input == null)
                return;

            if (this.input.Consumable)
                this.input?.Consume(input);
        }

        protected override void OnPostUpdate(float deltaTime)
        {
            if (this.input == null)
                return;

            if (this.input.AutoReset)
                this.input?.ResetInput();
        }
    }
}