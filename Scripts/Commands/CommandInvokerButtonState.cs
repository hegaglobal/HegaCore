using System.Collections.Generic;

namespace HegaCore
{
    public abstract class CommandInvokerButtonState<TInputKey, TInvoker> : IOnUpdate
        where TInvoker : CommandInvoker<TInputKey>
    {
        private readonly TInvoker press;
        private readonly TInvoker down;
        private readonly TInvoker up;

        public CommandInvokerButtonState(TInvoker press, TInvoker down, TInvoker up)
        {
            this.press = press;
            this.down = down;
            this.up = up;
        }

        public void Register(ButtonState state, TInputKey inputKey, string commandKey)
        {
            this.press.RemoveCommand(commandKey);
            this.down.RemoveCommand(commandKey);
            this.up.RemoveCommand(commandKey);

            switch (state)
            {
                case ButtonState.Press:
                    this.press.Register(inputKey, commandKey);
                    break;

                case ButtonState.Down:
                    this.down.Register(inputKey, commandKey);
                    break;

                case ButtonState.Up:
                    this.up.Register(inputKey, commandKey);
                    break;
            }
        }

        public void Register(ButtonState state, string commandKey, params TInputKey[] inputKeys)
        {
            this.press.RemoveCommand(commandKey);
            this.up.RemoveCommand(commandKey);
            this.down.RemoveCommand(commandKey);

            switch (state)
            {
                case ButtonState.Press:
                    this.press.Register(commandKey, inputKeys);
                    break;

                case ButtonState.Down:
                    this.down.Register(commandKey, inputKeys);
                    break;

                case ButtonState.Up:
                    this.up.Register(commandKey, inputKeys);
                    break;
            }
        }

        public bool ContainsKey(ButtonState state, TInputKey inputKey)
        {
            switch (state)
            {
                case ButtonState.Press:
                    return this.press.ContainsKey(inputKey);

                case ButtonState.Down:
                    return this.down.ContainsKey(inputKey);

                case ButtonState.Up:
                    return this.up.ContainsKey(inputKey);
            }

            return false;
        }

        public bool ContainsCommand(ButtonState state, string commandKey)
        {
            switch (state)
            {
                case ButtonState.Press:
                    return this.press.ContainsCommand(commandKey);

                case ButtonState.Down:
                    return this.down.ContainsCommand(commandKey);

                case ButtonState.Up:
                    return this.up.ContainsCommand(commandKey);
            }

            return false;
        }

        public IEnumerable<ButtonState> ContainsKey(TInputKey inputKey)
        {
            if (this.press.ContainsKey(inputKey))
                yield return ButtonState.Press;

            if (this.down.ContainsKey(inputKey))
                yield return ButtonState.Down;

            if (this.up.ContainsKey(inputKey))
                yield return ButtonState.Up;
        }

        public IEnumerable<ButtonState> ContainsCommand(string commandKey)
        {
            if (this.press.ContainsCommand(commandKey))
                yield return ButtonState.Press;

            if (this.down.ContainsCommand(commandKey))
                yield return ButtonState.Down;

            if (this.up.ContainsCommand(commandKey))
                yield return ButtonState.Up;
        }

        public bool TryGetCommand(ButtonState state, TInputKey inputKey, out string commandKey)
        {
            switch (state)
            {
                case ButtonState.Press:
                    return this.press.TryGetCommand(inputKey, out commandKey);

                case ButtonState.Down:
                    return this.down.TryGetCommand(inputKey, out commandKey);

                case ButtonState.Up:
                    return this.up.TryGetCommand(inputKey, out commandKey);
            }

            commandKey = default;
            return false;
        }

        public void Remove(TInputKey inputKey)
        {
            this.press.Remove(inputKey);
            this.down.Remove(inputKey);
            this.up.Remove(inputKey);
        }

        public void Remove(params TInputKey[] inputKeys)
        {
            this.press.Remove(inputKeys);
            this.down.Remove(inputKeys);
            this.up.Remove(inputKeys);
        }

        public void Remove(string commandKey)
        {
            this.press.RemoveCommand(commandKey);
            this.down.RemoveCommand(commandKey);
            this.up.RemoveCommand(commandKey);
        }

        public void Remove(params string[] commandKeys)
        {
            this.press.RemoveCommand(commandKeys);
            this.down.RemoveCommand(commandKeys);
            this.up.RemoveCommand(commandKeys);
        }

        public void Remove(ButtonState state, TInputKey inputKey)
        {
            switch (state)
            {
                case ButtonState.Press:
                    this.press.Remove(inputKey);
                    break;

                case ButtonState.Down:
                    this.down.Remove(inputKey);
                    break;

                case ButtonState.Up:
                    this.up.Remove(inputKey);
                    break;
            }
        }

        public void Remove(ButtonState state, params TInputKey[] inputKeys)
        {
            switch (state)
            {
                case ButtonState.Press:
                    this.press.Remove(inputKeys);
                    break;

                case ButtonState.Down:
                    this.down.Remove(inputKeys);
                    break;

                case ButtonState.Up:
                    this.up.Remove(inputKeys);
                    break;
            }
        }

        public void Remove(ButtonState state, string commandKey)
        {
            switch (state)
            {
                case ButtonState.Press:
                    this.press.RemoveCommand(commandKey);
                    break;

                case ButtonState.Down:
                    this.down.RemoveCommand(commandKey);
                    break;

                case ButtonState.Up:
                    this.up.RemoveCommand(commandKey);
                    break;
            }
        }

        public void Remove(ButtonState state, params string[] commandKeys)
        {
            switch (state)
            {
                case ButtonState.Press:
                    this.press.RemoveCommand(commandKeys);
                    break;

                case ButtonState.Down:
                    this.down.RemoveCommand(commandKeys);
                    break;

                case ButtonState.Up:
                    this.up.RemoveCommand(commandKeys);
                    break;
            }
        }

        public void OnUpdate(float deltaTime)
        {
            this.press.OnUpdate(deltaTime);
            this.down.OnUpdate(deltaTime);
            this.up.OnUpdate(deltaTime);
        }
    }
}