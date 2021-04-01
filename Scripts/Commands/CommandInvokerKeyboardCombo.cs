namespace HegaCore
{
    public class CommandInvokerKeyboardCombo : CommandInvokerPressState<KeyCodeCombo, CommandInvokerKeyCodeCombo>
    {
        public CommandInvokerKeyboardCombo(CommandInvokerKeyCodeCombo press,
                                           CommandInvokerKeyCodeCombo down,
                                           CommandInvokerKeyCodeCombo up)
            : base(press, down, up)
        {
        }

        public static CommandInvokerKeyboardCombo Default { get; }
            = new CommandInvokerKeyboardCombo(
                CommandInvokerKeyPressCombo.Default,
                CommandInvokerKeyDownCombo.Default,
                CommandInvokerKeyUpCombo.Default
            );
    }
}