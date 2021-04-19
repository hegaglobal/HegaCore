using UnityEngine;

namespace HegaCore
{
    public class CommandInvokerKeyboard : CommandInvokerPressState<KeyCode, CommandInvokerKeyCode>
    {
        public CommandInvokerKeyboard(CommandInvokerKeyCode press,
                                      CommandInvokerKeyCode down,
                                      CommandInvokerKeyCode up)
            : base(press, down, up)
        {
        }

        public static CommandInvokerKeyboard Default { get; }
            = new CommandInvokerKeyboard(
                CommandInvokerKeyPress.Default,
                CommandInvokerKeyDown.Default,
                CommandInvokerKeyUp.Default
            );
    }
}