
using UnuGames;
using UnuGames.MVVM;

// This code is generated automatically by UIMan - UI Generator, please do not modify!

namespace HegaCore.UI
{
    public sealed partial class TooltipModel : ObservableModel
    {
        private string m_title = "";

        [UIManAutoProperty]
        public string Title
        {
            get { return this.m_title; }
            set { this.m_title = value; OnPropertyChanged(nameof(this.Title), value); }
        }

        private string m_content = "";

        [UIManAutoProperty]
        public string Content
        {
            get { return this.m_content; }
            set { this.m_content = value; OnPropertyChanged(nameof(this.Content), value); }
        }
    }
}
