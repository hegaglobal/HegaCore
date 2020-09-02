
using UnuGames;
using UnuGames.MVVM;

// This code is generated automatically by UIMan - UI Generator, please do not modify!

namespace HegaCore.UI
{
    public sealed partial class UIConversationDialog
    {
        private string m_avatar = default;

        [UIManAutoProperty]
        public string Avatar
        {
            get { return this.m_avatar; }
            set { this.m_avatar = value; OnPropertyChanged(nameof(this.Avatar), value); }
        }

        private string m_avatarAtlas = default;

        [UIManAutoProperty]
        public string AvatarAtlas
        {
            get { return this.m_avatarAtlas; }
            set { this.m_avatarAtlas = value; OnPropertyChanged(nameof(this.AvatarAtlas), value); }
        }

        private bool m_hasCharacterName = default;

        [UIManAutoProperty]
        public bool HasCharacterName
        {
            get { return this.m_hasCharacterName; }
            set { this.m_hasCharacterName = value; OnPropertyChanged(nameof(this.HasCharacterName), value); }
        }

        private string m_characterName = default;

        [UIManAutoProperty]
        public string CharacterName
        {
            get { return this.m_characterName; }
            set { this.m_characterName = value; OnPropertyChanged(nameof(this.CharacterName), value); }
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
