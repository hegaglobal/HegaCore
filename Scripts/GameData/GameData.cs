using System;
//using System.Collections.Generic;
using UnityEngine;

namespace HegaCore
{
    public class GameData
    {
        public bool Daemon { get; set; }

        public int LastPlayerIndex { get; }

        public PlayerSave PlayerSave
            => this.userSaveData.PlayerSaves[this.PlayerIndex];

        public GameSettings Settings
            => this.userSaveData.Settings;

        public bool Initialized { get; private set; }

        public int PlayerIndex { get; private set; }

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

        private readonly UserSaveData userSaveData;
        private readonly ISaveDataHandler handler;

        //private readonly List<int> unlockedMissions;
        //private readonly List<ChapterId> unlockedChapters;
        //private readonly List<ChapterId> unlockedPoses;
        //private readonly List<ChapterId> unlockedWorks;

        public GameData(UserSaveData saveData, ISaveDataHandler handler)
        {
            this.userSaveData = saveData ?? throw new ArgumentNullException(nameof(saveData));
            this.handler = handler ?? throw new ArgumentNullException(nameof(handler));

            this.LastPlayerIndex = this.userSaveData.PlayerSaves.Length - 1;
            this.PlayerIndex = 0;

            //this.unlockedMissions = new List<int>();
            //this.unlockedChapters = new List<ChapterId>();
            //this.unlockedPoses = new List<ChapterId>();
            //this.unlockedWorks = new List<ChapterId>();
        }

        public void InitializePlayerSave(int playerIndex)
        {
            this.PlayerIndex = Mathf.Clamp(playerIndex, 0, this.LastPlayerIndex);
            this.Initialized = true;
        }

        public void DeinitializePlayerSave()
        {
            this.Initialized = false;
            this.PlayerIndex = 0;
            //this.BattleId = default;
            this.BattleTutorial = false;
            //this.unlockedMissions.Clear();
            //this.unlockedChapters.Clear();
            //this.unlockedPoses.Clear();
            //this.unlockedWorks.Clear();
        }

        public bool HasAnySave()
        {
            var result = false;

            foreach (var save in this.userSaveData.PlayerSaves)
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
            => this.BattleTutorial || !this.PlayerSave.DoneBattleTutorial;

        public bool IsValidPlayerIndex(int index)
            => index >= 0 && index <= this.LastPlayerIndex;

        public bool TryGetPlayer(int index, out PlayerSave data)
        {
            data = default;

            if (!IsValidPlayerIndex(index))
                return false;

            data = this.userSaveData.PlayerSaves[index];
            return true;
        }

        public void DeletePlayer(int index)
        {
            if (!IsValidPlayerIndex(index))
                return;

            this.userSaveData.PlayerSaves[index].New();
        }

        public void SetBattleTutorial(bool value = true)
        {
            if (!Validate())
                return;

            this.PlayerSave.DoneBattleTutorial = value;
        }

        public void SetLobbyTutorial(bool value = true)
        {
            if (!Validate())
                return;

            this.PlayerSave.DoneLobbyTutorial = value;
        }

        public bool ToggleTutorial()
        {
            if (!Validate())
                return false;

            this.PlayerSave.DoneBattleTutorial = !this.PlayerSave.DoneBattleTutorial;
            return this.PlayerSave.DoneBattleTutorial;
        }

        public int ChangePlayerGold(int amount)
        {
            if (!Validate())
                return 0;

            if (amount != 0)
                this.PlayerSave.Gold += amount;

            return this.PlayerSave.Gold;
        }

        public int ChangePlayerPoint(int amount)
        {
            if (!Validate())
                return 0;

            if (amount != 0)
                this.PlayerSave.Point += amount;

            return this.PlayerSave.Point;
        }

        public int ChangePlayerGoodPoint(int amount)
        {
            if (!Validate())
                return 0;

            if (amount != 0)
                this.PlayerSave.GoodPoint += amount;

            return this.PlayerSave.GoodPoint;
        }

        public int ChangePlayerBadPoint(int amount)
        {
            if (!Validate())
                return 0;

            if (amount != 0)
                this.PlayerSave.BadPoint += amount;

            return this.PlayerSave.BadPoint;
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

            this.PlayerSave.Existed = true;
            this.PlayerSave.LastTime = lastTime;
        }

        public bool IsBad()
            => this.PlayerSave.BadPoint > this.PlayerSave.GoodPoint;

        public void Load()
            => this.userSaveData.Copy(this.handler.Load());

        public void Save()
            => this.handler.Save(this.userSaveData);

        public void SaveSettings()
        {
            if (this.Daemon)
                UnuLogger.Log(Newtonsoft.Json.JsonConvert.SerializeObject(this.Settings));

            this.handler.Save(this.userSaveData);
        }

        public void SavePlayer()
        {
            if (!Validate())
                return;

            if (this.Daemon)
                UnuLogger.Log(Newtonsoft.Json.JsonConvert.SerializeObject(this.PlayerSave));

            ChangePlayerLastTime();
            this.handler.Save(this.userSaveData);
        }

        private bool Validate()
        {
            if (!this.Initialized)
            {
                UnuLogger.LogError($"{nameof(GameData)}.{nameof(InitializePlayerSave)}() must be called before using this method.");
                return false;
            }

            return true;
        }
    }
}