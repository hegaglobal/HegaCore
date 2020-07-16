using System;
//using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace HegaCore
{
    public abstract class GameDataManager<TGameData, TPlayerData, TGameDataHandler>
        where TGameData : GameData<TPlayerData>, new()
        where TPlayerData : PlayerData<TPlayerData>, new()
        where TGameDataHandler : GameDataHandler<TGameData, TPlayerData>
    {
        public bool Daemon { get; set; }

        public TPlayerData CurrentPlayer
            => this.data.Players[this.CurrentPlayerIndex];

        public GameSettings Settings
            => this.data.Settings;

        public bool Initialized { get; private set; }

        public int CurrentPlayerIndex { get; private set; }

        public int LastPlayerIndex { get; }

        //public BattleId BattleId { get; set; }

        //public ChapterId ChapterId { get; set; }

        public bool ShowConversation { get; set; }

        public bool BattleTutorial { get; set; }

        //public ListSegment<int> UnlockedMissions
        //    => this.unlockedMissions;

        //public ListSegment<ChapterId> UnlockedChapters
        //    => this.unlockedChapters;

        //public ListSegment<ChapterId> UnlockedPoses
        //    => this.unlockedPoses;

        //public ListSegment<ChapterId> UnlockedWorks
        //    => this.unlockedWorks;

        private readonly TGameData data;
        private readonly TGameDataHandler handler;

        //private readonly List<int> unlockedMissions;
        //private readonly List<ChapterId> unlockedChapters;
        //private readonly List<ChapterId> unlockedPoses;
        //private readonly List<ChapterId> unlockedWorks;

        public GameDataManager(TGameData data, TGameDataHandler handler)
        {
            this.data = data ?? throw new ArgumentNullException(nameof(data));
            this.handler = handler ?? throw new ArgumentNullException(nameof(handler));

            this.LastPlayerIndex = this.data.Players.Length - 1;
            this.CurrentPlayerIndex = 0;

            //this.unlockedMissions = new List<int>();
            //this.unlockedChapters = new List<ChapterId>();
            //this.unlockedPoses = new List<ChapterId>();
            //this.unlockedWorks = new List<ChapterId>();
        }

        public virtual void InitializeCurrentPlayer(int playerIndex)
        {
            this.CurrentPlayerIndex = Mathf.Clamp(playerIndex, 0, this.LastPlayerIndex);
            this.Initialized = true;
        }

        public virtual void DeinitializeCurrentPlayer()
        {
            this.Initialized = false;
            this.CurrentPlayerIndex = 0;
            this.BattleTutorial = false;
            //this.BattleId = default;
            //this.unlockedMissions.Clear();
            //this.unlockedChapters.Clear();
            //this.unlockedPoses.Clear();
            //this.unlockedWorks.Clear();
        }

        public bool HasAnyPlayer()
        {
            var result = false;

            foreach (var save in this.data.Players)
            {
                if (save.Existed)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        //public void ClearUnlockedMissions()
        //    => this.unlockedMissions.Clear();

        //public void UnlockMission(int id)
        //{
        //    if (this.unlockedMissions.Contains(id))
        //        return;

        //    this.unlockedMissions.Add(id);
        //}

        //public void ClearUnlockedChapters()
        //    => this.unlockedChapters.Clear();

        //public void UnlockChapter(in ChapterId id)
        //{
        //    if (this.unlockedChapters.Contains(id))
        //        return;

        //    this.unlockedChapters.Add(id);
        //}

        //public void ClearUnlockedPoses()
        //    => this.unlockedPoses.Clear();

        //public void UnlockPose(in ChapterId id)
        //{
        //    if (this.unlockedPoses.Contains(id))
        //        return;

        //    this.unlockedPoses.Add(id);
        //}

        //public void ClearUnlockedWorks()
        //    => this.unlockedWorks.Clear();

        //public void UnlockWork(in ChapterId id)
        //{
        //    if (this.unlockedWorks.Contains(id))
        //        return;

        //    this.unlockedWorks.Add(id);
        //}

        public bool IsBattleTutorial()
            => this.BattleTutorial || !this.CurrentPlayer.DoneBattleTutorial;

        public bool IsValidCurrentPlayerIndex(int index)
            => index >= 0 && index <= this.LastPlayerIndex;

        public bool TryGetPlayer(int index, out TPlayerData data)
        {
            data = default;

            if (!IsValidCurrentPlayerIndex(index))
                return false;

            data = this.data.Players[index];
            return true;
        }

        public void DeletePlayer(int index)
        {
            if (!IsValidCurrentPlayerIndex(index))
                return;

            this.data.Players[index].Reset();
        }

        public void SetBattleTutorial(bool value = true)
        {
            if (!Validate())
                return;

            this.CurrentPlayer.DoneBattleTutorial = value;
        }

        public void SetLobbyTutorial(bool value = true)
        {
            if (!Validate())
                return;

            this.CurrentPlayer.DoneLobbyTutorial = value;
        }

        public bool ToggleTutorial()
        {
            if (!Validate())
                return false;

            this.CurrentPlayer.DoneBattleTutorial = !this.CurrentPlayer.DoneBattleTutorial;
            return this.CurrentPlayer.DoneBattleTutorial;
        }

        public int ChangePlayerGold(int amount)
        {
            if (!Validate())
                return 0;

            if (amount != 0)
                this.CurrentPlayer.Gold += amount;

            return this.CurrentPlayer.Gold;
        }

        public int ChangePlayerPoint(int amount)
        {
            if (!Validate())
                return 0;

            if (amount != 0)
                this.CurrentPlayer.Point += amount;

            return this.CurrentPlayer.Point;
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

        public bool IsBad()
            => this.CurrentPlayer.BadPoint > this.CurrentPlayer.GoodPoint;

        public void Load()
            => this.data.Copy(this.handler.Load());

        public void Save()
            => this.handler.Save(this.data);

        public void SaveSettings()
        {
            if (this.Daemon)
                UnuLogger.Log(JsonConvert.SerializeObject(this.Settings));

            this.handler.Save(this.data);
        }

        public void SavePlayer()
        {
            if (!Validate())
                return;

            if (this.Daemon)
                UnuLogger.Log(JsonConvert.SerializeObject(this.CurrentPlayer));

            ChangePlayerLastTime();
            this.handler.Save(this.data);
        }

        private bool Validate()
        {
            if (!this.Initialized)
            {
                UnuLogger.LogError($"Must call {nameof(InitializeCurrentPlayer)}() before using this method.");
                return false;
            }

            return true;
        }
    }
}