using UnityEditor;
using UnityEngine;
using HegaCore.Editor;
using Sirenix.OdinInspector;
using UnuGames;

namespace HegaCore.AutoGen
{
    public partial class HegaProjectConfig
    {
        private const string AutoGenFolder = "Assets/AutoGen";

        [MenuItem("Hega/Init New Project", priority = 0)]
        private static void InitNewProject()
        {
            if (!ScriptableObjectHelper.TryGet<HegaProjectConfig>(out _))
                ScriptableObjectHelper.Create<HegaProjectConfig>(true, AutoGenFolder);
        }

        [PropertySpace, TitleGroup("Folders")]
        [Button(ButtonSizes.Large, Name = "Init New Project")]
        private void InitializeNewProject()
        {
            var projectFolder = MakePath(Application.dataPath, this.ProjectFolder);
            var hegaFolder = MakePath(Application.dataPath, this.HegaCoreFolder);

            var assetProjectResources = MakePath("Assets", this.ProjectFolder, "Resources");

            var projectFolderAddressables = MakePath(projectFolder, "Addressables");
            var projectFolderAnimations = MakePath(projectFolder, "Animations");
            var projectFolderDatabase = MakePath(projectFolder, "Database");
            var projectFolderResources = MakePath(projectFolder, "Resources");
            var projectFolderScenes = MakePath(projectFolder, "Scenes");
            var projectFolderScripts = MakePath(projectFolder, "Scripts");

            // Addressables.UI
            var projectFolderAddressablesUI = MakePath(projectFolderAddressables, "UI");
            var projectFolderAddressablesUIActivities = MakePath(projectFolderAddressablesUI, "Activities");
            var projectFolderAddressablesUIDialogs = MakePath(projectFolderAddressablesUI, "Dialogs");
            var projectFolderAddressablesUIScreens = MakePath(projectFolderAddressablesUI, "Screens");

            // Addressables.Systems
            var projectFolderAddressablesSystems = MakePath(projectFolderAddressables, "Systems");
            var projectFolderAddressablesSystemsProject = MakePath(projectFolderAddressablesSystems, this.ProjectSystemsFolder);

            // Addressables.UI - Keeps
            var projectFileAddressablesUIScreensKeep = MakeKeepFile(projectFolderAddressablesUIScreens);

            // Keeps
            var projectFileAnimationsKeep = MakeKeepFile(projectFolderAnimations);
            var projectFileDatabaseKeep = MakeKeepFile(projectFolderDatabase);
            var projectFileScenesKeep = MakeKeepFile(projectFolderScenes);
            var projectFileScriptsKeep = MakeKeepFile(projectFolderScripts);

            CreateFolders(
                projectFolderAddressablesUIScreens,
                projectFolderAnimations,
                projectFolderDatabase,
                projectFolderScenes,
                projectFolderScripts
            );

            CreateFiles(
                projectFileAddressablesUIScreensKeep,
                projectFileAnimationsKeep,
                projectFileDatabaseKeep,
                projectFileScenesKeep,
                projectFileScriptsKeep
            );

            // HegaCore
            var hegaFolderPrefabs = MakePath(hegaFolder, "Prefabs");
            var hegaFolderPrefabsSystem = MakePath(hegaFolderPrefabs, "System");
            var hegaFolderPrefabsUIMan = MakePath(hegaFolderPrefabs, "UIMan");
            var hegaFolderPrefabsUIManResources = MakePath(hegaFolderPrefabsUIMan, "[TO_COPY]Resources");
            var hegaFolderPrefabsUIManActivities = MakePath(hegaFolderPrefabsUIMan, "Activities");
            var hegaFolderPrefabsUIManDialogs = MakePath(hegaFolderPrefabsUIMan, "Dialogs");

            CopyPrefabs(hegaFolderPrefabsSystem, projectFolderAddressablesSystemsProject);
            CopyPrefabs(hegaFolderPrefabsUIManResources, projectFolderResources);
            CopyPrefabs(hegaFolderPrefabsUIManActivities, projectFolderAddressablesUIActivities);
            CopyPrefabs(hegaFolderPrefabsUIManDialogs, projectFolderAddressablesUIDialogs);

            CreateScriptableFile<UIManConfig>(assetProjectResources);

            AssetDatabase.Refresh();
        }
    }
}