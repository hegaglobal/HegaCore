#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;

namespace HegaCore
{
    public abstract partial class SingletonDatabase<T>
    {
        [Button("Find All"), BoxGroup("Csv Files")]
        private void FindAllCsvFiles()
        {
            if (!AssetDatabase.IsValidFolder(this.internalPath))
                return;

            var guids = AssetDatabase.FindAssets($"t:{nameof(TextAsset)}", new[] { this.internalPath });

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