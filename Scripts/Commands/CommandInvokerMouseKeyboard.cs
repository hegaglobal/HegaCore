namespace HegaCore
{
    public class CommandInvokerMouseKeyboard : CommandInvokerPressState<MouseKeyCode, CommandInvokerMouseKeyCode>
    {
        public CommandInvokerMouseKeyboard(CommandInvokerMouseKeyCode press,
                                           CommandInvokerMouseKeyCode down,
                                           CommandInvokerMouseKeyCode up)
            : base(press, down, up)
        {
        }

        public static CommandInvokerMouseKeyboard Default { get; }
            = new CommandInvokerMouseKeyboard(
                CommandInvokerMouseKeyCodePress.Default,
                CommandInvokerMouseKeyCodeDown.Default,
                CommandInvokerMouseKeyCodeUp.Default
            );
    }
}