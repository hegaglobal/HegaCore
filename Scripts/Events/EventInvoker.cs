using VisualNovelData.Data;
using VisualNovelData.Commands;
using System;

namespace HegaCore
{
    public abstract class BaseEventInvoker
    {
        public CommandSystem CommandSystem { get; }

        public EventCommandSystem EventCommandSystem { get; }

        public BaseGameDataContainer BaseDataContainer { get; }

        public BaseEventInvoker(in ReadEventData eventData, BaseGameDataContainer dataContainer)
        {
            this.CommandSystem = new CommandSystem();
            this.EventCommandSystem = new EventCommandSystem(this.CommandSystem);
            this.EventCommandSystem.Set(eventData);
            this.BaseDataContainer = dataContainer ?? throw new ArgumentNullException(nameof(dataContainer));
        }

        public void Invoke(string @event)
        {
            UnuLogger.Log($"<color=#b71c1c>Invoke Event:</color> {@event}");
            this.EventCommandSystem.Invoke(@event, this.BaseDataContainer.GetPlayerProgressPoint());
        }

        public void Invoke(string @event, int stage)
        {
            UnuLogger.Log($"<color=#b71c1c>Invoke Event:</color> {@event}");
            this.EventCommandSystem.Invoke(@event, stage);
        }
    }

    public abstract class EventInvoker<TPlayerData, TGameData, THandler, TContainer> : BaseEventInvoker
        where TPlayerData : PlayerData<TPlayerData>, new()
        where TGameData : GameData<TPlayerData>
        where THandler : GameDataHandler<TPlayerData, TGameData>, new()
        where TContainer : GameDataContainer<TPlayerData, TGameData, THandler>
    {
        public TContainer DataContainer { get; }

        public EventInvoker(in ReadEventData eventData, TContainer dataContainer) : base(eventData, dataContainer)
        {
            this.DataContainer = dataContainer ?? throw new ArgumentNullException(nameof(dataContainer));
        }
    }
}
