using System;
using VisualNovelData.Data;

namespace HegaCore
{
    public abstract class EventInvoker<TPlayerData, TGameData, THandler, TContainer> : EventInvoker
        where TPlayerData : PlayerData<TPlayerData>, new()
        where TGameData : GameData<TPlayerData>
        where THandler : GameDataHandler<TPlayerData, TGameData>, new()
        where TContainer : GameDataContainer<TPlayerData, TGameData, THandler>
    {
        public TContainer DataContainer { get; private set; }

        public virtual void Initialize(in ReadEventData eventData, TContainer dataContainer)
        {
            this.DataContainer = dataContainer ?? throw new ArgumentNullException(nameof(dataContainer));

            base.Initialize(eventData, dataContainer);
        }
    }
}
