#if UNITY_EDITOR

using UnityEngine;

namespace HegaCore.Editor
{
    public class PlayerSaveViewer : MonoBehaviour
    {
        [Header("Save Data")]
        [SerializeField]
        private int Revision = default;

        [SerializeField]
        private bool Existed = default;

        [SerializeField]
        private bool DoneBattleTutorial = default;

        [SerializeField]
        private bool DoneLobbyTutorial = default;

        [SerializeField]
        private int Gold = default;

        [SerializeField]
        private int Point = default;

        [SerializeField]
        private int GoodPoint = default;

        [SerializeField]
        private int BadPoint = default;

        [SerializeField]
        private string LastTime = default;

        public void Set(PlayerSave data)
        {
            this.Revision = data.Revision;
            this.Existed = data.Existed;
            this.DoneBattleTutorial = data.DoneBattleTutorial;
            this.DoneLobbyTutorial = data.DoneLobbyTutorial;
            this.Gold = data.Gold;
            this.Point = data.Point;
            this.GoodPoint = data.GoodPoint;
            this.BadPoint = data.BadPoint;
            this.LastTime = data.LastTime;
        }

        public void CopyTo(PlayerSave data)
        {
            data.Revision = this.Revision;
            data.Existed = this.Existed;
            data.DoneBattleTutorial = this.DoneBattleTutorial;
            data.DoneLobbyTutorial = this.DoneLobbyTutorial;
            data.Gold = this.Gold;
            data.Point = this.Point;
            data.GoodPoint = this.GoodPoint;
            data.BadPoint = this.BadPoint;
            data.LastTime = this.LastTime;
        }
    }
}

#endif