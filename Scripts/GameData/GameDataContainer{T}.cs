using Newtonsoft.Json;

namespace HegaCore
{
    public abstract class GameDataContainer<TPlayerData, TGameSettings, TGameData, THandler> : GameDataContainer
        where TPlayerData : PlayerData<TPlayerData>, new()
        where TGameSettings : GameSettings<TGameSettings>, new()
        where TGameData : GameData<TPlayerData, TGameSettings>
        where THandler : GameDataHandler<TPlayerData, TGameSettings, TGameData>, new()
    {
        public THandler Handler { get; }

        public TGameData Data { get; }

        public TPlayerData CurrentPlayer
            => this.Data.Players[this.CurrentPlayerIndex];

        public TGameSettings CurrentSettings
            => this.Data.Settings;

        public override PlayerData Player
            => this.CurrentPlayer;

        public override GameSettings Settings
            => this.Data.Settings;

        protected GameDataContainer()
        {
            this.Handler = new THandler();
            this.Data = this.Handler.New();

            this.LastPlayerIndex = this.Data.Players.Length - 1;
            this.CurrentPlayerIndex = 0;
        }

        public override bool HasAnyPlayer()
        {
            var result = false;

            foreach (var save in this.Data.Players)
            {
                if (save.Existed)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        public override bool TryGetPlayer(int index, out PlayerData data)
        {
            var result = TryGetPlayer(index, out TPlayerData dataT);
            data = dataT;

            return result;
        }

        public bool TryGetPlayer(int index, out TPlayerData data)
        {
            data = default;

            if (!IsValidPlayerIndex(index))
                return false;

            data = this.Data.Players[index];
            return true;
        }

        public override void DeletePlayer(int index)
        {
            if (!IsValidPlayerIndex(index))
                return;

            this.Data.Players[index].Reset();
        }

        public override void Load(bool shouldBackUp = false)
            => this.Data.CopyFrom(this.Handler.Load(shouldBackUp));

        public override void Save()
            => this.Handler.Save(this.Data);

        public override void SaveSettings()
        {
            if (this.Daemon)
                UnuLogger.Log(JsonConvert.SerializeObject(this.CurrentSettings));

            this.Handler.Save(this.Data);
        }

        public override void SavePlayer()
        {
            if (!Validate())
                return;

            if (this.Daemon)
                UnuLogger.Log(JsonConvert.SerializeObject(this.CurrentPlayer));

            ChangePlayerLastTime();
            this.Handler.Save(this.Data);
        }
    }
}