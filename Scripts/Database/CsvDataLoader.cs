using System;
using System.Collections.Generic;
using System.IO;
using System.Table;
using UnityEngine;
using TinyCsvParser;
using TinyCsvParser.Advanced;
using TinyCsvParser.Mapping;
using TinyCsvParser.Tokenizer.RFC4180;
using System.Linq;

namespace HegaCore.Database.Csv
{
    public sealed class CsvDataLoader
    {
        private readonly CsvReaderOptions readerOptions;
        private readonly CsvParserOptions parserOptions;
        private readonly AdvancedCsvParserOptions advancedParserOptions;

        private DatabaseConfig config;

        public CsvDataLoader()
        {
            var options = new Options('"', '\\', ',');
            var tokenizer = new RFC4180Tokenizer(options);
            this.readerOptions = new CsvReaderOptions(new[] { "\r\n", "\n" });
            this.parserOptions = new CsvParserOptions(true, true, "//", tokenizer, 1, true);
            this.advancedParserOptions = new AdvancedCsvParserOptions(tokenizer, "//", 0, 2, true, true, true, true, 1);
        }

        public void Initialize(DatabaseConfig config)
        {
            this.config = config;
        }

        public void Load<TEntity, TMapping>(ITable<TEntity> table, TextAsset file, bool autoIncrement = false)
            where TEntity : class, IEntry, new()
            where TMapping : CsvMapping<TEntity>, new()
        {
            var csvData = GetCsvData(file);
            var entries = Pool.Provider.List<TEntity>();

            Parse<TEntity, TMapping>(csvData, entries);
            table.AddRange(entries, autoIncrement);

            Pool.Provider.Return(entries);
        }

        public void Load<TEntity, TMapping>(ITable<TEntity> table, TextAsset file, IGetId<TEntity> idGetter)
            where TEntity : class, IEntry, new()
            where TMapping : CsvMapping<TEntity>, new()
        {
            var csvData = GetCsvData(file);
            var entries = Pool.Provider.List<TEntity>();

            Parse<TEntity, TMapping>(csvData, entries);
            table.AddRange(entries, idGetter);

            Pool.Provider.Return(entries);
        }

        public void Load<TEntity, TMapping, TIdGetter>(ITable<TEntity> table, TextAsset file)
            where TEntity : class, IEntry, new()
            where TMapping : CsvMapping<TEntity>, new()
            where TIdGetter : IGetId<TEntity>, new()
        {
            var csvData = GetCsvData(file);
            var entries = Pool.Provider.List<TEntity>();

            Parse<TEntity, TMapping>(csvData, entries);
            table.AddRange(entries, new TIdGetter());

            Pool.Provider.Return(entries);
        }

        private void Parse<TEntity, TMapping>(string csvData, List<TEntity> output)
            where TEntity : class, new()
            where TMapping : CsvMapping<TEntity>, new()
        {
            var mapper = new TMapping();

            if (IsAdvanced<TEntity, TMapping>())
                AdvancedParse(csvData, mapper, output);
            else
                BasicParse(csvData, mapper, output);
        }

        public void Load<TEntity, TMapping>(ITable<TEntity> table, TMapping mapper, TextAsset file, bool autoIncrement = false)
            where TEntity : class, IEntry, new()
            where TMapping : CsvMapping<TEntity>
        {
            var csvData = GetCsvData(file);
            var entries = Pool.Provider.List<TEntity>();

            Parse<TEntity, TMapping>(csvData, mapper, entries);
            table.AddRange(entries, autoIncrement);

            Pool.Provider.Return(entries);
        }

        public void Load<TEntity, TMapping>(ITable<TEntity> table, TMapping mapper, TextAsset file, IGetId<TEntity> idGetter)
            where TEntity : class, IEntry, new()
            where TMapping : CsvMapping<TEntity>
        {
            var csvData = GetCsvData(file);
            var entries = Pool.Provider.List<TEntity>();

            Parse<TEntity, TMapping>(csvData, mapper, entries);
            table.AddRange(entries, idGetter);

            Pool.Provider.Return(entries);
        }

        public void Load<TEntity, TMapping, TIdGetter>(ITable<TEntity> table, TMapping mapper, TextAsset file)
            where TEntity : class, IEntry, new()
            where TMapping : CsvMapping<TEntity>
            where TIdGetter : IGetId<TEntity>, new()
        {
            var csvData = GetCsvData(file);
            var entries = Pool.Provider.List<TEntity>();

            Parse<TEntity, TMapping>(csvData, mapper, entries);
            table.AddRange(entries, new TIdGetter());

            Pool.Provider.Return(entries);
        }

        private void Parse<TEntity, TMapping>(string csvData, TMapping mapper, List<TEntity> output)
            where TEntity : class, new()
            where TMapping : CsvMapping<TEntity>
        {
            if (IsAdvanced<TEntity, TMapping>())
                AdvancedParse(csvData, mapper, output);
            else
                BasicParse(csvData, mapper, output);
        }

        private bool IsAdvanced<TEntity, TMapping>()
            where TEntity : class, new()
            where TMapping : CsvMapping<TEntity>
        {
            var attributes = typeof(TMapping).CustomAttributes;
            return attributes.Any(x => x.AttributeType.IsAssignableFrom(typeof(RowAsColumnAttribute)));
        }

        private void BasicParse<TEntity, TMapping>(string csvData, TMapping mapper, List<TEntity> output)
            where TEntity : class, new()
            where TMapping : CsvMapping<TEntity>
        {
            var parser = new CsvParser<TEntity>(this.parserOptions, mapper);
            var entries = parser.ReadFromString(this.readerOptions, csvData);

            foreach (var entry in entries)
            {
                if (entry.IsValid)
                    output.Add(entry.Result);
            }
        }

        private void AdvancedParse<TEntity, TMapping>(string csvData, TMapping mapper, List<TEntity> output)
            where TEntity : class, new()
            where TMapping : CsvMapping<TEntity>
        {
            var parser = new AdvancedCsvParser<TEntity>(this.advancedParserOptions, mapper);
            var entries = parser.ReadFromString(this.readerOptions, csvData);

            foreach (var entry in entries)
            {
                if (entry.IsValid)
                    output.Add(entry.Result);
            }
        }

        private string GetCsvData(TextAsset file)
        {
            var externalFilePath = this.config.GetExternalCsvFileFullPath($"{file.name}.csv");

            if (!File.Exists(externalFilePath))
            {
                UnuLogger.Log($"Read [built-in]/{file.name}");
                return file.text;
            }

            string data;

            try
            {
                UnuLogger.Log($"Read {externalFilePath}");
                data = File.ReadAllText(externalFilePath);
            }
            catch (Exception ex)
            {
                UnuLogger.LogException(ex);

                UnuLogger.Log($"Read [built-in]/{file.name}");
                data = file.text;
            }

            return data;
        }

        public void Load<TData, TParser>(TData data, TextAsset file, in Segment<string> languages)
            where TData : class
            where TParser : VisualNovelData.Parser.ICsvParser<TData>, new()
        {
            var parser = new TParser();
            parser.Initialize(languages);
            parser.Parse(GetCsvData(file), data);
        }
    }
}
