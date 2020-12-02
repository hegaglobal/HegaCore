using System;

namespace HegaCore.UI
{
    public delegate void GalleryClipAction(in CharacterId id, Action makeInvisible, Action makeVisible);

    public partial class UIGalleryDialog
    {
        public static class Settings
        {
            public static GameDataContainer DataContainer { get; private set; }

            public static int FirstCharacterId { get; private set; }

            public static GalleryClipAction ShowClip { get; private set; }

            public static void Initialize(GameDataContainer dataContainer, int firstCharacterId, GalleryClipAction showClip)
            {
                DataContainer = dataContainer;
                FirstCharacterId = firstCharacterId;
                ShowClip = showClip;
            }
        }
    }
}