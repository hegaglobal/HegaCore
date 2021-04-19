using System.Collections.Generic;

namespace HegaCore
{
    public abstract class CommandInvokerPressState<TInput, TInvoker> : IOnUpdate
        where TInvoker : CommandInvoker<TInput>
    {
        protected readonly TInvoker Press;
        protected readonly TInvoker Down;
        protected readonly TInvoker Up;

        protected CommandInvokerPressState(TInvoker press, TInvoker down, TInvoker up)
        {
            this.Press = press;
            this.Down = down;
            this.Up = up;
        }

        public void Register(PressState state, TInput input, string commandId)
        {
            this.Press.RemoveCommand(commandId);
            this.Down.RemoveCommand(commandId);
            this.Up.RemoveCommand(commandId);

            switch (state)
            {
                case PressState.Press:
                    this.Press.Register(input, commandId);
                    break;

                case PressState.Down:
                    this.Down.Register(input, commandId);
                    break;

                case PressState.Up:
                    this.Up.Register(input, commandId);
                    break;
            }
        }

        public void Register(PressState state, IEnumerable<TInput> inputs, string commandId)
        {
            this.Press.RemoveCommand(commandId);
            this.Up.RemoveCommand(commandId);
            this.Down.RemoveCommand(commandId);

            switch (state)
            {
                case PressState.Press:
                    this.Press.Register(inputs, commandId);
                    break;

                case PressState.Down:
                    this.Down.Register(inputs, commandId);
                    break;

                case PressState.Up:
                    this.Up.Register(inputs, commandId);
                    break;
            }
        }

        public void Register(PressState state, string commandId, params TInput[] inputs)
        {
            this.Press.RemoveCommand(commandId);
            this.Up.RemoveCommand(commandId);
            this.Down.RemoveCommand(commandId);

            switch (state)
            {
                case PressState.Press:
                    this.Press.Register(commandId, inputs);
                    break;

                case PressState.Down:
                    this.Down.Register(commandId, inputs);
                    break;

                case PressState.Up:
                    this.Up.Register(commandId, inputs);
                    break;
            }
        }

        public bool ContainsInput(PressState state, TInput input)
        {
            switch (state)
            {
                case PressState.Press:
                    return this.Press.ContainsKey(input);

                case PressState.Down:
                    return this.Down.ContainsKey(input);

                case PressState.Up:
                    return this.Up.ContainsKey(input);
            }

            return false;
        }

        public bool ContainsCommand(PressState state, string commandId)
        {
            switch (state)
            {
                case PressState.Press:
                    return this.Press.ContainsCommand(commandId);

                case PressState.Down:
                    return this.Down.ContainsCommand(commandId);

                case PressState.Up:
                    return this.Up.ContainsCommand(commandId);
            }

            return false;
        }

        public IEnumerable<PressState> ContainsInput(TInput input)
        {
            if (this.Press.ContainsKey(input))
                yield return PressState.Press;

            if (this.Down.ContainsKey(input))
                yield return PressState.Down;

            if (this.Up.ContainsKey(input))
                yield return PressState.Up;
        }

        public IEnumerable<PressState> ContainsCommand(string commandId)
        {
            if (this.Press.ContainsCommand(commandId))
                yield return PressState.Press;

            if (this.Down.ContainsCommand(commandId))
                yield return PressState.Down;

            if (this.Up.ContainsCommand(commandId))
                yield return PressState.Up;
        }

        public bool TryGetCommand(PressState state, TInput input, out string commandId)
        {
            switch (state)
            {
                case PressState.Press:
                    return this.Press.TryGetCommand(input, out commandId);

                case PressState.Down:
                    return this.Down.TryGetCommand(input, out commandId);

                case PressState.Up:
                    return this.Up.TryGetCommand(input, out commandId);
            }

            commandId = default;
            return false;
        }

        public void Remove(TInput input)
        {
            this.Press.Remove(input);
            this.Down.Remove(input);
            this.Up.Remove(input);
        }

        public void Remove(IEnumerable<TInput> inputs)
        {
            this.Press.Remove(inputs);
            this.Down.Remove(inputs);
            this.Up.Remove(inputs);
        }

        public void Remove(params TInput[] inputs)
        {
            this.Press.Remove(inputs);
            this.Down.Remove(inputs);
            this.Up.Remove(inputs);
        }

        public void Remove(string commandId)
        {
            this.Press.RemoveCommand(commandId);
            this.Down.RemoveCommand(commandId);
            this.Up.RemoveCommand(commandId);
        }

        public void Remove(IEnumerable<string> commandIds)
        {
            this.Press.RemoveCommand(commandIds);
            this.Down.RemoveCommand(commandIds);
            this.Up.RemoveCommand(commandIds);
        }

        public void Remove(params string[] commandIds)
        {
            this.Press.RemoveCommand(commandIds);
            this.Down.RemoveCommand(commandIds);
            this.Up.RemoveCommand(commandIds);
        }

        public void Remove(PressState state, TInput input)
        {
            switch (state)
            {
                case PressState.Press:
                    this.Press.Remove(input);
                    break;

                case PressState.Down:
                    this.Down.Remove(input);
                    break;

                case PressState.Up:
                    this.Up.Remove(input);
                    break;
            }
        }

        public void Remove(PressState state, IEnumerable<TInput> inputs)
        {
            switch (state)
            {
                case PressState.Press:
                    this.Press.Remove(inputs);
                    break;

                case PressState.Down:
                    this.Down.Remove(inputs);
                    break;

                case PressState.Up:
                    this.Up.Remove(inputs);
                    break;
            }
        }

        public void Remove(PressState state, params TInput[] inputs)
        {
            switch (state)
            {
                case PressState.Press:
                    this.Press.Remove(inputs);
                    break;

                case PressState.Down:
                    this.Down.Remove(inputs);
                    break;

                case PressState.Up:
                    this.Up.Remove(inputs);
                    break;
            }
        }

        public void Remove(PressState state, string commandId)
        {
            switch (state)
            {
                case PressState.Press:
                    this.Press.RemoveCommand(commandId);
                    break;

                case PressState.Down:
                    this.Down.RemoveCommand(commandId);
                    break;

                case PressState.Up:
                    this.Up.RemoveCommand(commandId);
                    break;
            }
        }

        public void Remove(PressState state, IEnumerable<string> commandIds)
        {
            switch (state)
            {
                case PressState.Press:
                    this.Press.RemoveCommand(commandIds);
                    break;

                case PressState.Down:
                    this.Down.RemoveCommand(commandIds);
                    break;

                case PressState.Up:
                    this.Up.RemoveCommand(commandIds);
                    break;
            }
        }

        public void Remove(PressState state, params string[] commandIds)
        {
            switch (state)
            {
                case PressState.Press:
                    this.Press.RemoveCommand(commandIds);
                    break;

                case PressState.Down:
                    this.Down.RemoveCommand(commandIds);
                    break;

                case PressState.Up:
                    this.Up.RemoveCommand(commandIds);
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