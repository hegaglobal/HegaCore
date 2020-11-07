using System;
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

        public ReadList<int> PrePastMissions => this.prePastMissions;

        public ReadList<int> PastMissions => this.pastMissions;

        public ReadList<CharacterId> CharacterImages => this.characterImages;

        public ReadList<CharacterId> CharacterClips => this.characterClips;

        private readonly List<int> missions = new List<int>();
        private readonly List<int> prePastMissions = new List<int>();
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

        public void SetPlayerGameMode(GameMode value)
        {
            if (!Validate())
                return;

            this.BasePlayer.GameMode = value;
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

        public bool ChangePlayerWealth(int amount)
        {
            if (!Validate())
                return false;

            if (amount == 0)
                return false;

            this.BasePlayer.Wealth += amount;
            return true;
        }

        public int GetPlayerProgressPoint()
        {
            if (!Validate())
                return 0;

            return this.BasePlayer.ProgressPoint;
        }

        public bool ChangePlayerProgressPoint(int amount)
        {
            if (!Validate())
                return false;

            if (amount == 0)
                return false;

            this.BasePlayer.ProgressPoint += amount;
            return true;
        }

        public bool ChangePlayerProgressPoint(int missionId, int amount)
        {
            if (!Validate())
                return false;

            if (this.CurrentMission != missionId ||
                !this.missions.Contains(missionId))
                return false;

            var latest = this.missions.Max();

            if (this.CurrentMission != latest || amount == 0)
                return false;

            this.BasePlayer.ProgressPoint += amount;
            return true;
        }

        public bool ChangePlayerGoodPoint(int amount)
        {
            if (!Validate())
                return false;

            if (amount == 0)
                return false;

            this.BasePlayer.GoodPoint += amount;
            return true;
        }

        public bool ChangePlayerBadPoint(int amount)
        {
            if (!Validate())
                return false;

            if (amount == 0)
                return false;

            this.BasePlayer.BadPoint += amount;
            return true;
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

        public bool UnlockMission(int id)
        {
            if (this.missions.Contains(id))
                return false;

            this.missions.Add(id);
            return true;
        }

        public void ClearMissions()
            => this.missions.Clear();

        public bool PrePassMission(int id)
        {
            if (this.prePastMissions.Contains(id))
                return false;

            this.prePastMissions.Add(id);
            return true;
        }

        public void ClearPrePastMissions()
            => this.prePastMissions.Clear();

        public bool PassMission(int id)
        {
            if (this.pastMissions.Contains(id))
                return false;

            this.pastMissions.Add(id);
            return true;
        }

        public void ClearPastMissions()
            => this.pastMissions.Clear();

        public void ClearCharacterImages()
            => this.characterImages.Clear();

        public bool UnlockCharacterImage(in CharacterId id)
        {
            if (this.characterImages.Contains(id))
                return false;

            this.characterImages.Add(id);
            return true;
        }

        public void ClearCharacterClips()
            => this.characterClips.Clear();

        public bool UnlockCharacterClip(in CharacterId id)
        {
            if (this.characterClips.Contains(id))
                return false;

            this.characterClips.Add(id);
            return true;
        }

        public bool SetPlayerCharacter(int value)
        {
            if (!Validate())
                return false;

            this.BasePlayer.CharacterId = value;
            return true;
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

        public bool ChangePlayerCharacterProgress(int characterId, int amount)
        {
            if (!Validate())
                return false;

            if (this.BasePlayer.CharacterProgressMap.ContainsKey(characterId))
                this.BasePlayer.CharacterProgressMap[characterId] += amount;
            else
                this.BasePlayer.CharacterProgressMap[characterId] = amount;

            return true;
        }

        public bool SetPlayerCharacterProgress(int characterId, int value)
        {
            if (!Validate())
                return false;

            this.BasePlayer.CharacterProgressMap[characterId] = value;
            return true;
        }

        public bool UnlockPlayerCharacterProgress(int characterId, int value)
        {
            if (!Validate())
                return false;

            if (this.BasePlayer.CharacterProgressMap.TryGetValue(characterId, out var progress) &&
                progress >= value)
                return false;

            this.BasePlayer.CharacterProgressMap[characterId] = value;
            return true;
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