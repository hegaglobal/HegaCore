
using UnuGames;
using UnuGames.MVVM;

// This code is generated automatically by UIMan - UI Generator, please do not modify!

namespace HegaCore
{
    public sealed partial class EnergyModel : ObservableModel
    {
        private int m_maxValue = default;

        [UIManAutoProperty]
        public int MaxValue
        {
            get { return this.m_maxValue; }
            set { this.m_maxValue = value; OnPropertyChanged(nameof(this.MaxValue), value); }
        }

        private int m_value = default;

        [UIManAutoProperty]
        public int Value
        {
            get { return this.m_value; }
            set { this.m_value = value; OnPropertyChanged(nameof(this.Value), value); }
        }

        private float m_progress = default;

        [UIManAutoProperty]
        public float Progress
        {
            get { return this.m_progress; }
            set { this.m_progress = value; OnPropertyChanged(nameof(this.Progress), value); }
        }
    }
}
