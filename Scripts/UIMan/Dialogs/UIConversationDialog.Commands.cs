using UnityEngine;

namespace HegaCore.UI
{
    public partial class UIConversationDialog
    {
        public static class Commands
        {
            public static class Key
            {
                public const string SpeedUp = "SpeedUpConversation";
                public const string SkipNext = "SkipNextConversation";
            }

            private readonly struct ConversationSkipNext : ICommand
            {
                public void Execute()
                {
                }

                public void PostExecute()
                {
                }

                public void PreExecute()
                {
                }
            }

            private readonly struct ConversationSpeedUp : ICommand
            {
                public void Execute()
                {
                }

                public void PostExecute()
                {
                }

                public void PreExecute()
                {
                }
            }
        }
    }
}