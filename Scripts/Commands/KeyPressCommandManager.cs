using System.Collections.Generic;
using UnityEngine;

namespace HegaCore
{
    public sealed class KeyPressCommandManager : KeyCodeCommandManager<KeyPressCommandManager>
    {
        protected override void OnUpdate(float deltaTime, IEnumerable<KeyCode> keys)
        {
            var commandManager = CommandManager.Instance;

            foreach (var key in keys)
            {
                if (TryGetCommandKey(key, out var command))
                {

                }
            }
        }
    }
}