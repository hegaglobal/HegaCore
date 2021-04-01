using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace HegaCore
{
    [Serializable, InlineProperty]
    public struct MouseButtonCombo : IEquatable<MouseButtonCombo>
    {
        [HorizontalGroup(PaddingLeft = 6), HideLabel]
        public MouseButton First;

        [HorizontalGroup, HideLabel]
        public MouseButton Second;

        public MouseButtonCombo(MouseButton first, MouseButton second)
        {
            this.First = first;
            this.Second = second;
        }

        public bool Equals(MouseButtonCombo other)
            => this.First == other.First && this.Second == other.Second;

        public override bool Equals(object obj)
            => obj is MouseButtonCombo other &&
               this.First == other.First && this.Second == other.Second;

        public override int GetHashCode()
        {
            var hashCode = 43270662;
            hashCode = hashCode * -1521134295 + this.First.GetHashCode();
            hashCode = hashCode * -1521134295 + this.Second.GetHashCode();
            return hashCode;
        }

        public static implicit operator MouseButtonCombo(in (MouseButton first, MouseButton second) combo)
            => new MouseButtonCombo(combo.first, combo.second);
    }

    public abstract class CommandInvokerMouseButtonCombo : CommandInvoker<MouseButtonCombo>
    {
        protected CommandInvokerMouseButtonCombo(IReadOnlyCommandMap commandMap) : base(commandMap) { }
    }

    public sealed class CommandInvokerMouseButtonPressCombo : CommandInvokerMouseButtonCombo
    {
        public CommandInvokerMouseButtonPressCombo(IReadOnlyCommandMap commandMap) : base(commandMap) { }

        protected override bool CanInvoke(MouseButtonCombo input)
            => Input.GetMouseButton(input.First) && Input.GetMouseButton(input.Second);

        public static CommandInvokerMouseButtonPressCombo Default { get; }
            = new CommandInvokerMouseButtonPressCombo(CommandMap.Default);
    }

    public sealed class CommandInvokerMouseButtonDownCombo : CommandInvokerMouseButtonCombo
    {
        public CommandInvokerMouseButtonDownCombo(IReadOnlyCommandMap commandMap) : base(commandMap) { }

        protected override bool CanInvoke(MouseButtonCombo input)
            => Input.GetMouseButtonDown(input.First) && Input.GetMouseButtonDown(input.Second);

        public static CommandInvokerMouseButtonDownCombo Default { get; }
            = new CommandInvokerMouseButtonDownCombo(CommandMap.Default);
    }

    public sealed class CommandInvokerMouseButtonUpCombo : CommandInvokerMouseButtonCombo
    {
        public CommandInvokerMouseButtonUpCombo(IReadOnlyCommandMap commandMap) : base(commandMap) { }

        protected override bool CanInvoke(MouseButtonCombo input)
            => Input.GetMouseButtonUp(input.First) && Input.GetMouseButtonUp(input.Second);

        public static CommandInvokerMouseButtonUpCombo Default { get; }
            = new CommandInvokerMouseButtonUpCombo(CommandMap.Default);
    }
}
