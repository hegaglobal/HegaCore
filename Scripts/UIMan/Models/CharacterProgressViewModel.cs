using UnuGames;
using UnuGames.MVVM;

// This code is generated automatically by UIMan - UI Generator, please do not modify!

namespace HegaCore.UI
{
    public sealed partial class CharacterProgressViewModel : ObservableModel
    {
        private float m_max = default;

        [UIManAutoProperty]
        public float Max
        {
            get { return this.m_max; }
            set { this.m_max = value; OnPropertyChanged(nameof(this.Max), value); }
        }

        private float m_value = default;

        [UIManAutoProperty]
        public float Value
        {
            get { return this.m_value; }
            set { this.m_value = value; OnPropertyChanged(nameof(this.Value), value); }
        }

        private float m_milestone_1 = default;

        [UIManAutoProperty]
        public float Milestone_1
        {
            get { return this.m_milestone_1; }
            set { this.m_milestone_1 = value; OnPropertyChanged(nameof(this.Milestone_1), value); }
        }

        private float m_milestone_2 = default;

        [UIManAutoProperty]
        public float Milestone_2
        {
            get { return this.m_milestone_2; }
            set { this.m_milestone_2 = value; OnPropertyChanged(nameof(this.Milestone_2), value); }
        }

        private bool m_interactable = default;

        [UIManAutoProperty]
        public bool Interactable
        {
            get { return this.m_interactable; }
            set { this.m_interactable = value; OnPropertyChanged(nameof(this.Interactable), value); }
        }

        private bool m_interactable_1 = default;

        [UIManAutoProperty]
        public bool Interactable_1
        {
            get { return this.m_interactable_1; }
            set { this.m_interactable_1 = value; OnPropertyChanged(nameof(this.Interactable_1), value); }
        }

        private bool m_interactable_2 = default;

        [UIManAutoProperty]
        public bool Interactable_2
        {
            get { return this.m_interactable_2; }
            set { this.m_interactable_2 = value; OnPropertyChanged(nameof(this.Interactable_2), value); }
        }
    }
}
