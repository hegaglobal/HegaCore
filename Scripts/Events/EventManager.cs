using System;
using UnityEngine;
using VisualNovelData.Commands;

namespace HegaCore
{
    public class EventManager : SingletonBehaviour<EventManager>
    {
        public BaseEventInvoker Invoker { get; private set; }

        public CommandSystem CommandSystem
            => this.Invoker.CommandSystem;

        public EventCommandSystem EventCommandSystem
            => this.Invoker.EventCommandSystem;

        public BaseGameDataContainer BaseDataContainer
            => this.Invoker.BaseDataContainer;

        public void Initialize(BaseEventInvoker invoker)
        {
            this.Invoker = invoker ?? throw new ArgumentNullException(nameof(invoker));
        }
    }
}