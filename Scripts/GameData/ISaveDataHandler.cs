namespace HegaCore
{
    public interface ISaveDataHandler
    {
        SaveData Load();

        void Save(SaveData data);
    }
}