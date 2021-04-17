using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace HegaCore
{
    [Serializable, InlineProperty]
    public struct MouseButton : IEquatable<MouseButton>
    {
        [HideLabel]
        public int Id;

        public MouseButton(int id)
            => this.Id = id;

        public override bool Equals(object obj)
            => obj is MouseButton other &&
               this.Id == other.Id;

        public bool Equals(MouseButton other)
            => this.Id == other.Id;

        public override int GetHashCode()
            => 2108858624 + this.Id.GetHashCode();

        public static implicit operator MouseButton(int id)
            => new MouseButton(id);

        public static implicit operator int(MouseButton id)
            => id.Id;

        public static MouseButton Left { get; } = new MouseButton(0);

        public static MouseButton Right { get; } = new MouseButton(1);

        public static MouseButton Middle { get; } = new MouseButton(2);
    }

    public abstract class CommandInvokerMouseButton : CommandInvoker<MouseButton>
    {
        protected CommandInvokerMouseButton(IReadOnlyCommandMap commandMap) : base(commandMap) { }
    }

    public sealed class CommandInvokerMouseButtonPress : CommandInvokerMouseButton
    {
        public CommandInvokerMouseButtonPress(IReadOnlyCommandMap commandMap) : base(commandMap) { }

        protected override bool CanInvoke(MouseButton input)
            => Input.GetMouseButton(input);

        public static CommandInvokerMouseButtonPress Default { get; }
            = new CommandInvokerMouseButtonPress(CommandMap.Default);
    }

    public sealed class CommandInvokerMouseButtonDown : CommandInvokerMouseButton
    {
        public CommandInvokerMouseButtonDown(IReadOnlyCommandMap commandMap) : base(commandMap) { }

        protected override bool CanInvoke(MouseButton input)
            => Input.GetMouseButtonDown(input);

        public static CommandInvokerMouseButtonDown Default { get; }
            = new CommandInvokerMouseButtonDown(CommandMap.Default);
    }

    public sealed class CommandInvokerMouseButtonUp : CommandInvokerMouseButton
    {
        public CommandInvokerMouseButtonUp(IReadOnlyCommandMap commandMap) : base(commandMap) { }

        protected override bool CanInvoke(MouseButton input)
            => Input.GetMouseButtonUp(input);

        public static CommandInvokerMouseButtonUp Default { get; }
            = new CommandInvokerMouseButtonUp(CommandMap.Default);
    }
}
