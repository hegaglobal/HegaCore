using System;

namespace HegaCore
{
    [Serializable]
    public sealed class SaveData
    {
        public PlayerSave[] PlayerSaves;

        public GameSettings Settings;

        public SaveData()
        {
            this.PlayerSaves = new PlayerSave[4];

            for (var i = 0; i < this.PlayerSaves.Length; i++)
            {
                this.PlayerSaves[i] = new PlayerSave();
            }

            this.Settings = new GameSettings {
                Language = "en"
            };
        }

        public void Copy(SaveData data)
        {
            for (var i = 0; i < this.PlayerSaves.Length; i++)
            {
                this.PlayerSaves[i].Copy(data.PlayerSaves[i]);
            }

            this.Settings.Copy(data?.Settings);
        }
    }
}