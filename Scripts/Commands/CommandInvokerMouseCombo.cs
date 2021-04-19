namespace HegaCore
{
    public class CommandInvokerMouseCombo : CommandInvokerPressState<MouseButtonCombo, CommandInvokerMouseButtonCombo>
    {
        public CommandInvokerMouseCombo(CommandInvokerMouseButtonCombo press,
                                        CommandInvokerMouseButtonCombo down,
                                        CommandInvokerMouseButtonCombo up)
            : base(press, down, up)
        {
        }

        public static CommandInvokerMouseCombo Default { get; }
            = new CommandInvokerMouseCombo(
                CommandInvokerMouseButtonPressCombo.Default,
                CommandInvokerMouseButtonDownCombo.Default,
                CommandInvokerMouseButtonUpCombo.Default
            );
    }
}