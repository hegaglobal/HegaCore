using System;
using UnityEngine;
using Newtonsoft.Json;

namespace HegaCore
{
    public abstract class GameDataContainer<TPlayerData, TGameData, THandler>
        where TPlayerData : PlayerData<TPlayerData>, new()
        where TGameData : GameData<TPlayerData>, new()
        where THandler : GameDataHandler<TPlayerData, TGameData>, new()
    {
        public TGameData Data { get; }

        public THandler Handler { get; }

        public TPlayerData CurrentPlayer
            => this.Data.Players[this.CurrentPlayerIndex];

        public GameSettings Settings
            => this.Data.Settings;

        public bool CurrentPlayerInitialized { get; private set; }

        public bool Daemon { get; set; }

        public bool DarkLord { get; set; }

        public int CurrentPlayerIndex { get; private set; }

        public int LastPlayerIndex { get; }

        public bool ShowConversation { get; set; }

        public bool BattleTutorial { get; set; }

        public GameDataContainer()
        {
            this.Data = new TGameData();
            this.Handler = new THandler();

            this.LastPlayerIndex = this.Data.Players.Length - 1;
            this.CurrentPlayerIndex = 0;
        }

        public virtual void InitializeCurrentPlayer(int playerIndex)
        {
            this.CurrentPlayerIndex = Mathf.Clamp(playerIndex, 0, this.LastPlayerIndex);
            this.CurrentPlayerInitialized = true;
        }

        public virtual void DeinitializeCurrentPlayer()
        {
            this.CurrentPlayerInitialized = false;
            this.CurrentPlayerIndex = 0;
            this.BattleTutorial = false;
        }

        public bool HasAnyPlayer()
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

        public bool IsValidPlayerIndex(int index)
            => index >= 0 && index <= this.LastPlayerIndex;

        public bool TryGetPlayer(int index, out TPlayerData data)
        {
            data = default;

            if (!IsValidPlayerIndex(index))
                return false;

            data = this.Data.Players[index];
            return true;
        }

        public bool WillPlayerDoBattleTutorial()
        {
            if (!Validate())
                return false;

            return this.BattleTutorial || !this.CurrentPlayer.DoneBattleTutorial;
        }

        public void DeletePlayer(int index)
        {
            if (!IsValidPlayerIndex(index))
                return;

            this.Data.Players[index].Reset();
        }

        public void SetPlayerBattleTutorial(bool value = true)
        {
            if (!Validate())
                return;

            this.CurrentPlayer.DoneBattleTutorial = value;
        }

        public void SetPlayerLobbyTutorial(bool value = true)
        {
            if (!Validate())
                return;

            this.CurrentPlayer.DoneLobbyTutorial = value;
        }

        public bool TogglePlayerBattleTutorial()
        {
            if (!Validate())
                return false;

            this.CurrentPlayer.DoneBattleTutorial = !this.CurrentPlayer.DoneBattleTutorial;
            return this.CurrentPlayer.DoneBattleTutorial;
        }

        public bool TogglePlayerLobbyTutorial()
        {
            if (!Validate())
                return false;

            this.CurrentPlayer.DoneLobbyTutorial = !this.CurrentPlayer.DoneLobbyTutorial;
            return this.CurrentPlayer.DoneLobbyTutorial;
        }

        public int ChangePlayerWealth(int amount)
        {
            if (!Validate())
                return 0;

            if (amount != 0)
                this.CurrentPlayer.Wealth += amount;

            return this.CurrentPlayer.Wealth;
        }

        public int ChangePlayerProgressPoint(int amount)
        {
            if (!Validate())
                return 0;

            if (amount != 0)
                this.CurrentPlayer.ProgressPoint += amount;

            return this.CurrentPlayer.ProgressPoint;
        }

        public int ChangePlayerGoodPoint(int amount)
        {
            if (!Validate())
                return 0;

            if (amount != 0)
                this.CurrentPlayer.GoodPoint += amount;

            return this.CurrentPlayer.GoodPoint;
        }

        public int ChangePlayerBadPoint(int amount)
        {
            if (!Validate())
                return 0;

            if (amount != 0)
                this.CurrentPlayer.BadPoint += amount;

            return this.CurrentPlayer.BadPoint;
        }

        public void ChangePlayerLastTime()
        {
            if (!Validate())
                return;

            string lastTime;

            try
            {
                var now = DateTime.Now;
                lastTime = $"{now.Year}/{now.Month}/{now.Day}\n{now.Hour}:{now.Minute}:{now.Second}";
            }
            catch
            {
                lastTime = "2020-01-01\n00:00:00";
            }

            this.CurrentPlayer.Existed = true;
            this.CurrentPlayer.LastTime = lastTime;
        }

        public bool IsPlayerBad()
        {
            if (!Validate())
                return false;

            return this.CurrentPlayer.BadPoint > this.CurrentPlayer.GoodPoint;
        }

        public void Load(bool shouldBackup = false)
            => this.Data.Copy(this.Handler.Load(shouldBackup));

        public void Save()
            => this.Handler.Save(this.Data);

        public void SaveSettings()
        {
            if (this.Daemon)
                UnuLogger.Log(JsonConvert.SerializeObject(this.Settings));

            this.Handler.Save(this.Data);
        }

        public void SavePlayer()
        {
            if (!Validate())
                return;

            if (this.Daemon)
                UnuLogger.Log(JsonConvert.SerializeObject(this.CurrentPlayer));

            ChangePlayerLastTime();
            this.Handler.Save(this.Data);
        }

        protected bool Validate()
        {
            if (!this.CurrentPlayerInitialized)
            {
                UnuLogger.LogError($"Must call {nameof(InitializeCurrentPlayer)}() before using this method.");
                return false;
            }

            return true;
        }
    }
}