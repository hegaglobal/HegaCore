namespace HegaCore
{
    public class CommandInvokerInputString : CommandInvokerInput<string>
    {
        public CommandInvokerInputString(IReadOnlyCommandMap commandMap) : base(commandMap) { }
    }
}