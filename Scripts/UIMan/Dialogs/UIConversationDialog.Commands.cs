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

            public static void RegisterDefaultInputKeys()
            {
                SetSkipNextKeys(ButtonState.Up, KeyCode.Return, KeyCode.KeypadEnter, KeyCode.Space);
                SetSpeedUpKeys(ButtonState.Press, KeyCode.LeftControl, KeyCode.RightControl);
            }

            public static void RemoveInputKeys()
            {
                CommandInvokerKey.Remove(Key.SpeedUp, Key.SkipNext);
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

            public static class Key
            {
                public const string SpeedUp = "UIConversationDialog-SpeedUp";
                public const string SkipNext = "UIConversationDialog-SkipNext";
            }
        }
    }
}