using System;

namespace HegaCore
{
    [Serializable]
    public abstract class GameData<TPlayerData> where TPlayerData : PlayerData<TPlayerData>, new()
    {
        public bool OnceCorrupted;

        public TPlayerData[] Players;

        public GameSettings Settings;

        protected GameData() : this(false) { }

        protected GameData(bool corrupted)
        {
            this.OnceCorrupted = corrupted;
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
                this.Settings.Copy(data?.Settings);
            }
        }
    }
}