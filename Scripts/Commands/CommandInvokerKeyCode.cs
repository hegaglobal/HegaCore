﻿using UnityEngine;

namespace HegaCore
{
    public abstract class CommandInvokerKeyCode : CommandInvoker<KeyCode>
    {
        protected CommandInvokerKeyCode(IReadOnlyCommandMap commandMap) : base(commandMap) { }
    }

    public sealed class CommandInvokerKeyPress : CommandInvokerKeyCode
    {
        public CommandInvokerKeyPress(IReadOnlyCommandMap commandMap) : base(commandMap) { }

        protected override bool CanInvoke(KeyCode input)
            => Input.GetKey(input);

        public static CommandInvokerKeyPress Default { get; }
            = new CommandInvokerKeyPress(CommandMap.Default);
    }

    public sealed class CommandInvokerKeyDown : CommandInvokerKeyCode
    {
        public CommandInvokerKeyDown(IReadOnlyCommandMap commandMap) : base(commandMap) { }

        protected override bool CanInvoke(KeyCode input)
            => Input.GetKeyDown(input);

        public static CommandInvokerKeyDown Default { get; }
            = new CommandInvokerKeyDown(CommandMap.Default);
    }

    public sealed class CommandInvokerKeyUp : CommandInvokerKeyCode
    {
        public CommandInvokerKeyUp(IReadOnlyCommandMap commandMap) : base(commandMap) { }

        protected override bool CanInvoke(KeyCode input)
            => Input.GetKeyUp(input);

        public static CommandInvokerKeyUp Default { get; }
            = new CommandInvokerKeyUp(CommandMap.Default);
    }
}
