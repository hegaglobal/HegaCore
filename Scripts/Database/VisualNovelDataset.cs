using VisualNovelData.Data;

namespace HegaCore
{
    public static class VisualNovelDataset
    {
        public static ReadCharacterData Character { get; private set; }

        public static ReadNovelData Novel { get; private set; }

        public static void Initialize(in ReadCharacterData character, in ReadNovelData novel)
        {
            Character = character;
            Novel = novel;
        }
    }
}