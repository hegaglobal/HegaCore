using System;
using UnityEngine;

namespace HegaCore.UI
{
    public partial class UIConversationDialog
    {
        public static class Commands
        {
            private static CommandMap _commandMap;

            public static CommandMap CommandMap
            {
                get
                {
                    if (_commandMap == null)
                        _commandMap = CommandMap.Default;

                    return _commandMap;
                }

                set => _commandMap = value ?? CommandMap.Default;
            }

            private static CommandInvokerKey _commandInvokerKey;

            public static CommandInvokerKey CommandInvokerKey
            {
                get
                {
                    if (_commandInvokerKey == null)
                        _commandInvokerKey = CommandInvokerKey.Default;

                    return _commandInvokerKey;
                }

                set => _commandInvokerKey = value ?? CommandInvokerKey.Default;
            }

            private static CommandInvokerMouseButton _commandInvokerMouse;

            public static CommandInvokerMouseButton CommandInvokerMouseButton
            {
                get
                {
                    if (_commandInvokerMouse == null)
                        _commandInvokerMouse = CommandInvokerMouseButton.Default;

                    return _commandInvokerMouse;
                }

                set => _commandInvokerMouse = value ?? CommandInvokerMouseButton.Default;
            }

            public static void RegisterSpeedUpCommand(Action execute, Action deactivate)
            {
                CommandMap.Register(Key.SpeedUp, new ActionCommand(execute, deactivate));
            }

            public static void RegisterSkipNextCommand(Action execute, Action deactivate)
            {
                CommandMap.Register(Key.SkipNext, new ActionCommand(execute, deactivate));
            }

            public static void RemoveCommands()
            {
                CommandMap.Remove(Key.SpeedUp, Key.SkipNext);
            }

            public static void RegisterDefaultInput()
            {
                SetSpeedUpKeys(ButtonState.Press, KeyCode.LeftControl, KeyCode.RightControl);
                SetSkipNextKeys(ButtonState.Up, KeyCode.Return, KeyCode.KeypadEnter, KeyCode.Space);
                SetSkipNextMouseButtons(ButtonState.Press, 0);
            }

            public static void RemoveInput()
            {
                CommandInvokerKey.Remove(Key.SpeedUp, Key.SkipNext);
                CommandInvokerMouseButton.Remove(Key.SkipNext);
            }

            public static void SetSpeedUpKeys(ButtonState state, params KeyCode[] keys)
            {
                CommandInvokerKey.Remove(Key.SpeedUp);
                CommandInvokerKey.Register(state, Key.SpeedUp, keys);
            }

            public static void SetSkipNextKeys(ButtonState state, params KeyCode[] keys)
            {
                CommandInvokerKey.Remove(Key.SkipNext);
                CommandInvokerKey.Register(state, Key.SkipNext, keys);
            }

            public static void SetSkipNextMouseButtons(ButtonState state, params int[] buttons)
            {
                CommandInvokerMouseButton.Remove(Key.SkipNext);
                CommandInvokerMouseButton.Register(state, Key.SkipNext, buttons);
            }

            public static class Key
            {
                public const string SpeedUp = "UIConversationDialog-SpeedUp";
                public const string SkipNext = "UIConversationDialog-SkipNext";
            }
        }
    }
}