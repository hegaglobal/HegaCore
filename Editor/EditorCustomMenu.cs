using System.Linq;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace HegaCore.Editor
{
    public sealed class EditorCustomMenu : MonoBehaviour
    {
        private static readonly string[] _addressableCubismSymbols = new[] {
            "CUBISM_LOADER",
            "CUBISM_USE_MUTUAL_TEXTURE"
        };

        [MenuItem("Tools/Toggle Addressable Cubism")]
        public static void ToggleAddressableCubism()
        {
            var symbolString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            var symbols = symbolString.Split(';').ToList();

            foreach (var s in _addressableCubismSymbols)
            {
                if (symbols.Contains(s))
                    symbols.Remove(s);
                else
                    symbols.Add(s);
            }

            PlayerSettings.SetScriptingDefineSymbolsForGroup(
                EditorUserBuildSettings.selectedBuildTargetGroup,
                string.Join(";", symbols.ToArray())
            );
        }

        [MenuItem("Tools/Delete User Data File")]
        public static void DeleteUserDataFile()
        {
            var config = ScriptableObjectHelper.GetOrCreate<DatabaseConfig>(false, "Assets/Game/Resources");

            var parentPath = Directory.GetParent(Application.dataPath).FullName;
            var folderPath = Path.Combine(parentPath, config.SaveDataEditorFolder);
            var filePath = Path.Combine(folderPath, config.SaveDataFile);

            if (Directory.Exists(folderPath) && File.Exists(filePath))
                File.Delete(filePath);
        }

        [MenuItem("Tools/Toggle Cheat")]
        public static void ToggleCheat()
        {
            var config = ScriptableObjectHelper.GetOrCreate<DatabaseConfig>(false, "Assets/Game/Resources");

            var parentPath = Directory.GetParent(Application.dataPath).FullName;
            var folderPath = Path.Combine(parentPath, config.ExternalCsvFolder);
            var filePath = Path.Combine(folderPath, config.DaemonFile);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Debug.Log("CHEAT: OFF");
            }
            else
            {
                File.Create(filePath).Dispose();
                Debug.Log("CHEAT: ON");
            }
        }
    }
}
