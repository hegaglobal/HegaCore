using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HegaCore
{
    public abstract class GameDataContainer
    {
        public abstract PlayerData Player { get; }

        public abstract GameSettings Settings { get; }

        public bool CurrentPlayerInitialized { get; protected set; }

        public int CurrentPlayerIndex { get; protected set; }

        public int LastPlayerIndex { get; protected set; }

        public bool Daemon { get; set; }

        public bool DarkLord { get; set; }

        public bool BattleTutorial { get; set; }

        public int CurrentMission { get; set; }

        public ReadList<int> Missions => this.missions;

        public ReadList<int> PendingMissions => this.pendingMissions;

        public ReadList<int> PassedMissions => this.passedMissions;

        public ReadList<CharacterId> CharacterImages => this.characterImages;

        public ReadList<CharacterId> CharacterClips => this.characterClips;

        private readonly List<int> missions = new List<int>();
        private readonly List<int> pendingMissions = new List<int>();
        private readonly List<int> passedMissions = new List<int>();
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

            return this.BattleTutorial || !this.Player.DoneBattleTutorial;
        }

        public void SetPlayerGameMode(GameMode value)
        {
            if (!Validate())
                return;

            this.Player.GameMode = value;
        }

        public void SetPlayerBattleTutorial(bool value = true)
        {
            if (!Validate())
                return;

            this.Player.DoneBattleTutorial = value;
        }

        public void SetPlayerLobbyTutorial(bool value = true)
        {
            if (!Validate())
                return;

            this.Player.DoneLobbyTutorial = value;
        }

        public bool TogglePlayerBattleTutorial()
        {
            if (!Validate())
                return false;

            this.Player.DoneBattleTutorial = !this.Player.DoneBattleTutorial;
            return this.Player.DoneBattleTutorial;
        }

        public bool TogglePlayerLobbyTutorial()
        {
            if (!Validate())
                return false;

            this.Player.DoneLobbyTutorial = !this.Player.DoneLobbyTutorial;
            return this.Player.DoneLobbyTutorial;
        }

        public bool ChangePlayerWealth(int amount)
        {
            if (!Validate())
                return false;

            if (amount == 0)
                return false;

            this.Player.Wealth += amount;
            return true;
        }

        public int GetPlayerProgressPoint()
        {
            if (!Validate())
                return 0;

            return this.Player.ProgressPoint;
        }

        public bool ChangePlayerProgressPoint(int amount)
        {
            if (!Validate())
                return false;

            if (amount == 0)
                return false;

            this.Player.ProgressPoint += amount;
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

            this.Player.ProgressPoint += amount;
            return true;
        }

        public bool ChangePlayerGoodPoint(int amount)
        {
            if (!Validate())
                return false;

            if (amount == 0)
                return false;

            this.Player.GoodPoint += amount;
            return true;
        }

        public bool ChangePlayerBadPoint(int amount)
        {
            if (!Validate())
                return false;

            if (amount == 0)
                return false;

            this.Player.BadPoint += amount;
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

            this.Player.Existed = true;
            this.Player.LastTime = lastTime;
        }

        public bool IsPlayerBad()
        {
            if (!Validate())
                return false;

            return this.Player.BadPoint > this.Player.GoodPoint;
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

        public bool PendingMission(int id)
        {
            if (this.pendingMissions.Contains(id))
                return false;

            this.pendingMissions.Add(id);
            return true;
        }

        public void ClearPendingMissions()
            => this.pendingMissions.Clear();

        public bool PassMission(int id)
        {
            if (this.passedMissions.Contains(id))
                return false;

            this.passedMissions.Add(id);
            return true;
        }

        public void ClearPassedMissions()
            => this.passedMissions.Clear();

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

            this.Player.CharacterId = value;
            return true;
        }

        public int GetPlayerCharacterProgress(int characterId, bool silent = false)
        {
            if (!Validate())
                return 0;

            if (!this.Player.CharacterProgressMap.ContainsKey(characterId))
            {
                if (!silent)
                    UnuLogger.LogError($"Cannot found any progress record for character={characterId}");

                return 0;
            }

            return this.Player.CharacterProgressMap[characterId];
        }

        public bool ChangePlayerCharacterProgress(int characterId, int amount)
        {
            if (!Validate())
                return false;

            if (this.Player.CharacterProgressMap.ContainsKey(characterId))
                this.Player.CharacterProgressMap[characterId] += amount;
            else
                this.Player.CharacterProgressMap[characterId] = amount;

            return true;
        }

        public bool SetPlayerCharacterProgress(int characterId, int value)
        {
            if (!Validate())
                return false;

            this.Player.CharacterProgressMap[characterId] = value;
            return true;
        }

        public bool UnlockPlayerCharacterProgress(int characterId, int value)
        {
            if (!Validate())
                return false;

            if (this.Player.CharacterProgressMap.TryGetValue(characterId, out var progress) &&
                progress >= value)
                return false;

            this.Player.CharacterProgressMap[characterId] = value;
            return true;
        }

        public abstract bool TryGetPlayer(int index, out PlayerData data);

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