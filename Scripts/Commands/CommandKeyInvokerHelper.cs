using System.Collections.Generic;
using UnityEngine;

namespace HegaCore
{
    public static class CommandKeyInvokerHelper
    {
        public static void Register(InputKeyState state, KeyCode inputKey, string commandKey)
        {
            CommandKeyPressInvoker.Instance.Remove(commandKey);
            CommandKeyUpInvoker.Instance.Remove(commandKey);
            CommandKeyDownInvoker.Instance.Remove(commandKey);

            switch (state)
            {
                case InputKeyState.Press:
                    CommandKeyPressInvoker.Instance.Register(inputKey, commandKey);
                    break;

                case InputKeyState.Up:
                    CommandKeyUpInvoker.Instance.Register(inputKey, commandKey);
                    break;

                case InputKeyState.Down:
                    CommandKeyDownInvoker.Instance.Register(inputKey, commandKey);
                    break;
            }
        }

        public static void Register(InputKeyState state, string commandKey, params KeyCode[] inputKeys)
        {
            CommandKeyPressInvoker.Instance.Remove(commandKey);
            CommandKeyUpInvoker.Instance.Remove(commandKey);
            CommandKeyDownInvoker.Instance.Remove(commandKey);

            switch (state)
            {
                case InputKeyState.Press:
                    CommandKeyPressInvoker.Instance.Register(commandKey, inputKeys);
                    break;

                case InputKeyState.Up:
                    CommandKeyUpInvoker.Instance.Register(commandKey, inputKeys);
                    break;

                case InputKeyState.Down:
                    CommandKeyDownInvoker.Instance.Register(commandKey, inputKeys);
                    break;
            }
        }

        public static bool Contains(InputKeyState state, KeyCode inputKey)
        {
            switch (state)
            {
                case InputKeyState.Press:
                    return CommandKeyPressInvoker.Instance.Contains(inputKey);

                case InputKeyState.Up:
                    return CommandKeyUpInvoker.Instance.Contains(inputKey);

                case InputKeyState.Down:
                    return CommandKeyDownInvoker.Instance.Contains(inputKey);

                default:
                    return false;
            }
        }

        public static bool Contains(InputKeyState state, string commandKey)
        {
            switch (state)
            {
                case InputKeyState.Press:
                    return CommandKeyPressInvoker.Instance.Contains(commandKey);

                case InputKeyState.Up:
                    return CommandKeyUpInvoker.Instance.Contains(commandKey);

                case InputKeyState.Down:
                    return CommandKeyDownInvoker.Instance.Contains(commandKey);

                default:
                    return false;
            }
        }

        public static IEnumerable<InputKeyState> Contains(KeyCode inputKey)
        {
            if (CommandKeyPressInvoker.Instance.Contains(inputKey))
                yield return InputKeyState.Press;

            if (CommandKeyUpInvoker.Instance.Contains(inputKey))
                yield return InputKeyState.Up;

            if (CommandKeyDownInvoker.Instance.Contains(inputKey))
                yield return InputKeyState.Down;
        }

        public static IEnumerable<InputKeyState> Contains(string commandKey)
        {
            if (CommandKeyPressInvoker.Instance.Contains(commandKey))
                yield return InputKeyState.Press;

            if (CommandKeyUpInvoker.Instance.Contains(commandKey))
                yield return InputKeyState.Up;

            if (CommandKeyDownInvoker.Instance.Contains(commandKey))
                yield return InputKeyState.Down;
        }

        public static bool TryGetCommandKey(InputKeyState state, KeyCode inputKey, out string commandKey)
        {
            switch (state)
            {
                case InputKeyState.Press:
                    return CommandKeyPressInvoker.Instance.TryGetCommandKey(inputKey, out commandKey);

                case InputKeyState.Up:
                    return CommandKeyUpInvoker.Instance.TryGetCommandKey(inputKey, out commandKey);

                case InputKeyState.Down:
                    return CommandKeyDownInvoker.Instance.TryGetCommandKey(inputKey, out commandKey);

                default:
                    commandKey = default;
                    return false;
            }
        }

        public static void Remove(KeyCode inputKey)
        {
            CommandKeyPressInvoker.Instance.Remove(inputKey);
            CommandKeyUpInvoker.Instance.Remove(inputKey);
            CommandKeyDownInvoker.Instance.Remove(inputKey);
        }

        public static void Remove(params KeyCode[] inputKeys)
        {
            CommandKeyPressInvoker.Instance.Remove(inputKeys);
            CommandKeyUpInvoker.Instance.Remove(inputKeys);
            CommandKeyDownInvoker.Instance.Remove(inputKeys);
        }

        public static void Remove(string commandKey)
        {
            CommandKeyPressInvoker.Instance.Remove(commandKey);
            CommandKeyUpInvoker.Instance.Remove(commandKey);
            CommandKeyDownInvoker.Instance.Remove(commandKey);
        }

        public static void Remove(params string[] commandKeys)
        {
            CommandKeyPressInvoker.Instance.Remove(commandKeys);
            CommandKeyUpInvoker.Instance.Remove(commandKeys);
            CommandKeyDownInvoker.Instance.Remove(commandKeys);
        }

        public static void Remove(InputKeyState state, KeyCode inputKey)
        {
            switch (state)
            {
                case InputKeyState.Press:
                    CommandKeyPressInvoker.Instance.Remove(inputKey);
                    break;

                case InputKeyState.Up:
                    CommandKeyUpInvoker.Instance.Remove(inputKey);
                    break;

                case InputKeyState.Down:
                    CommandKeyDownInvoker.Instance.Remove(inputKey);
                    break;
            }
        }

        public static void Remove(InputKeyState state, params KeyCode[] inputKeys)
        {
            switch (state)
            {
                case InputKeyState.Press:
                    CommandKeyPressInvoker.Instance.Remove(inputKeys);
                    break;

                case InputKeyState.Up:
                    CommandKeyUpInvoker.Instance.Remove(inputKeys);
                    break;

                case InputKeyState.Down:
                    CommandKeyDownInvoker.Instance.Remove(inputKeys);
                    break;
            }
        }

        public static void Remove(InputKeyState state, string commandKey)
        {
            switch (state)
            {
                case InputKeyState.Press:
                    CommandKeyPressInvoker.Instance.Remove(commandKey);
                    break;

                case InputKeyState.Up:
                    CommandKeyUpInvoker.Instance.Remove(commandKey);
                    break;

                case InputKeyState.Down:
                    CommandKeyDownInvoker.Instance.Remove(commandKey);
                    break;
            }
        }

        public static void Remove(InputKeyState state, params string[] commandKeys)
        {
            switch (state)
            {
                case InputKeyState.Press:
                    CommandKeyPressInvoker.Instance.Remove(commandKeys);
                    break;

                case InputKeyState.Up:
                    CommandKeyUpInvoker.Instance.Remove(commandKeys);
                    break;

                case InputKeyState.Down:
                    CommandKeyDownInvoker.Instance.Remove(commandKeys);
                    break;
            }
        }

        public static void OnUpdate(float deltaTime)
        {
            CommandKeyPressInvoker.Instance.OnUpdate(deltaTime);
            CommandKeyUpInvoker.Instance.OnUpdate(deltaTime);
            CommandKeyDownInvoker.Instance.OnUpdate(deltaTime);
        }
    }
}