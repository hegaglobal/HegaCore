﻿using System.Table;
using TinyCsvParser.Mapping;
using TinyCsvParser.TypeConverter;

namespace HegaCore.Database
{
    public sealed class GameModeEntry : IEntry
    {
        public int Id { get; private set; }

        public GameMode Mode { get; private set; }

        public bool IsUsed { get; private set; }

        void IEntry.SetId(int value)
            => this.Id = value;

        public sealed class Mapping : CsvMapping<GameModeEntry>
        {
            public Mapping()
            {
                var col = 0;

                MapProperty(++col, x => x.Mode, (x, v) => x.Mode = v, new EnumConverter<GameMode>());
                MapProperty(++col, x => x.IsUsed, (x, v) => x.IsUsed = v);
            }
        }
    }
}