using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace HegaCore
{
    [Serializable, InlineProperty]
    public struct MouseKeyCode : IEquatable<MouseKeyCode>
    {
        [HorizontalGroup(PaddingLeft = 6), HideLabel]
        public MouseButton MouseButton;

        [HorizontalGroup, HideLabel]
        public KeyCode KeyCode;

        public MouseKeyCode(MouseButton mouseButton, KeyCode keyCode)
        {
            this.MouseButton = mouseButton;
            this.KeyCode = keyCode;
        }

        public MouseKeyCode(KeyCode keyCode, MouseButton mouseButton)
        {
            this.KeyCode = keyCode;
            this.MouseButton = mouseButton;
        }

        public bool Equals(MouseKeyCode other)
            => this.MouseButton == other.MouseButton && this.KeyCode == other.KeyCode;

        public override bool Equals(object obj)
            => obj is MouseKeyCode other &&
               this.MouseButton == other.MouseButton && this.KeyCode == other.KeyCode;

        public override int GetHashCode()
        {
            var hashCode = 43270662;
            hashCode = hashCode * -1521134295 + this.MouseButton.GetHashCode();
            hashCode = hashCode * -1521134295 + this.KeyCode.GetHashCode();
            return hashCode;
        }

        public static implicit operator MouseKeyCode(in (MouseButton mouseButton, KeyCode keyCode) combo)
            => new MouseKeyCode(combo.mouseButton, combo.keyCode);

        public static implicit operator MouseKeyCode(in (KeyCode keyCode, MouseButton mouseButton) combo)
            => new MouseKeyCode(combo.mouseButton, combo.keyCode);
    }

    public abstract class CommandInvokerMouseKeyCode : CommandInvoker<MouseKeyCode>
    {
        protected CommandInvokerMouseKeyCode(IReadOnlyCommandMap commandMap) : base(commandMap) { }
    }

    public sealed class CommandInvokerMouseKeyCodePress : CommandInvokerMouseKeyCode
    {
        public CommandInvokerMouseKeyCodePress(IReadOnlyCommandMap commandMap) : base(commandMap) { }

        protected override bool CanInvoke(MouseKeyCode input)
            => Input.GetMouseButton(input.MouseButton) && Input.GetKey(input.KeyCode);

        public static CommandInvokerMouseKeyCodePress Default { get; }
            = new CommandInvokerMouseKeyCodePress(CommandMap.Default);
    }

    public sealed class CommandInvokerMouseKeyCodeDown : CommandInvokerMouseKeyCode
    {
        public CommandInvokerMouseKeyCodeDown(IReadOnlyCommandMap commandMap) : base(commandMap) { }

        protected override bool CanInvoke(MouseKeyCode input)
            => Input.GetMouseButtonDown(input.MouseButton) && Input.GetKeyDown(input.KeyCode);

        public static CommandInvokerMouseKeyCodeDown Default { get; }
            = new CommandInvokerMouseKeyCodeDown(CommandMap.Default);
    }

    public sealed class CommandInvokerMouseKeyCodeUp : CommandInvokerMouseKeyCode
    {
        public CommandInvokerMouseKeyCodeUp(IReadOnlyCommandMap commandMap) : base(commandMap) { }

        protected override bool CanInvoke(MouseKeyCode input)
            => Input.GetMouseButtonUp(input.MouseButton) && Input.GetKeyUp(input.KeyCode);

        public static CommandInvokerMouseKeyCodeUp Default { get; }
            = new CommandInvokerMouseKeyCodeUp(CommandMap.Default);
    }
}
