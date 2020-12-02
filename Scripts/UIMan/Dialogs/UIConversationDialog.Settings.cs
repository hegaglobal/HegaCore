using VisualNovelData.Commands;

namespace HegaCore.UI
{
    public partial class UIConversationDialog
    {
        public static class Settings
        {
            public static string AvatarAtlasName { get; private set; }

            public static ConversationDurations Durations { get; private set; }

            public static float BackgroundDurationChange { get; private set; }

            public static GameDataContainer DataContainer { get; private set; }

            public static CommandSystem CommandSystem { get; private set; }

            public static void Initialize(string avatarAtlasName, in ConversationDurations durations, float bgDurationChange,
                                          GameDataContainer dataContainer, CommandSystem commandSystem)
            {
                AvatarAtlasName = avatarAtlasName;
                Durations = durations;
                BackgroundDurationChange = bgDurationChange;
                DataContainer = dataContainer;
                CommandSystem = commandSystem;
            }
        }
    }
}