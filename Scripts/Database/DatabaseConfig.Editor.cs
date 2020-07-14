#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;

namespace HegaCore
{
    public sealed partial class DatabaseConfig
    {
        [Button("Find All"), BoxGroup("Csv Files")]
        private void FindAllCsvFiles()
        {
            this.internalCsvPath = $"Assets/{this.internalCsvFolder}";

            if (!AssetDatabase.IsValidFolder(this.internalCsvPath))
                return;

            var guids = AssetDatabase.FindAssets($"t:{nameof(TextAsset)}", new[] { this.internalCsvPath });

            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
                this.csvFiles[asset.name] = asset;
            }

            Undo.RecordObject(this, "Changed Csv Files");
        }
    }
}

#endif