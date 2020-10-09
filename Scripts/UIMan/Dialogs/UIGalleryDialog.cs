
using UnuGames;
using UnuGames.MVVM;

// This code is generated automatically by UIMan - UI Generator, please do not modify!

namespace HegaCore.UI
{
    public partial class UIGalleryDialog
    {
        private string m_characterName = default;

        [UIManAutoProperty]
        public string CharacterName
        {
            get { return this.m_characterName; }
            set { this.m_characterName = value; OnPropertyChanged(nameof(this.CharacterName), value); }
        }
    }
}
