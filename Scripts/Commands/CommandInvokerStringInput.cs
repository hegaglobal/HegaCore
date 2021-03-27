namespace HegaCore
{
    public class CommandInvokerStringInput : CommandInvoker<string>
    {
        public CommandInvokerStringInput(IReadOnlyCommandMap commandMap) : base(commandMap) { }

        protected override bool CanInvoke(string input)
        {
            return false;
        }
    }
}