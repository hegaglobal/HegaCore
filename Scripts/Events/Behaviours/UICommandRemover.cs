using UnityEngine;

namespace HegaCore.Events.Commands
{
    public sealed class UICommandRemover : OnDestroyRemover
    {
        protected override void Remove()
        {
            if (SingletonBehaviour.Quitting)
                return;

            var system = EventManager.Instance.CommandSystem;
            var events = GetComponentsInChildren<UICommand>();

            foreach (var e in events)
            {
                system.Remove(e.Key);
            }
        }
    }
}