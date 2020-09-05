
using UnuGames;
using UnuGames.MVVM;

// This code is generated automatically by UIMan - UI Generator, please do not modify!

namespace HegaCore.UI
{
    public sealed partial class UIConversationDialog
    {
        private string m_speakerAvatar = default;

        [UIManAutoProperty]
        public string SpeakerAvatar
        {
            get { return this.m_speakerAvatar; }
            set { this.m_speakerAvatar = value; OnPropertyChanged(nameof(this.SpeakerAvatar), value); }
        }

        private string m_avatarAtlas = default;

        [UIManAutoProperty]
        public string AvatarAtlas
        {
            get { return this.m_avatarAtlas; }
            set { this.m_avatarAtlas = value; OnPropertyChanged(nameof(this.AvatarAtlas), value); }
        }

        private bool m_hasSpeakerName = default;

        [UIManAutoProperty]
        public bool HasSpeakerName
        {
            get { return this.m_hasSpeakerName; }
            set { this.m_hasSpeakerName = value; OnPropertyChanged(nameof(this.HasSpeakerName), value); }
        }

        private string m_speakerName = default;

        [UIManAutoProperty]
        public string SpeakerName
        {
            get { return this.m_speakerName; }
            set { this.m_speakerName = value; OnPropertyChanged(nameof(this.SpeakerName), value); }
        }

        private bool m_isTyping = default;

        [UIManAutoProperty]
        public bool IsTyping
        {
            get { return this.m_isTyping; }
            set { this.m_isTyping = value; OnPropertyChanged(nameof(this.IsTyping), value); }
        }
    }
}
