
using UnuGames;
using UnuGames.MVVM;

// This code is generated automatically by UIMan - UI Generator, please do not modify!

namespace HegaCore.UI
{
    public partial class UIGalleryDialog
    {
        private string m_characterKey = default;

        [UIManAutoProperty]
        public string CharacterKey
        {
            get { return this.m_characterKey; }
            set { this.m_characterKey = value; OnPropertyChanged(nameof(this.CharacterKey), value); }
        }

        private string m_characterIconKey = default;

        [UIManAutoProperty]
        public string CharacterIconKey
        {
            get { return this.m_characterIconKey; }
            set { this.m_characterIconKey = value; OnPropertyChanged(nameof(this.CharacterIconKey), value); }
        }
    }
}
