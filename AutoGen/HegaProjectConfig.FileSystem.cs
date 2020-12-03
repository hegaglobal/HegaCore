using System.IO;
using UnityEngine;
using HegaCore.Editor;

namespace HegaCore.AutoGen
{
    public partial class HegaProjectConfig
    {
        private static class FileName
        {
            public const string Keep = ".keep";
        }

        private static string MakePath(string path1, string path2)
            => Path.Combine(path1, path2).Replace("\\", "/");

        private static string MakePath(string path1, string path2, string path3)
            => Path.Combine(path1, path2, path3).Replace("\\", "/");

        private static string MakePath(string path1, string path2, string path3, string path4)
            => Path.Combine(path1, path2, path3, path4).Replace("\\", "/");

        private static string MakePath(params string[] paths)
            => Path.Combine(paths).Replace("\\", "/");

        private static string MakeKeepFile(string path)
            => MakePath(path, FileName.Keep);

        private static string MakeFile(string path, string fileName)
            => MakePath(path, fileName);

        private static string MakeCsFile(string path, string prefix, string name)
            => MakePath(path, $"{prefix}{name}.cs");

        private static string MakeAsmdefFile(string path, string name)
            => MakePath(path, $"{name}.asmdef");

        private static void CreateFolders(params string[] paths)
        {
            foreach (var path in paths)
            {
                if (Directory.Exists(path))
                    continue;

                Directory.CreateDirectory(path);
                UnuLogger.Log($"Create Folder: {path}");
            }
        }

        private static void CreateFiles(params string[] paths)
        {
            foreach (var path in paths)
            {
                if (File.Exists(path))
                    continue;

                File.WriteAllText(path, string.Empty);
                UnuLogger.Log($"Create File: {path}");
            }
        }

        private static void CreateFiles(params (string path, string content)[] files)
        {
            foreach (var (path, content) in files)
            {
                File.WriteAllText(path, content);
                UnuLogger.Log($"Create File: {path}");
            }
        }

        private static void CopyPrefabs(string sourcePath, string destPath, bool overwrite = false)
        {
            if (!Directory.Exists(sourcePath))
                return;

            CreateFolders(destPath);

            var files = Directory.GetFiles(sourcePath, "*.prefab");

            foreach (var sourceFile in files)
            {
                var fileName = sourceFile.Replace(sourcePath, "") .Replace("\\", "") .Replace("/", "");
                var destFile = MakePath(destPath, fileName);

                if (File.Exists(destFile) && !overwrite)
                    continue;

                File.Copy(sourceFile, destFile, overwrite);
                UnuLogger.Log($"Copy File: [FROM] {sourceFile}\n[TO] {destFile}");
            }
        }

        private static void CreateScriptableFile<T>(string folderPath, string fileName = null,
                                                    bool ignoreExisting = false)
            where T : ScriptableObject
        {
            CreateFolders(folderPath);

            if (string.IsNullOrWhiteSpace(fileName))
                fileName = typeof(T).Name;

            var filePath = MakePath(folderPath, $"{fileName}.asset");

            if (File.Exists(filePath) && !ignoreExisting)
                return;

            ScriptableObjectHelper.Create<T>(false, folderPath);
            UnuLogger.Log($"Create File: {filePath}");
        }
    }
}