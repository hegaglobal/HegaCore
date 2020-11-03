using System.Collections.Generic;
using UnityEngine;

namespace HegaCore
{
    public sealed class CommandKeyDownInvoker : CommandKeyInvoker<CommandKeyDownInvoker>
    {
        protected override void OnUpdate(float deltaTime, ISet<KeyCode> executed, IEnumerable<KeyCode> keys)
        {
            var commandManager = CommandManager.Instance;

            foreach (var key in keys)
            {
                if (TryGetCommandKey(key, out var command))
                {
                    if (Input.GetKeyDown(key))
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