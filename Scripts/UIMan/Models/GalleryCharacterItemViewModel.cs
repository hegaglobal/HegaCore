
using UnuGames;
using UnuGames.MVVM;

// This code is generated automatically by UIMan - UI Generator, please do not modify!

namespace HegaCore.UI
{
    public partial class GalleryCharacterItemViewModel : ObservableModel
    {
        private bool m_unlocked = default;

        [UIManAutoProperty]
        public bool Unlocked
        {
            get { return this.m_unlocked; }
            set { this.m_unlocked = value; OnPropertyChanged(nameof(this.Unlocked), value); }
        }

        private bool m_selected = default;

        [UIManAutoProperty]
        public bool Selected
        {
            get { return this.m_selected; }
            set { this.m_selected = value; OnPropertyChanged(nameof(this.Selected), value); }
        }

        private string m_character = default;

        [UIManAutoProperty]
        public string Character
        {
            get { return this.m_character; }
            set { this.m_character = value; OnPropertyChanged(nameof(this.Character), value); }
        }
    }
}
