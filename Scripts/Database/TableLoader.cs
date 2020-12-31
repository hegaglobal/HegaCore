using System.Collections.Generic;
using System.Table;
using UnityEngine;
using TinyCsvParser.Mapping;
using VisualNovelData.Parser;

namespace HegaCore
{
    using Database.Csv;

    public class TableLoader
    {
        public DatabaseConfig Config { get; }

        private readonly CsvDataLoader csvLoader;

        public TableLoader(DatabaseConfig config)
        {
            this.Config = config;
            this.csvLoader = new CsvDataLoader();
            this.csvLoader.Initialize(this.Config);
        }

        public bool TryGetCsv(string name, out TextAsset csv, bool silent = false)
        {
            if (!this.Config.CsvFiles.TryGetValue(name, out csv))
            {
                csv = null;

                if (!silent)
                    UnuLogger.LogError($"Cannot find CSV file by name={name}", this.Config);
            }

            return csv;
        }

        public void Load<TEntity, TMapping>(ITable<TEntity> table, string file, bool autoIncrement = false)
            where TEntity : class, IEntry, new()
            where TMapping : CsvMapping<TEntity>, new()
        {
            if (!TryGetCsv(file, out var csv))
                return;

            this.csvLoader.Load<TEntity, TMapping>(table, csv, autoIncrement);
        }

        public void Load<TEntity, TMapping>(ITable<TEntity> table, string file, IGetId<TEntity> idGetter)
            where TEntity : class, IEntry, new()
            where TMapping : CsvMapping<TEntity>, new()
        {
            if (!TryGetCsv(file, out var csv))
                return;

            this.csvLoader.Load<TEntity, TMapping>(table, csv, idGetter);
        }

        public void Load<TEntity, TMapping, TIdGetter>(ITable<TEntity> table, string file)
            where TEntity : class, IEntry, new()
            where TMapping : CsvMapping<TEntity>, new()
            where TIdGetter : IGetId<TEntity>, new()
        {
            if (!TryGetCsv(file, out var csv))
                return;

            this.csvLoader.Load<TEntity, TMapping, TIdGetter>(table, csv);
        }

        public void Load<TData, TParser>(TData data, string file, in Segment<string> languages)
            where TData : class
            where TParser : ICsvParser<TData>, new()
        {
            if (!TryGetCsv(file, out var csv))
                return;

            this.csvLoader.Load<TData, TParser>(data, csv, languages);
        }

        public bool TryCreate<TEntity, TMapping>(out Table<TEntity> table, string file, bool autoIncrement = false)
            where TEntity : class, IEntry, new()
            where TMapping : CsvMapping<TEntity>, new()
        {
            if (!TryGetCsv(file, out var csv))
            {
                table = default;
                return false;
            }

            table = new Table<TEntity>();
            this.csvLoader.Load<TEntity, TMapping>(table, csv, autoIncrement);
            return true;
        }

        public bool TryCreate<TEntity, TMapping>(out Table<TEntity> table, string file, IGetId<TEntity> idGetter)
            where TEntity : class, IEntry, new()
            where TMapping : CsvMapping<TEntity>, new()
        {
            if (!TryGetCsv(file, out var csv))
            {
                table = default;
                return false;
            }

            table = new Table<TEntity>();
            this.csvLoader.Load<TEntity, TMapping>(table, csv, idGetter);
            return true;
        }

        public bool TryCreate<TEntity, TMapping, TIdGetter>(out Table<TEntity> table, string file)
            where TEntity : class, IEntry, new()
            where TMapping : CsvMapping<TEntity>, new()
            where TIdGetter : IGetId<TEntity>, new()
        {
            if (!TryGetCsv(file, out var csv))
            {
                table = default;
                return false;
            }

            table = new Table<TEntity>();
            this.csvLoader.Load<TEntity, TMapping, TIdGetter>(table, csv);
            return true;
        }
    }
}
