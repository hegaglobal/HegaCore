﻿using System.Collections.Generic;
using UnityEngine;
using VisualNovelData.Data;

namespace HegaCore
{
    public abstract class EventManager<TPlayerData, TGameSettings, TGameData, THandler, TContainer, TInvoker, TManager> : SingletonBehaviour<TManager>
        where TPlayerData : PlayerData<TPlayerData>, new()
        where TGameSettings : GameSettings<TGameSettings>, new()
        where TGameData : GameData<TPlayerData, TGameSettings>
        where THandler : GameDataHandler<TPlayerData, TGameSettings, TGameData>, new()
        where TContainer : GameDataContainer<TPlayerData, TGameSettings, TGameData, THandler>
        where TInvoker: EventInvoker<TPlayerData, TGameSettings, TGameData, THandler, TContainer>, new()
        where TManager : EventManager<TPlayerData, TGameSettings, TGameData, THandler, TContainer, TInvoker, TManager>
    {
        public TInvoker Invoker { get; private set; }

        private ReadDictionary<int, string> eventMap;

        public virtual void Initialize(in ReadEventData eventData, TContainer dataContainer,
                                       in ReadDictionary<int, string> eventMap)
        {
            this.Invoker = new TInvoker();
            this.Invoker.Initialize(eventData, dataContainer);
            this.eventMap = eventMap;

            EventManager.Instance.Initialize(this.Invoker, eventMap);
        }

        public void Invoke(int eventId, string addon = "")
        {
            if (!this.eventMap.TryGetValue(eventId, out var eventName))
                return;

            this.Invoker.Invoke(string.IsNullOrEmpty(addon) ? eventName : $"{eventName}_{addon}");
        }

        public void Invoke(int stage, int eventId, string addon = "")
        {
            if (!this.eventMap.TryGetValue(eventId, out var eventName))
                return;

            this.Invoker.Invoke(string.IsNullOrEmpty(addon) ? eventName : $"{eventName}_{addon}", stage);
        }
    }
}