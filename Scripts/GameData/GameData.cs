using System;

namespace HegaCore
{
    [Serializable]
    public abstract class GameData<TPlayerData> where TPlayerData : PlayerData<TPlayerData>, new()
    {
        public TPlayerData[] Players;

        public GameSettings Settings;

        public GameData()
        {
            this.Players = new TPlayerData[4];

            for (var i = 0; i < this.Players.Length; i++)
            {
                this.Players[i] = new TPlayerData();
            }

            this.Settings = new GameSettings {
                Language = "en"
            };
        }

        public void Copy(GameData<TPlayerData> data)
        {
            for (var i = 0; i < this.Players.Length; i++)
            {
                this.Players[i].CopyFrom(data.Players[i]);
            }

            this.Settings.Copy(data?.Settings);
        }
    }
}