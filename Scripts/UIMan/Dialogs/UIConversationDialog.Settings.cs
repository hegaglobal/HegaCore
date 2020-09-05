using VisualNovelData.Data;
using VisualNovelData.Commands;

namespace HegaCore.UI
{
    public partial class UIConversationDialog
    {
        public static class Settings
        {
            public static string AvatarAtlasName { get; private set; }

            public static ActorDuration ActorDuration { get; private set; }

            public static BaseGameDataContainer DataContainer { get; private set; }

            public static CharacterData Character { get; private set; }

            public static NovelData Novel { get; private set; }

            public static CommandSystem CommandSystem { get; private set; }

            public static void Initialize(string avatarAtlasName, in ActorDuration actorDuration,
                                          BaseGameDataContainer dataContainer, CharacterData character, NovelData novel,
                                          CommandSystem commandSystem)
            {
                AvatarAtlasName = avatarAtlasName;
                ActorDuration = actorDuration;
                DataContainer = dataContainer;
                Character = character;
                Novel = novel;
                CommandSystem = commandSystem;
            }
        }
    }
}