using System;
using UnityEngine;

namespace HegaCore.UI
{
    public partial class UIConversationDialog
    {
        public static class Commands
        {
            public static void RegisterSpeedUpCommand(Action execute, Action deactivate)
            {
                CommandManager.Instance.Register(Key.SpeedUp, new ActionCommand(execute, deactivate));
            }

            public static void RegisterSkipNextCommand(Action execute, Action deactivate)
            {
                CommandManager.Instance.Register(Key.SkipNext, new ActionCommand(execute, deactivate));
            }

            public static void RemoveCommands()
            {
                CommandManager.Instance.Remove(Key.SpeedUp, Key.SkipNext);
            }

            public static void RegisterDefaultInputKeys()
            {
                SetSkipNextKeys(InputKeyState.Up, KeyCode.Return, KeyCode.KeypadEnter, KeyCode.Space);
                SetSpeedUpKeys(InputKeyState.Press, KeyCode.LeftControl, KeyCode.RightControl);
            }

            public static void RemoveInputKeys()
            {
                CommandKeyInvokerHelper.Remove(Key.SpeedUp, Key.SkipNext);
            }

            public static void SetSpeedUpKeys(InputKeyState state, params KeyCode[] keys)
            {
                CommandKeyInvokerHelper.Remove(Key.SpeedUp);
                CommandKeyInvokerHelper.Register(state, Key.SpeedUp, keys);
            }

            public static void SetSkipNextKeys(InputKeyState state, params KeyCode[] keys)
            {
                CommandKeyInvokerHelper.Remove(Key.SkipNext);
                CommandKeyInvokerHelper.Register(state, Key.SkipNext, keys);
            }

            public static class Key
            {
                public const string SpeedUp = "SpeedUpConversation";
                public const string SkipNext = "SkipNextConversation";
            }
        }
    }
}