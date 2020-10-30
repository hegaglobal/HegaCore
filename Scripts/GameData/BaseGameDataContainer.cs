﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HegaCore
{
    public abstract class BaseGameDataContainer
    {
        public abstract BasePlayerData BasePlayer { get; }

        public abstract GameSettings Settings { get; }

        public bool CurrentPlayerInitialized { get; protected set; }

        public int CurrentPlayerIndex { get; protected set; }

        public int LastPlayerIndex { get; protected set; }

        public bool Daemon { get; set; }

        public bool DarkLord { get; set; }

        public bool BattleTutorial { get; set; }

        public int CurrentMission { get; set; }

        public ReadList<int> Missions => this.missions;

        public ReadList<int> PastMissions => this.pastMissions;

        public ReadList<CharacterId> CharacterImages => this.characterImages;

        public ReadList<CharacterId> CharacterClips => this.characterClips;

        private readonly List<int> missions = new List<int>();
        private readonly List<int> pastMissions = new List<int>();
        private readonly List<CharacterId> characterImages = new List<CharacterId>();
        private readonly List<CharacterId> characterClips = new List<CharacterId>();

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

        public bool IsValidPlayerIndex(int index)
            => index >= 0 && index <= this.LastPlayerIndex;

        public bool WillPlayerDoBattleTutorial()
        {
            if (!Validate())
                return false;

            return this.BattleTutorial || !this.BasePlayer.DoneBattleTutorial;
        }

        public void SetPlayerBattleTutorial(bool value = true)
        {
            if (!Validate())
                return;

            this.BasePlayer.DoneBattleTutorial = value;
        }

        public void SetPlayerLobbyTutorial(bool value = true)
        {
            if (!Validate())
                return;

            this.BasePlayer.DoneLobbyTutorial = value;
        }

        public bool TogglePlayerBattleTutorial()
        {
            if (!Validate())
                return false;

            this.BasePlayer.DoneBattleTutorial = !this.BasePlayer.DoneBattleTutorial;
            return this.BasePlayer.DoneBattleTutorial;
        }

        public bool TogglePlayerLobbyTutorial()
        {
            if (!Validate())
                return false;

            this.BasePlayer.DoneLobbyTutorial = !this.BasePlayer.DoneLobbyTutorial;
            return this.BasePlayer.DoneLobbyTutorial;
        }

        public int ChangePlayerWealth(int amount)
        {
            if (!Validate())
                return 0;

            if (amount != 0)
                this.BasePlayer.Wealth += amount;

            return this.BasePlayer.Wealth;
        }

        public int GetPlayerProgressPoint()
        {
            if (!Validate())
                return 0;

            return this.BasePlayer.ProgressPoint;
        }

        public int ChangePlayerProgressPoint(int amount)
        {
            if (!Validate())
                return 0;

            if (amount != 0)
                this.BasePlayer.ProgressPoint += amount;

            return this.BasePlayer.ProgressPoint;
        }

        public int ChangePlayerProgressPoint(int missionId, int amount)
        {
            if (!Validate())
                return 0;

            if (this.CurrentMission == missionId &&
                this.missions.Contains(missionId))
            {
                var latest = this.missions.Max();

                if (this.CurrentMission == latest && amount != 0)
                    this.BasePlayer.ProgressPoint += amount;
            }

            return this.BasePlayer.ProgressPoint;
        }

        public int ChangePlayerGoodPoint(int amount)
        {
            if (!Validate())
                return 0;

            if (amount != 0)
                this.BasePlayer.GoodPoint += amount;

            return this.BasePlayer.GoodPoint;
        }

        public int ChangePlayerBadPoint(int amount)
        {
            if (!Validate())
                return 0;

            if (amount != 0)
                this.BasePlayer.BadPoint += amount;

            return this.BasePlayer.BadPoint;
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

            this.BasePlayer.Existed = true;
            this.BasePlayer.LastTime = lastTime;
        }

        public bool IsPlayerBad()
        {
            if (!Validate())
                return false;

            return this.BasePlayer.BadPoint > this.BasePlayer.GoodPoint;
        }

        public void UnlockMission(int id)
        {
            if (!this.missions.Contains(id))
                this.missions.Add(id);
        }

        public void ClearMissions()
            => this.missions.Clear();

        public void PassMission(int id)
        {
            if (!this.pastMissions.Contains(id))
                this.pastMissions.Add(id);
        }

        public void ClearPastMissions()
            => this.pastMissions.Clear();

        public void ClearCharacterImages()
            => this.characterImages.Clear();

        public void UnlockCharacterImage(in CharacterId id)
        {
            if (this.characterImages.Contains(id))
                return;

            this.characterImages.Add(id);
        }

        public void ClearCharacterClips()
            => this.characterClips.Clear();

        public void UnlockCharacterClip(in CharacterId id)
        {
            if (this.characterClips.Contains(id))
                return;

            this.characterClips.Add(id);
        }

        public void SetPlayerCharacter(int value)
        {
            if (!Validate())
                return;

            this.BasePlayer.CharacterId = value;
        }

        public int GetPlayerCharacterProgress(int characterId, bool silent = false)
        {
            if (!Validate())
                return 0;

            if (!this.BasePlayer.CharacterProgressMap.ContainsKey(characterId))
            {
                if (!silent)
                    UnuLogger.LogError($"Cannot found any progress record for character={characterId}");

                return 0;
            }

            return this.BasePlayer.CharacterProgressMap[characterId];
        }

        public int ChangePlayerCharacterProgress(int characterId, int amount)
        {
            if (!Validate())
                return 0;

            var afterChanged = this.BasePlayer.CharacterProgressMap.ContainsKey(characterId)
                ? (this.BasePlayer.CharacterProgressMap[characterId] += amount)
                : (this.BasePlayer.CharacterProgressMap[characterId] = amount);

            return afterChanged;
        }

        public int SetPlayerCharacterProgress(int characterId, int value)
        {
            if (!Validate())
                return 0;

            this.BasePlayer.CharacterProgressMap[characterId] = value;
            return value;
        }

        public abstract bool TryGetPlayer(int index, out BasePlayerData data);

        public abstract bool HasAnyPlayer();

        public abstract void DeletePlayer(int index);

        public abstract void Load(bool shouldBackUp = false);

        public abstract void Save();

        public abstract void SaveSettings();

        public abstract void SavePlayer();

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