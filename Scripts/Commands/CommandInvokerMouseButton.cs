namespace HegaCore
{
    public class CommandInvokerMouseButton : CommandInvokerButtonState<int, CommandInvokerMouseButtonId>
    {
        public CommandInvokerMouseButton(CommandInvokerMouseButtonId press,
                                         CommandInvokerMouseButtonId down,
                                         CommandInvokerMouseButtonId up)
            : base(press, down, up)
        {
        }

        public static CommandInvokerMouseButton Default { get; }
            = new CommandInvokerMouseButton(
                CommandInvokerMouseButtonPress.Default,
                CommandInvokerMouseButtonDown.Default,
                CommandInvokerMouseButtonUp.Default
            );
    }
}