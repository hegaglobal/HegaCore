using System;

namespace HegaCore
{
    [Serializable]
    public abstract class GameData<T> where T : PlayerData<T>, new()
    {
        public T[] Players;

        public GameSettings Settings;

        public GameData()
        {
            this.Players = new T[4];

            for (var i = 0; i < this.Players.Length; i++)
            {
                this.Players[i] = new T();
            }

            this.Settings = new GameSettings {
                Language = "en"
            };
        }

        public void Copy(GameData<T> data)
        {
            for (var i = 0; i < this.Players.Length; i++)
            {
                this.Players[i].CopyFrom(data.Players[i]);
            }

            this.Settings.Copy(data?.Settings);
        }
    }
}