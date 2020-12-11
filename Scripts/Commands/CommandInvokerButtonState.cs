using System.Collections.Generic;

namespace HegaCore
{
    public abstract class CommandInvokerButtonState<TInputKey, TInvoker> : IOnUpdate
        where TInvoker : CommandInvoker<TInputKey>
    {
        protected readonly TInvoker Press;
        protected readonly TInvoker Down;
        protected readonly TInvoker Up;

        public CommandInvokerButtonState(TInvoker press, TInvoker down, TInvoker up)
        {
            this.Press = press;
            this.Down = down;
            this.Up = up;
        }

        public void Register(ButtonState state, TInputKey inputKey, string commandKey)
        {
            this.Press.RemoveCommand(commandKey);
            this.Down.RemoveCommand(commandKey);
            this.Up.RemoveCommand(commandKey);

            switch (state)
            {
                case ButtonState.Press:
                    this.Press.Register(inputKey, commandKey);
                    break;

                case ButtonState.Down:
                    this.Down.Register(inputKey, commandKey);
                    break;

                case ButtonState.Up:
                    this.Up.Register(inputKey, commandKey);
                    break;
            }
        }

        public void Register(ButtonState state, IEnumerable<TInputKey> inputKeys, string commandKey)
        {
            this.Press.RemoveCommand(commandKey);
            this.Up.RemoveCommand(commandKey);
            this.Down.RemoveCommand(commandKey);

            switch (state)
            {
                case ButtonState.Press:
                    this.Press.Register(inputKeys, commandKey);
                    break;

                case ButtonState.Down:
                    this.Down.Register(inputKeys, commandKey);
                    break;

                case ButtonState.Up:
                    this.Up.Register(inputKeys, commandKey);
                    break;
            }
        }

        public void Register(ButtonState state, string commandKey, params TInputKey[] inputKeys)
        {
            this.Press.RemoveCommand(commandKey);
            this.Up.RemoveCommand(commandKey);
            this.Down.RemoveCommand(commandKey);

            switch (state)
            {
                case ButtonState.Press:
                    this.Press.Register(commandKey, inputKeys);
                    break;

                case ButtonState.Down:
                    this.Down.Register(commandKey, inputKeys);
                    break;

                case ButtonState.Up:
                    this.Up.Register(commandKey, inputKeys);
                    break;
            }
        }

        public bool ContainsKey(ButtonState state, TInputKey inputKey)
        {
            switch (state)
            {
                case ButtonState.Press:
                    return this.Press.ContainsKey(inputKey);

                case ButtonState.Down:
                    return this.Down.ContainsKey(inputKey);

                case ButtonState.Up:
                    return this.Up.ContainsKey(inputKey);
            }

            return false;
        }

        public bool ContainsCommand(ButtonState state, string commandKey)
        {
            switch (state)
            {
                case ButtonState.Press:
                    return this.Press.ContainsCommand(commandKey);

                case ButtonState.Down:
                    return this.Down.ContainsCommand(commandKey);

                case ButtonState.Up:
                    return this.Up.ContainsCommand(commandKey);
            }

            return false;
        }

        public IEnumerable<ButtonState> ContainsKey(TInputKey inputKey)
        {
            if (this.Press.ContainsKey(inputKey))
                yield return ButtonState.Press;

            if (this.Down.ContainsKey(inputKey))
                yield return ButtonState.Down;

            if (this.Up.ContainsKey(inputKey))
                yield return ButtonState.Up;
        }

        public IEnumerable<ButtonState> ContainsCommand(string commandKey)
        {
            if (this.Press.ContainsCommand(commandKey))
                yield return ButtonState.Press;

            if (this.Down.ContainsCommand(commandKey))
                yield return ButtonState.Down;

            if (this.Up.ContainsCommand(commandKey))
                yield return ButtonState.Up;
        }

        public bool TryGetCommand(ButtonState state, TInputKey inputKey, out string commandKey)
        {
            switch (state)
            {
                case ButtonState.Press:
                    return this.Press.TryGetCommand(inputKey, out commandKey);

                case ButtonState.Down:
                    return this.Down.TryGetCommand(inputKey, out commandKey);

                case ButtonState.Up:
                    return this.Up.TryGetCommand(inputKey, out commandKey);
            }

            commandKey = default;
            return false;
        }

        public void Remove(TInputKey inputKey)
        {
            this.Press.Remove(inputKey);
            this.Down.Remove(inputKey);
            this.Up.Remove(inputKey);
        }

        public void Remove(IEnumerable<TInputKey> inputKeys)
        {
            this.Press.Remove(inputKeys);
            this.Down.Remove(inputKeys);
            this.Up.Remove(inputKeys);
        }

        public void Remove(params TInputKey[] inputKeys)
        {
            this.Press.Remove(inputKeys);
            this.Down.Remove(inputKeys);
            this.Up.Remove(inputKeys);
        }

        public void Remove(string commandKey)
        {
            this.Press.RemoveCommand(commandKey);
            this.Down.RemoveCommand(commandKey);
            this.Up.RemoveCommand(commandKey);
        }

        public void Remove(IEnumerable<string> commandKeys)
        {
            this.Press.RemoveCommand(commandKeys);
            this.Down.RemoveCommand(commandKeys);
            this.Up.RemoveCommand(commandKeys);
        }

        public void Remove(params string[] commandKeys)
        {
            this.Press.RemoveCommand(commandKeys);
            this.Down.RemoveCommand(commandKeys);
            this.Up.RemoveCommand(commandKeys);
        }

        public void Remove(ButtonState state, TInputKey inputKey)
        {
            switch (state)
            {
                case ButtonState.Press:
                    this.Press.Remove(inputKey);
                    break;

                case ButtonState.Down:
                    this.Down.Remove(inputKey);
                    break;

                case ButtonState.Up:
                    this.Up.Remove(inputKey);
                    break;
            }
        }

        public void Remove(ButtonState state, IEnumerable<TInputKey> inputKeys)
        {
            switch (state)
            {
                case ButtonState.Press:
                    this.Press.Remove(inputKeys);
                    break;

                case ButtonState.Down:
                    this.Down.Remove(inputKeys);
                    break;

                case ButtonState.Up:
                    this.Up.Remove(inputKeys);
                    break;
            }
        }

        public void Remove(ButtonState state, params TInputKey[] inputKeys)
        {
            switch (state)
            {
                case ButtonState.Press:
                    this.Press.Remove(inputKeys);
                    break;

                case ButtonState.Down:
                    this.Down.Remove(inputKeys);
                    break;

                case ButtonState.Up:
                    this.Up.Remove(inputKeys);
                    break;
            }
        }

        public void Remove(ButtonState state, string commandKey)
        {
            switch (state)
            {
                case ButtonState.Press:
                    this.Press.RemoveCommand(commandKey);
                    break;

                case ButtonState.Down:
                    this.Down.RemoveCommand(commandKey);
                    break;

                case ButtonState.Up:
                    this.Up.RemoveCommand(commandKey);
                    break;
            }
        }

        public void Remove(ButtonState state, IEnumerable<string> commandKeys)
        {
            switch (state)
            {
                case ButtonState.Press:
                    this.Press.RemoveCommand(commandKeys);
                    break;

                case ButtonState.Down:
                    this.Down.RemoveCommand(commandKeys);
                    break;

                case ButtonState.Up:
                    this.Up.RemoveCommand(commandKeys);
                    break;
            }
        }

        public void Remove(ButtonState state, params string[] commandKeys)
        {
            switch (state)
            {
                case ButtonState.Press:
                    this.Press.RemoveCommand(commandKeys);
                    break;

                case ButtonState.Down:
                    this.Down.RemoveCommand(commandKeys);
                    break;

                case ButtonState.Up:
                    this.Up.RemoveCommand(commandKeys);
                    break;
            }
        }

        public void OnUpdate(float deltaTime)
        {
            this.Press.OnUpdate(deltaTime);
            this.Down.OnUpdate(deltaTime);
            this.Up.OnUpdate(deltaTime);
        }
    }
}