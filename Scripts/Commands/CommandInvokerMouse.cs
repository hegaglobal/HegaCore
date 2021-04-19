namespace HegaCore
{
    public class CommandInvokerMouse : CommandInvokerPressState<MouseButton, CommandInvokerMouseButton>
    {
        public CommandInvokerMouse(CommandInvokerMouseButton press,
                                   CommandInvokerMouseButton down,
                                   CommandInvokerMouseButton up)
            : base(press, down, up)
        {
        }

        public static CommandInvokerMouse Default { get; }
            = new CommandInvokerMouse(
                CommandInvokerMouseButtonPress.Default,
                CommandInvokerMouseButtonDown.Default,
                CommandInvokerMouseButtonUp.Default
            );
    }
}