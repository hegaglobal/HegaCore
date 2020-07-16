
using UnuGames;
using UnuGames.MVVM;

// This code is generated automatically by UIMan - UI Generator, please do not modify!

namespace HegaCore
{
    public sealed partial class UIModalDialog
    {
        private string m_title = default;

        [UIManAutoProperty]
        public string Title
        {
            get { return this.m_title; }
            set { this.m_title = value; OnPropertyChanged(nameof(this.Title), value); }
        }

        private string m_cotent = default;

        [UIManAutoProperty]
        public string Content
        {
            get { return this.m_cotent; }
            set { this.m_cotent = value; OnPropertyChanged(nameof(this.Content), value); }
        }

        private bool m_hasTitle = default;

        [UIManAutoProperty]
        public bool HasTitle
        {
            get { return this.m_hasTitle; }
            set { this.m_hasTitle = value; OnPropertyChanged(nameof(this.HasTitle), value); }
        }

        private bool m_isProceed = default;

        [UIManAutoProperty]
        public bool IsProceed
        {
            get { return this.m_isProceed; }
            set { this.m_isProceed = value; OnPropertyChanged(nameof(this.IsProceed), value); }
        }
    }
}
