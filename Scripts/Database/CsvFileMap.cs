using System;
using UnityEngine;

namespace HegaCore.Database
{
    [Serializable]
    public sealed class CsvFileMap : SerializableDictionary<string, TextAsset> { }
}