﻿/// <summary>
/// The script gives you choice to whether to build addressable bundles when clicking the build button.
/// For custom build script, call PreExport method yourself.
/// For cloud build, put BuildAddressablesProcessor.PreExport as PreExport command.
/// Discussion: https://forum.unity.com/threads/how-to-trigger-build-player-content-when-build-unity-project.689602/
/// </summary>
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace HegaCore.Editor
{
    public sealed class BuildAddressablesProcessor
    {
        /// <summary>
        /// Run a clean build before export.
        /// </summary>
        public static void PreExport()
        {
            Debug.Log($"{nameof(BuildAddressablesProcessor)}.{nameof(PreExport)} start");

            AddressableImporter.FolderImporter.ReimportFolders(new string[] { "Assets" });
            AddressableAssetSettings.CleanPlayerContent(AddressableAssetSettingsDefaultObject.Settings.ActivePlayerDataBuilder);
            AddressableAssetSettings.BuildPlayerContent();

            Debug.Log($"{nameof(BuildAddressablesProcessor)}.{nameof(PreExport)} done");
        }

        [InitializeOnLoadMethod]
        internal static void Initialize()
        {
            BuildPlayerWindow.RegisterBuildPlayerHandler(BuildPlayerHandler);
        }

        private static void BuildPlayerHandler(BuildPlayerOptions options)
        {
            if (EditorUtility.DisplayDialog("Build with Addressables",
                "Do you want to build a clean addressables before export?",
                "Build with Addressables", "Skip"))
            {
                PreExport();
            }

            BuildPlayerWindow.DefaultBuildMethods.BuildPlayer(options);
        }

    }
}