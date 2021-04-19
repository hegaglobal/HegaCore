using System.Linq;
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

        [PropertySpace, FoldoutGroup("Folders")]
        [Button(ButtonSizes.Large, Name = "Init New Project"), GUIColor(0f, 1f, 0f)]
        private void InitializeNewProject()
        {
            AddDefineSymbols();

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
            var projectFolderAddressablesUIBackgrounds = MakePath(projectFolderAddressablesUI, "Backgrounds");

            // Addressables.Systems
            var projectFolderAddressablesSystems = MakePath(projectFolderAddressables, "Systems");
            var projectFolderAddressablesSystemsProject = MakePath(projectFolderAddressablesSystems, this.ProjectSystemsFolder);

            // Addressables.UI - Keeps
            var projectFileAddressablesUIScreensKeep = MakeKeepFile(projectFolderAddressablesUIScreens);
            var projectFileAddressablesUIBackgroundsKeep = MakeKeepFile(projectFolderAddressablesUIBackgrounds);

            // Keeps
            var projectFileAnimationsKeep = MakeKeepFile(projectFolderAnimations);
            var projectFileDatabaseKeep = MakeKeepFile(projectFolderDatabase);
            var projectFileScenesKeep = MakeKeepFile(projectFolderScenes);
            var projectFileScriptsKeep = MakeKeepFile(projectFolderScripts);

            CreateFolders(
                projectFolderAddressablesUIScreens,
                projectFolderAddressablesUIBackgrounds,
                projectFolderAnimations,
                projectFolderDatabase,
                projectFolderScenes,
                projectFolderScripts
            );

            CreateFiles(
                projectFileAddressablesUIScreensKeep,
                projectFileAddressablesUIBackgroundsKeep,
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
            CreateScriptableFile<DatabaseConfig>(assetProjectResources);

            AssetDatabase.Refresh();

            ConfigUIMan();
            ConfigDatabase();
        }

        private void ConfigUIMan()
        {
            if (!ScriptableObjectHelper.TryGet<UIManConfig>(out var config, false))
                return;

            config.screenPrefabFolder = MakePath(this.ProjectFolder, "Addressables/UI/Screens");
            config.dialogPrefabFolder = MakePath(this.ProjectFolder, "Addressables/UI/Dialogs");
            config.activityPrefabFolder = MakePath(this.ProjectFolder, "Addressables/UI/Activities");
            config.backgroundRootFolder = MakePath(this.ProjectFolder, "Addressables/UI/Backgrounds");
            config.animRootFolder = MakePath(this.ProjectFolder, "Game/Animations");
        }

        private void ConfigDatabase()
        {
            if (!ScriptableObjectHelper.TryGet<DatabaseConfig>(out var config, false))
                return;

            config.InternalCsvFolder = MakePath(this.ProjectFolder, "Database");
            config.ExternalCsvFolder = "../External_Database";
        }

        private void AddDefineSymbols()
        {
            var buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            var definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            var allDefines = definesString.Split(';').ToList();

            allDefines.AddRange(this.scriptingDefineSymbols.Except(allDefines));

            PlayerSettings.SetScriptingDefineSymbolsForGroup(
                buildTargetGroup,
                string.Join(";", allDefines.ToArray())
            );
        }
    }
}