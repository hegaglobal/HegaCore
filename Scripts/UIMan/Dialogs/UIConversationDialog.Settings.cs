using VisualNovelData.Commands;

namespace HegaCore.UI
{
    public partial class UIConversationDialog
    {
        public static class Settings
        {
            public static string AvatarAtlasName { get; private set; }

            public static ActorDuration ActorDuration { get; private set; }

            public static float BackgroundDurationChange { get; private set; }

            public static BaseGameDataContainer DataContainer { get; private set; }

            public static CommandSystem CommandSystem { get; private set; }

            public static void Initialize(string avatarAtlasName, in ActorDuration actorDuration, float bgDurationChange,
                                          BaseGameDataContainer dataContainer, CommandSystem commandSystem)
            {
                AvatarAtlasName = avatarAtlasName;
                ActorDuration = actorDuration;
                BackgroundDurationChange = bgDurationChange;
                DataContainer = dataContainer;
                CommandSystem = commandSystem;
            }
        }
    }
}