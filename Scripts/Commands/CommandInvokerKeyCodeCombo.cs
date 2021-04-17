using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace HegaCore
{
    [Serializable, InlineProperty]
    public struct KeyCodeCombo : IEquatable<KeyCodeCombo>
    {
        [HorizontalGroup(PaddingLeft = 6), HideLabel]
        public KeyCode First;

        [HorizontalGroup, HideLabel]
        public KeyCode Second;

        public KeyCodeCombo(KeyCode first, KeyCode second)
        {
            this.First = first;
            this.Second = second;
        }

        public bool Equals(KeyCodeCombo other)
            => this.First == other.First && this.Second == other.Second;

        public override bool Equals(object obj)
            => obj is KeyCodeCombo other &&
               this.First == other.First && this.Second == other.Second;

        public override int GetHashCode()
        {
            var hashCode = 43270662;
            hashCode = hashCode * -1521134295 + this.First.GetHashCode();
            hashCode = hashCode * -1521134295 + this.Second.GetHashCode();
            return hashCode;
        }

        public static implicit operator KeyCodeCombo(in (KeyCode first, KeyCode second) combo)
            => new KeyCodeCombo(combo.first, combo.second);
    }

    public abstract class CommandInvokerKeyCodeCombo : CommandInvoker<KeyCodeCombo>
    {
        protected CommandInvokerKeyCodeCombo(IReadOnlyCommandMap commandMap) : base(commandMap) { }
    }

    public sealed class CommandInvokerKeyPressCombo : CommandInvokerKeyCodeCombo
    {
        public CommandInvokerKeyPressCombo(IReadOnlyCommandMap commandMap) : base(commandMap) { }

        protected override bool CanInvoke(KeyCodeCombo input)
            => Input.GetKey(input.First) && Input.GetKey(input.Second);

        public static CommandInvokerKeyPressCombo Default { get; }
            = new CommandInvokerKeyPressCombo(CommandMap.Default);
    }

    public sealed class CommandInvokerKeyDownCombo : CommandInvokerKeyCodeCombo
    {
        public CommandInvokerKeyDownCombo(IReadOnlyCommandMap commandMap) : base(commandMap) { }

        protected override bool CanInvoke(KeyCodeCombo input)
            => Input.GetKeyDown(input.First) && Input.GetKeyDown(input.Second);

        public static CommandInvokerKeyDownCombo Default { get; }
            = new CommandInvokerKeyDownCombo(CommandMap.Default);
    }

    public sealed class CommandInvokerKeyUpCombo : CommandInvokerKeyCodeCombo
    {
        public CommandInvokerKeyUpCombo(IReadOnlyCommandMap commandMap) : base(commandMap) { }

        protected override bool CanInvoke(KeyCodeCombo input)
            => Input.GetKeyUp(input.First) && Input.GetKeyUp(input.Second);

        public static CommandInvokerKeyUpCombo Default { get; }
            = new CommandInvokerKeyUpCombo(CommandMap.Default);
    }
}
