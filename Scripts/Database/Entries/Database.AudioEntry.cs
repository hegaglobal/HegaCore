using TinyCsvParser.Mapping;
using TinyCsvParser.TypeConverter;

namespace HegaCore.Database
{
    public sealed class AudioEntry : Entry
    {
        public string Key { get; private set; }

        public string AssetKey { get; private set; }

        public AudioType Type { get; private set; }

        public sealed class Mapping : CsvMapping<AudioEntry>
        {
            public Mapping()
            {
                var col = 0;

                MapProperty(++col, x => x.Key, (x, v) => x.Key = v);
                MapProperty(++col, x => x.AssetKey, (x, v) => x.AssetKey = v);
                MapProperty(++col, x => x.Type, (x, v) => x.Type = v, new EnumConverter<AudioType>());
            }
        }
    }
}