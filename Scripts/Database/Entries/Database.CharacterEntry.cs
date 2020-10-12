using System.Table;
using TinyCsvParser.Mapping;

namespace HegaCore.Database
{
    public sealed class CharacterEntry : IEntry
    {
        public int Id { get; private set; }

        public string Name { get; private set; }

        public int MaxProgress { get; private set; }

        public int Milestone_1 { get; private set; }

        public int Milestone_2 { get; private set; }

        public string Thumbnail_1 { get; private set; }

        public string Thumbnail_2 { get; private set; }

        public string IconKey { get; private set; }

        void IEntry.SetId(int value)
            => this.Id = value;

        public sealed class Mapping : CsvMapping<CharacterEntry>
        {
            public Mapping()
            {
                var col = 0;

                MapProperty(++col, x => x.Id, (x, v) => x.Id = v);
                MapProperty(++col, x => x.Name, (x, v) => x.Name = v);
                MapProperty(++col, x => x.MaxProgress, (x, v) => x.MaxProgress = v);
                MapProperty(++col, x => x.Milestone_1, (x, v) => x.Milestone_1 = v);
                MapProperty(++col, x => x.Milestone_2, (x, v) => x.Milestone_2 = v);
                MapProperty(++col, x => x.Thumbnail_1, (x, v) => x.Thumbnail_1 = v);
                MapProperty(++col, x => x.Thumbnail_2, (x, v) => x.Thumbnail_2 = v);
                MapProperty(++col, x => x.IconKey, (x, v) => x.IconKey = v);
            }
        }

        public readonly struct IdGetter : IGetId<CharacterEntry>
        {
            public int GetId(CharacterEntry entry)
                => entry.Id;
        }
    }
}