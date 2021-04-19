using System;
using VisualNovelData.Data;

namespace HegaCore
{
    public abstract class EventInvoker<TPlayerData, TGameSettings, TGameData, THandler, TContainer> : EventInvoker
        where TPlayerData : PlayerData<TPlayerData>, new()
        where TGameSettings : GameSettings<TGameSettings>, new()
        where TGameData : GameData<TPlayerData, TGameSettings>
        where THandler : GameDataHandler<TPlayerData, TGameSettings, TGameData>, new()
        where TContainer : GameDataContainer<TPlayerData, TGameSettings, TGameData, THandler>
    {
        public TContainer DataContainer { get; private set; }

        public virtual void Initialize(in ReadEventData eventData, TContainer dataContainer)
        {
            this.DataContainer = dataContainer ?? throw new ArgumentNullException(nameof(dataContainer));

            base.Initialize(eventData, dataContainer);
        }
    }
}
