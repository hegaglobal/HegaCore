using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Text;
using Newtonsoft.Json;

namespace HegaCore
{
    [Serializable]
    public abstract class GameDataHandlerJson<TPlayerData, TGameData> : GameDataHandler<TPlayerData, TGameData>, ITextFormatter<TGameData>
        where TPlayerData : PlayerData<TPlayerData>, new()
        where TGameData : GameData<TPlayerData>
    {
        protected override bool TryRead(string filePath, out TGameData data)
        {
            var @default = New();

            try
            {
                data = FileSystem.ReadFromTextFile(filePath, this, @default);
                return true;
            }
            catch
            {
                data = @default;
                return false;
            }
        }

        protected override void Write(TGameData data, string filePath)
            => FileSystem.WriteToTextFile(filePath, data, this);

        public virtual TGameData Deserialize(TextReader reader)
            => JsonConvert.DeserializeObject<TGameData>(Decrypt(reader.ReadToEnd()));

        public virtual void Serialize(TextWriter writer, TGameData @object)
            => writer.WriteLine(Encrypt(JsonConvert.SerializeObject(@object)));

        protected virtual string Encrypt(string input)
            => input;

        protected virtual string Decrypt(string input)
            => input;
    }
}