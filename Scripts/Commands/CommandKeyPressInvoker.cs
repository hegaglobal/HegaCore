using System.Collections.Generic;
using UnityEngine;

namespace HegaCore
{
    public sealed class CommandKeyPressInvoker : CommandKeyInvoker<CommandKeyPressInvoker>
    {
        protected override void OnUpdate(float deltaTime, ISet<KeyCode> executed, IEnumerable<KeyCode> keys)
        {
            var commandManager = CommandManager.Instance;

            foreach (var key in keys)
            {
                if (TryGetCommandKey(key, out var command))
                {
                    if (Input.GetKey(key))
                    {
                        executed.Add(key);
                        commandManager.Execute(command);
                    }
                    else if (executed.Contains(key))
                    {
                        executed.Remove(key);
                        commandManager.Deactivate(command);
                    }
                }
            }
        }
    }
}