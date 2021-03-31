namespace HegaCore
{
    public class CommandInvokerInputSwipe : CommandInvokerInput<SwipeDirection>
    {
        public CommandInvokerInputSwipe(IReadOnlyCommandMap commandMap) : base(commandMap) { }
    }
}
