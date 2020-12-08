using System;
using System.IO;

namespace HegaCore
{
    [Serializable]
    public abstract class GameDataHandlerBinary<TPlayerData, TGameData> : GameDataHandler<TPlayerData, TGameData>
        where TPlayerData : PlayerData<TPlayerData>, new()
        where TGameData : GameData<TPlayerData>
    {
        protected sealed override bool TryRead(string filePath, out TGameData data)
        {
            var @default = New();

            try
            {
                data = FileSystem.ReadFromBinaryFile<TGameData>(filePath, @default);
                return true;
            }
            catch
            {
                data = @default;
                return false;
            }
        }

        protected sealed override void Write(TGameData data, string filePath)
            => FileSystem.WriteToBinaryFile(filePath, data);

        protected virtual string Encrypt(string input)
            => input;

        protected virtual string Decrypt(string input)
            => input;
    }
}