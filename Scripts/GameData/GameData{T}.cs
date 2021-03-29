using System;

namespace HegaCore
{
    [Serializable]
    public abstract class GameData<TPlayerData, TGameSettings>
        where TPlayerData : PlayerData<TPlayerData>, new()
        where TGameSettings : GameSettings<TGameSettings>, new()
    {
        public bool OnceCorrupted;

        public TPlayerData[] Players;

        public TGameSettings Settings;

        protected GameData() : this(false) { }

        protected GameData(bool corrupted)
        {
            this.OnceCorrupted = corrupted;
            this.Players = new TPlayerData[4];

            for (var i = 0; i < this.Players.Length; i++)
            {
                this.Players[i] = new TPlayerData();
            }

            this.Settings = new TGameSettings();
        }

        public void CopyFrom(GameData<TPlayerData, TGameSettings> data)
        {
            if (data == null)
                return;

            this.OnceCorrupted = data.OnceCorrupted;

            if (this.Players != null && data.Players != null)
            {
                for (var i = 0; i < this.Players.Length; i++)
                {
                    if (i >= data.Players.Length)
                        continue;

                    this.Players[i].CopyFrom(data.Players[i]);
                }
            }

            if (this.Settings != null)
            {
                this.Settings.CopyFrom(data?.Settings);
            }
        }
    }
}