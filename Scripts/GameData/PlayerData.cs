using System;
using System.Collections.Generic;
using UnityEngine;

namespace HegaCore
{
    [Serializable]
    public abstract class BasePlayerData
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

        public CharacterProgressMap CharacterProgressMap = new CharacterProgressMap();

        public BasePlayerData()
        {
            this.Revision = 1;
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
            this.CharacterProgressMap.Clear();
        }

        protected void CopyFrom(BasePlayerData data)
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

            Copy(this.CharacterProgressMap, data.CharacterProgressMap);
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
    public abstract class PlayerData<T> : BasePlayerData where T : PlayerData<T>
    {
        public virtual void CopyFrom(T data)
        {
            base.CopyFrom(data);
        }
    }

    [Serializable]
    public sealed class CharacterProgressMap : SerializableDictionary<int, int> { }
}