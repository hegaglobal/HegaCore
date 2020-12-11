using UnityEngine;

namespace HegaCore
{
    public abstract class CommandInvokerMouseButtonId : CommandInvoker<int>
    {
        public CommandInvokerMouseButtonId(IReadOnlyCommandMap commandMap) : base(commandMap) { }
    }

    public sealed class CommandInvokerMouseButtonPress : CommandInvokerMouseButtonId
    {
        public CommandInvokerMouseButtonPress(IReadOnlyCommandMap commandMap) : base(commandMap) { }

        protected override bool CanInvoke(int inputKey)
            => Input.GetMouseButton(inputKey);

        public static CommandInvokerMouseButtonPress Default { get; }
            = new CommandInvokerMouseButtonPress(CommandMap.Default);
    }

    public sealed class CommandInvokerMouseButtonDown : CommandInvokerMouseButtonId
    {
        public CommandInvokerMouseButtonDown(IReadOnlyCommandMap commandMap) : base(commandMap) { }

        protected override bool CanInvoke(int inputKey)
            => Input.GetMouseButtonDown(inputKey);

        public static CommandInvokerMouseButtonDown Default { get; }
            = new CommandInvokerMouseButtonDown(CommandMap.Default);
    }

    public sealed class CommandInvokerMouseButtonUp : CommandInvokerMouseButtonId
    {
        public CommandInvokerMouseButtonUp(IReadOnlyCommandMap commandMap) : base(commandMap) { }

        protected override bool CanInvoke(int inputKey)
            => Input.GetMouseButtonUp(inputKey);

        public static CommandInvokerMouseButtonUp Default { get; }
            = new CommandInvokerMouseButtonUp(CommandMap.Default);
    }
}
