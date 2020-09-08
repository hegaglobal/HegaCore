using System.Table;
using TinyCsvParser.Mapping;
using TinyCsvParser.TypeConverter;

namespace HegaCore.Database
{
    public sealed class AudioEntry : IEntry
    {
        public int Id { get; private set; }

        public string Key { get; private set; }

        public string SecondKey { get; private set; }

        public AudioType Type { get; private set; }

        void IEntry.SetId(int value)
            => this.Id = value;

        public sealed class Mapping : CsvMapping<AudioEntry>
        {
            public Mapping()
            {
                var col = 0;

                MapProperty(++col, x => x.Key, (x, v) => x.Key = v);
                MapProperty(++col, x => x.SecondKey, (x, v) => x.SecondKey = v);
                MapProperty(++col, x => x.Type, (x, v) => x.Type = v, new EnumConverter<AudioType>());
            }
        }
    }
}