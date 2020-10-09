using System;
using System.Collections.Generic;
using System.Table;
using VisualNovelData.Data;

namespace HegaCore.UI
{
    using Database;

    public delegate void GalleryClipAction(in CharacterId id, Action makeInvisible, Action makeVisible);

    public partial class UIGalleryDialog
    {
        public static class Settings
        {
            public static BaseGameDataContainer DataContainer { get; private set; }

            public static ReadCharacterData CharacterData { get; private set; }

            public static ReadTable<CharacterEntry> CharacterTable { get; private set; }

            public static ReadDictionary<string, int> CharacterMap { get; private set; }

            public static int FirstCharacterId { get; private set; }

            public static GalleryClipAction ShowClip { get; private set; }

            public static void Initialize(BaseGameDataContainer dataContainer, in ReadCharacterData characterData, in ReadTable<CharacterEntry> characterTable,
                                          in ReadDictionary<string, int> characterMap, int firstCharacterId, GalleryClipAction showClip)
            {
                DataContainer = dataContainer;
                CharacterData = characterData;
                CharacterTable = characterTable;
                CharacterMap = characterMap;
                FirstCharacterId = firstCharacterId;
                ShowClip = showClip;
            }
        }
    }
}