using TinyCsvParser.Mapping;

namespace HegaCore.Database
{
    public sealed class LanguageEntry : Entry
    {
        public string Key { get; private set; }

        public bool IsUsed { get; private set; }

        public sealed class Mapping : CsvMapping<LanguageEntry>
        {
            public Mapping()
            {
                var col = 0;

                MapProperty(++col, x => x.Key, (x, v) => x.Key = v);
                MapProperty(++col, x => x.IsUsed, (x, v) => x.IsUsed = v);
            }
        }
    }
}