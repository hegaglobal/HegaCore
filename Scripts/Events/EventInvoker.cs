using System;
using VisualNovelData.Data;
using VisualNovelData.Commands;

namespace HegaCore
{
    public abstract class EventInvoker
    {
        public CommandSystem CommandSystem { get; }

        public EventCommandSystem EventCommandSystem { get; }

        public GameDataContainer BaseDataContainer { get; private set; }

        public EventInvoker()
        {
            this.CommandSystem = new CommandSystem();
            this.EventCommandSystem = new EventCommandSystem(this.CommandSystem);
        }

        public void Initialize(in ReadEventData eventData, GameDataContainer dataContainer)
        {
            this.EventCommandSystem.Set(eventData);
            this.BaseDataContainer = dataContainer ?? throw new ArgumentNullException(nameof(dataContainer));
        }

        public void Invoke(string @event)
        {
            UnuLogger.Log($"Invoke Event: {@event}");
            this.EventCommandSystem.Invoke(@event, this.BaseDataContainer.GetPlayerProgressPoint());
        }

        public void Invoke(string @event, int stage)
        {
            UnuLogger.Log($"Invoke Event: {@event}");
            this.EventCommandSystem.Invoke(@event, stage);
        }
    }
}
