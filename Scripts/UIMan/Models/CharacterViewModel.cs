
using UnuGames;
using UnuGames.MVVM;

// This code is generated automatically by UIMan - UI Generator, please do not modify!

namespace HegaCore.UI
{
    public partial class CharacterViewModel : ObservableModel
    {
        private bool m_unlocked = default;

        [UIManAutoProperty]
        public bool Unlocked
        {
            get { return this.m_unlocked; }
            set { this.m_unlocked = value; OnPropertyChanged(nameof(this.Unlocked), value); }
        }

        private float m_grayscaleAmount = default;

        [UIManAutoProperty]
        public float GrayscaleAmount
        {
            get { return this.m_grayscaleAmount; }
            set { this.m_grayscaleAmount = value; OnPropertyChanged(nameof(this.GrayscaleAmount), value); }
        }

        private string m_image = default;

        [UIManAutoProperty]
        public string Image
        {
            get { return this.m_image; }
            set { this.m_image = value; OnPropertyChanged(nameof(this.Image), value); }
        }

        private bool m_selected = default;

        [UIManAutoProperty]
        public bool Selected
        {
            get { return this.m_selected; }
            set { this.m_selected = value; OnPropertyChanged(nameof(this.Selected), value); }
        }
    }
}
