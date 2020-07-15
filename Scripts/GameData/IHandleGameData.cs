namespace HegaCore
{
    public interface IHandleGameData<TGameData, TPlayerData>
        where TGameData : GameData<TPlayerData>
        where TPlayerData : PlayerData<TPlayerData>, new()
    {
        TGameData Load();

        void Save(TGameData data);
    }
}