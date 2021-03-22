using System;
using System.Collections.Generic;
using UnityEngine;

namespace HegaCore.UI
{
    public partial class UIConversationDialog
    {
        public static class Commands
        {
            private static CommandMap _commandMap;
            private static CommandInvokerKey _keyInvoker;
            private static CommandInvokerMouseButton _mouseButtonInvoker;
            private static CommandIds _commandIdss;
            private static CommandInputs _inputs;
            private static ButtonStates? _buttonStates;

            public static void Initialize(CommandMap commandMap = null, CommandInvokerKey keyInvoker = null,
                                          CommandInvokerMouseButton mouseButtonInvoker = null,
                                          CommandIds commandIds = null, CommandInputs inputs = null,
                                          in ButtonStates? buttonStates = null)
            {
                _commandMap = commandMap;
                _keyInvoker = keyInvoker;
                _mouseButtonInvoker = mouseButtonInvoker;
                _commandIdss = commandIds;
                _inputs = inputs;
                _buttonStates = buttonStates;

                RegisterInput();
            }

            public static void RegisterInput()
            {
                var buttonStates = GetButtonStates();
                var inputKeys = GetCommandInputs();

                RegisterSpeedUpKey(buttonStates.SpeedUpKey, inputKeys.SpeedUpKeys);
                RegisterSkipNextKey(buttonStates.SkipNextKey, inputKeys.SkipNextKeys);
                RegisterSkipNextMouseButton(buttonStates.SkipNextMouseButton, inputKeys.SkipNextMouseButtons);
            }

            public static void RemoveInput()
            {
                var commandKeys = GetCommandIds();

                GetKeyInvoker().Remove(commandKeys.SpeedUp, commandKeys.SkipNext);
                GetMouseButtonInvoker().Remove(commandKeys.SkipNext);
            }

            public static void RegisterSpeedUpKey(ButtonState state, IEnumerable<KeyCode> inputKeys)
            {
                var commandKeys = GetCommandIds();
                var invoker = GetKeyInvoker();

                invoker.Remove(commandKeys.SpeedUp);
                invoker.Register(state, inputKeys, commandKeys.SpeedUp);
            }

            public static void RegisterSkipNextKey(ButtonState state, IEnumerable<KeyCode> inputKeys)
            {
                var commandKeys = GetCommandIds();
                var invoker = GetKeyInvoker();

                invoker.Remove(commandKeys.SkipNext);
                invoker.Register(state, inputKeys, commandKeys.SkipNext);
            }

            public static void RegisterSkipNextMouseButton(ButtonState state, IEnumerable<int> buttonKeys)
            {
                var commandKeys = GetCommandIds();
                var invoker = GetMouseButtonInvoker();

                invoker.Remove(commandKeys.SkipNext);
                invoker.Register(state, buttonKeys, commandKeys.SkipNext);
            }

            internal static void RegisterSpeedUpCommand(Action execute, Action deactivate)
            {
                GetCommandMap().Register(GetCommandIds().SpeedUp, new ActionCommand(execute, deactivate));
            }

            internal static void RegisterSkipNextCommand(Action execute, Action deactivate)
            {
                GetCommandMap().Register(GetCommandIds().SkipNext, new ActionCommand(execute, deactivate));
            }

            internal static void RemoveCommands()
            {
                var commandKeys = GetCommandIds();
                GetCommandMap().Remove(commandKeys.SpeedUp, commandKeys.SkipNext);
            }

            private static CommandMap GetCommandMap()
                => _commandMap ?? (_commandMap = CommandMap.Default);

            private static CommandInvokerKey GetKeyInvoker()
                => _keyInvoker ?? (_keyInvoker = CommandInvokerKey.Default);

            private static CommandInvokerMouseButton GetMouseButtonInvoker()
                => _mouseButtonInvoker ?? (_mouseButtonInvoker = CommandInvokerMouseButton.Default);

            private static CommandIds GetCommandIds()
                => _commandIdss ?? (_commandIdss = CommandIds.Default);

            private static CommandInputs GetCommandInputs()
                => _inputs ?? (_inputs = CommandInputs.Default);

            private static ButtonStates GetButtonStates()
            {
                if (!_buttonStates.HasValue)
                    _buttonStates = ButtonStates.Default;

                return _buttonStates.Value;
            }

            public class CommandIds
            {
                public readonly string SpeedUp;
                public readonly string SkipNext;

                public CommandIds(string speedUp, string skipNext)
                {
                    this.SpeedUp = speedUp;
                    this.SkipNext = skipNext;
                }

                public static CommandIds Default { get; }
                    = new CommandIds(
                        "UIConversationDialog-SpeedUp",
                        "UIConversationDialog-SkipNext"
                    );
            }

            public readonly struct ButtonStates
            {
                public readonly ButtonState SpeedUpKey;
                public readonly ButtonState SkipNextKey;
                public readonly ButtonState SkipNextMouseButton;

                public ButtonStates(ButtonState speedUpKey, ButtonState skipNextKey,
                                    ButtonState skipNextMouseButton)
                {
                    this.SpeedUpKey = speedUpKey;
                    this.SkipNextKey = skipNextKey;
                    this.SkipNextMouseButton = skipNextMouseButton;
                }

                public static ButtonStates Default { get; }
                    = new ButtonStates(ButtonState.Press, ButtonState.Up, ButtonState.Up);
            }

            public class CommandInputs
            {
                public ReadList<KeyCode> SpeedUpKeys => this.speedUpKeys;

                public ReadList<KeyCode> SkipNextKeys => this.skipNextKeys;

                public ReadList<int> SkipNextMouseButtons => this.skipNextMouseButtons;

                private readonly List<KeyCode> speedUpKeys = new List<KeyCode>();
                private readonly List<KeyCode> skipNextKeys = new List<KeyCode>();
                private readonly List<int> skipNextMouseButtons = new List<int>();

                public CommandInputs(IEnumerable<KeyCode> speedUpKeys,
                              IEnumerable<KeyCode> skipNextKeys,
                              IEnumerable<int> skipNextMouseButtons)
                {
                    this.speedUpKeys.AddRange(speedUpKeys);
                    this.skipNextKeys.AddRange(skipNextKeys);
                    this.skipNextMouseButtons.AddRange(skipNextMouseButtons);
                }

                public static CommandInputs Default { get; }
                    = new CommandInputs(
                        new [] { KeyCode.LeftControl, KeyCode.RightControl },
                        new [] { KeyCode.Return, KeyCode.KeypadEnter, KeyCode.Space },
                        new [] { 0 }
                    );
            }
        }
    }
}