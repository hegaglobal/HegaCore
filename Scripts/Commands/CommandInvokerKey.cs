using UnityEngine;

namespace HegaCore
{
    public class CommandInvokerKey : CommandInvokerButtonState<KeyCode, CommandInvokerKeyCode>
    {
        public CommandInvokerKey(CommandInvokerKeyCode press,
                                 CommandInvokerKeyCode down,
                                 CommandInvokerKeyCode up)
            : base(press, down, up)
        {
        }

        public static CommandInvokerKey Default { get; }
            = new CommandInvokerKey(
                CommandInvokerKeyPress.Default,
                CommandInvokerKeyDown.Default,
                CommandInvokerKeyUp.Default
            );
    }
}