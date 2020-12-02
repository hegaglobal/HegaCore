using System;
using System.Collections.Generic;
using UnityEngine;
using VisualNovelData.Commands;

namespace HegaCore
{
    public sealed class EventManager : SingletonBehaviour<EventManager>
    {
        public EventInvoker Invoker { get; private set; }

        private ReadDictionary<int, string> eventMap;

        public CommandSystem CommandSystem
            => this.Invoker.CommandSystem;

        public EventCommandSystem EventCommandSystem
            => this.Invoker.EventCommandSystem;

        public GameDataContainer BaseDataContainer
            => this.Invoker.BaseDataContainer;

        public void Initialize(EventInvoker invoker, in ReadDictionary<int, string> eventMap)
        {
            this.Invoker = invoker ?? throw new ArgumentNullException(nameof(invoker));
            this.eventMap = eventMap;
        }

        public void Invoke(int eventId, string addon = "")
        {
            if (!this.eventMap.TryGetValue(eventId, out var eventName))
                return;

            if (string.IsNullOrEmpty(addon))
                this.Invoker.Invoke(eventName);
            else
                this.Invoker.Invoke($"{eventName}_{addon}");
        }

        public void Invoke(int stage, int eventId, string addon = "")
        {
            if (!this.eventMap.TryGetValue(eventId, out var eventName))
                return;

            if (string.IsNullOrEmpty(addon))
                this.Invoker.Invoke(eventName, stage);
            else
                this.Invoker.Invoke($"{eventName}_{addon}", stage);
        }
    }
}