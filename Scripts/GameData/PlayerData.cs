using System;
using System.Collections.Generic;
using UnityEngine;

namespace HegaCore
{
    [Serializable]
    public abstract class PlayerData
    {
        public const int CurrentRevision = 1;

        public int Revision;

        public bool Existed;

        public bool DoneBattleTutorial;

        public bool DoneLobbyTutorial;

        public int Wealth;

        public int ProgressPoint;

        public int GoodPoint;

        public int BadPoint;

        public string LastTime;

        public int CharacterId;

        public int CurCharIndex;
        
        public GameMode GameMode;

        public CharacterProgressMap CharacterProgressMap = new CharacterProgressMap();

        Dictionary<int, UserCharacter> UserCharacterDict = new Dictionary<int, UserCharacter>();
        
        protected PlayerData()
        {
            this.Revision = 1;
            this.CurCharIndex = 0;
        }

        public virtual void Reset()
        {
            this.Revision = CurrentRevision;
            this.Existed = false;
            this.DoneBattleTutorial = false;
            this.DoneLobbyTutorial = false;
            this.Wealth = 0;
            this.ProgressPoint = 0;
            this.GoodPoint = 0;
            this.BadPoint = 0;
            this.LastTime = string.Empty;
            this.CharacterId = 0;
            this.CurCharIndex = 0;
            this.CharacterProgressMap.Clear();
            this.GameMode = GameMode.Normal;
            this.UserCharacterDict.Clear();
        }

        protected void CopyFrom(PlayerData data)
        {
            if (data == null)
                return;

            this.Revision = data.Revision;
            this.Existed = data.Existed;

            if (this.Existed && this.Revision < CurrentRevision)
            {
                this.DoneBattleTutorial = true;
                this.DoneLobbyTutorial = true;
            }
            else
            {
                this.DoneBattleTutorial = data.DoneBattleTutorial;
                this.DoneLobbyTutorial = data.DoneLobbyTutorial;
            }

            this.Wealth = data.Wealth;
            this.ProgressPoint = data.ProgressPoint;
            this.GoodPoint = data.GoodPoint;
            this.BadPoint = data.BadPoint;
            this.LastTime = data.LastTime ?? string.Empty;
            this.CharacterId = data.CharacterId;
            this.CurCharIndex = data.CurCharIndex;
            this.GameMode = data.GameMode;
            
            Copy(this.UserCharacterDict, data.UserCharacterDict);
            Copy(this.CharacterProgressMap, data.CharacterProgressMap);
        }
        
        public UserCharacter GetUserCharacter(int index)
        {
            if (!UserCharacterDict.ContainsKey(index))
            {
                UserCharacter newChar = new UserCharacter
                {
                    standClothesID = DataManager.Instance.DarkLord ? 1 : 0
                };
                UserCharacterDict.Add(index, newChar);
            }

            return UserCharacterDict[index];
        }
        
        public void Copy<TKey, TValue>(Dictionary<TKey, TValue> dest, Dictionary<TKey, TValue> source)
        {
            if (dest == null)
                return;

            dest.Clear();

            if (source != null)
                dest.AddRange(source);
        }

        public void Copy<TValue>(List<TValue> dest, List<TValue> source)
        {
            if (dest == null)
                return;

            dest.Clear();

            if (source != null)
                dest.AddRange(source);
        }

        public void Copy<TValue>(TValue[] dest, TValue[] source)
        {
            if (dest == null)
                return;

            for (var i = 0; i < dest.Length; i++)
            {
                dest[i] = default;
            }

            if (source == null)
                return;

            var length = Math.Min(dest.Length, source.Length);

            for (var i = 0; i < length; i++)
            {
                dest[i] = source[i];
            }
        }
    }

    [Serializable]
    public sealed class CharacterProgressMap : SerializableDictionary<int, int> { }
    
    [Serializable]
    public class UserCharacter
    {
        public int HeartEXP;
        public int HeartLevel;
        public int standClothesID;
        public Dictionary<string, float> interactValues;
        public bool hasBeenRewardedByInteract = false;

        public UserCharacter(int heartLevel, int heartExp)
        {
            HeartEXP = heartExp;
            HeartLevel = heartLevel;
            standClothesID = 0;
            interactValues = new Dictionary<string, float>();
        }
        public UserCharacter()
        {
            HeartEXP = 0;
            HeartLevel = 1;
            standClothesID = 0;
            interactValues = new Dictionary<string, float>();
            hasBeenRewardedByInteract = false;
        }
    }
}