using System;

namespace HegaCore
{
    [Serializable]
    public class PlayerSave
    {
        public const int CurrentRevision = 1;

        public int Revision;

        public bool Existed;

        public bool DoneBattleTutorial;

        public bool DoneLobbyTutorial;

        public int Gold;

        public int Point;

        public int GoodPoint;

        public int BadPoint;

        public string LastTime;

        public PlayerSave()
        {
            this.Revision = 1;
        }

        public virtual void New()
        {
            this.Revision = CurrentRevision;
            this.Existed = false;
            this.DoneBattleTutorial = false;
            this.DoneLobbyTutorial = false;
            this.Gold = 0;
            this.Point = 0;
            this.GoodPoint = 0;
            this.BadPoint = 0;
            this.LastTime = string.Empty;
        }

        public virtual void Copy(PlayerSave data)
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

            this.Gold = data.Gold;
            this.Point = data.Point;
            this.GoodPoint = data.GoodPoint;
            this.BadPoint = data.BadPoint;
            this.LastTime = data.LastTime ?? string.Empty;
        }
    }
}