namespace HegaCore
{
    public interface ISaveDataHandler
    {
        UserSaveData Load();

        void Save(UserSaveData data);
    }
}