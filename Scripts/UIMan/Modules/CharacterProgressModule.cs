using System.Table;
using UnityEngine;
using UnuGames;
using DG.Tweening;

namespace HegaCore.UI
{
    using Database;

    public delegate void CharacterMilestoneAction(int milestone);

    public sealed class CharacterProgressModule : UIManModule<CharacterProgressViewModel>
    {
        public event CharacterMilestoneAction OnMilestone;

        [SerializeField]
        private float tweenDuration = 1f;

        private ReadTable<CharacterEntry> table;
        private int characterId;
        private Tweener valueTween;

        public void Initialize(in ReadTable<CharacterEntry> table)
        {
            this.table = table;
            ResetData();
        }

        public void InitializeProgress(in ReadTable<CharacterEntry> table, int characterId, int value)
        {
            this.table = table;
            this.characterId = characterId;

            if (!this.table.TryGetEntry(characterId, out var data))
                return;

            SetData(data);

            this.DataInstance.Value = value;
            this.DataInstance.Interactable_1 = value >= data.Milestone_1;
            this.DataInstance.Interactable_2 = value >= data.Milestone_2;
        }

        public void UpdateProgress(int characterId, int value)
        {
            var newGirl = this.characterId != characterId;
            this.characterId = characterId;

            ResetData();

            if (!this.table.TryGetEntry((int)characterId, out var data))
                return;

            SetData(data);

            this.valueTween?.Kill();
            this.valueTween = DOTween.To(() => this.DataInstance.Value, SetValue, value, this.tweenDuration)
                                     .SetEase(Ease.InOutQuad)
                                     .OnComplete(OnUpdateComplete);
        }

        private void OnUpdateComplete()
        {
        }

        private void SetValue(float value)
        {
            this.DataInstance.Value = value;
            this.DataInstance.Interactable_1 = value >= this.DataInstance.Milestone_1;
            this.DataInstance.Interactable_2 = value >= this.DataInstance.Milestone_2;
        }

        private void ResetData()
        {
            this.DataInstance.Max = 0f;
            this.DataInstance.Value = 0f;
            this.DataInstance.Milestone_1 = 0f;
            this.DataInstance.Milestone_2 = 0f;
            this.DataInstance.Interactable_1 = false;
            this.DataInstance.Interactable_2 = false;
        }

        private void SetData(CharacterEntry data)
        {
            this.DataInstance.Max = data.Max;
            this.DataInstance.Milestone_1 = data.Milestone_1;
            this.DataInstance.Milestone_2 = data.Milestone_2;
        }

        public void UI_Button_Click_Milestone(int milestone)
            => this.OnMilestone?.Invoke(milestone);
    }
}