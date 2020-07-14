using System;
using System.Collections.Generic;
using System.IO;
using System.Table;
using UnityEngine;
using TinyCsvParser;
using TinyCsvParser.Mapping;
using TinyCsvParser.Tokenizer.RFC4180;

namespace HegaCore
{
    public sealed class CsvDataLoader
    {
        private readonly CsvParserOptions parserOptions;
        private readonly CsvReaderOptions readerOptions;

        private string externalPath;

        public CsvDataLoader()
        {
            var options = new Options('"', '\\', ',');
            var tokenizer = new RFC4180Tokenizer(options);
            this.parserOptions = new CsvParserOptions(true, "//", tokenizer);
            this.readerOptions = new CsvReaderOptions(new[] { "\r\n", "\n" });
        }

        public void SetExternalPath(string value)
            => this.externalPath = value;

        public void Load<TEntity, TMapping>(ITable<TEntity> table, TextAsset internalFile, bool autoIncrement = false)
            where TEntity : class, IEntry, new()
            where TMapping : CsvMapping<TEntity>, new()
        {
            var csvData = GetCsvData(internalFile);
            var rawData = Parse<TEntity, TMapping>(csvData);
            table.AddRange(rawData, autoIncrement);
        }

        public void Load<TEntity, TMapping>(ITable<TEntity> table, TextAsset internalFile, IGetId<TEntity> idGetter)
            where TEntity : class, IEntry, new()
            where TMapping : CsvMapping<TEntity>, new()
        {
            var csvData = GetCsvData(internalFile);
            var rawData = Parse<TEntity, TMapping>(csvData);
            table.AddRange(rawData, idGetter);
        }

        public void Load<TEntity, TMapping, TIdGetter>(ITable<TEntity> table, TextAsset internalFile)
            where TEntity : class, IEntry, new()
            where TMapping : CsvMapping<TEntity>, new()
            where TIdGetter : IGetId<TEntity>, new()
        {
            var csvData = GetCsvData(internalFile);
            var rawData = Parse<TEntity, TMapping>(csvData);
            table.AddRange(rawData, new TIdGetter());
        }

        public bool Daemon(string daemon)
        {
            var filePath = Path.Combine(this.externalPath, daemon);
            return File.Exists(filePath);
        }

        private IList<TEntity> Parse<TEntity, TMapping>(string csvData)
            where TEntity : class, new()
            where TMapping : CsvMapping<TEntity>, new()
        {
            var mapper = new TMapping();
            var parser = new CsvParser<TEntity>(this.parserOptions, mapper);
            var entries = parser.ReadFromString(this.readerOptions, csvData);
            var result = new List<TEntity>();

            foreach (var entry in entries)
            {
                if (entry.IsValid)
                    result.Add(entry.Result);
            }

            return result;
        }

        private string GetCsvData(TextAsset internalFile)
        {
            var externalFilePath = Path.Combine(this.externalPath, $"{internalFile.name}.csv");

            if (!File.Exists(externalFilePath))
            {
                UnuLogger.Log($"Read [built-in]/{internalFile.name}");
                return internalFile.text;
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

                UnuLogger.Log($"Read [built-in]/{internalFile.name}");
                data = internalFile.text;
            }

            return data;
        }

        public void Load<TData, TParser>(TData data, TextAsset internalFile, in Segment<string> languages)
            where TData : class
            where TParser : VisualNovelData.Parser.ICsvParser<TData>, new()
        {
            var parser = new TParser();
            parser.Initialize(languages);
            parser.Parse(GetCsvData(internalFile), data);
        }
    }
}
