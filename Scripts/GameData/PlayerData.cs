using System;

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
}