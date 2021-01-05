using System.IO;
using UnityEditor;
using UnityEngine;

namespace HegaCore.Editor
{
    public static class ScriptableObjectHelper
    {
        public static T Create<T>(bool autoSelection = true, string folderPath = "", string fileName = null) where T : ScriptableObject
        {
            T asset = ScriptableObject.CreateInstance<T>();

            string path;

            if (string.IsNullOrWhiteSpace(folderPath))
            {
                path = AssetDatabase.GetAssetPath(Selection.activeObject);

                if (string.IsNullOrWhiteSpace(path))
                    path = "Assets";
                else if (!string.IsNullOrEmpty(Path.GetExtension(path)))
                    path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), string.Empty);
            }
            else
            {
                path = folderPath.Replace('\\', '/');
            }

            var directoryPath = Path.Combine(Application.dataPath, "..", path);

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            if (string.IsNullOrWhiteSpace(fileName))
                fileName = typeof(T).Name;

            var assetPathAndName = AssetDatabase.GenerateUniqueAssetPath($"{path}/{fileName}.asset");

            AssetDatabase.CreateAsset(asset, assetPathAndName);
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();

            if (autoSelection)
                Selection.activeObject = asset;

            return asset;
        }

        public static T GetOrCreate<T>(bool autoSelection = true, string folderPath = "", string fileName = null) where T : ScriptableObject
        {
            var typeName = typeof(T).Name;
            var guids = AssetDatabase.FindAssets($"t:{typeName}");

            if (guids == null || guids.Length == 0)
            {
                return Create<T>(autoSelection, folderPath, fileName);
            }

            var file = AssetDatabase.GUIDToAssetPath(guids[0]);
            var asset = AssetDatabase.LoadAssetAtPath<T>(file);

            if (autoSelection)
                Selection.activeObject = asset;

            return asset;
        }

        public static bool TryGet<T>(out T asset, bool autoSelection = true) where T : ScriptableObject
        {
            var typeName = typeof(T).Name;
            var guids = AssetDatabase.FindAssets($"t:{typeName}");

            if (guids == null || guids.Length == 0)
                return asset = null;

            var file = AssetDatabase.GUIDToAssetPath(guids[0]);
            asset = AssetDatabase.LoadAssetAtPath<T>(file);

            if (autoSelection)
                Selection.activeObject = asset;

            return asset;
        }
    }
}
