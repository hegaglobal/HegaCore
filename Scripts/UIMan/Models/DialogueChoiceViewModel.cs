
using UnuGames;
using UnuGames.MVVM;

// This code is generated automatically by UIMan - UI Generator, please do not modify!

namespace HegaCore.UI
{
    public sealed partial class DialogueChoiceViewModel : ObservableModel
    {
        private int m_id = default;

        [UIManAutoProperty]
        public int Id
        {
            get { return this.m_id; }
            set { this.m_id = value; OnPropertyChanged(nameof(this.Id), value); }
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
