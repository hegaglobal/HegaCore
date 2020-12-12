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

        [MenuItem("Hega/Cubism/Toggle Addressables Support")]
        public static void ToggleAddressableCubismSymbols()
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

        [MenuItem("Hega/Delete User Data File")]
        public static void DeleteUserDataFile()
        {
            var config = ScriptableObjectHelper.GetOrCreate<DatabaseConfig>(false, "Assets/Game/Resources");
            var filePath = config.SaveData.FileFullPathEditor;

            if (Directory.Exists(config.SaveData.FolderFullPathEditor) &&
                File.Exists(filePath))
                File.Delete(filePath);
        }

        [MenuItem("Hega/Toggle Cheat")]
        public static void ToggleCheat()
        {
            var config = ScriptableObjectHelper.GetOrCreate<DatabaseConfig>(false, "Assets/Game/Resources");

            var folderPath = config.ExternalCsvFolderFullPath;
            var filePath = config.DaemonFileFullPath;

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
