
using UnuGames;
using UnuGames.MVVM;

// This code is generated automatically by UIMan - UI Generator, please do not modify!

namespace HegaCore.UI
{
    public sealed partial class VersionViewModel : ObservableModel
    {
        private string m_version = default;

        [UIManAutoProperty]
        public string Version
        {
            get { return this.m_version; }
            set { this.m_version = value; OnPropertyChanged(nameof(this.Version), value); }
        }
    }
}
