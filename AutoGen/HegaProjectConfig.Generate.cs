using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using HegaCore.AutoGen.Templates;

namespace HegaCore.AutoGen
{
    public partial class HegaProjectConfig
    {
        private static class AutoGenKey
        {
            public const string AsmdefRuntime = "#_ASMDEF_RUNTIME_#";
            public const string AsmdefEditor = "#_ASMDEF_EDITOR_#";
            public const string AsmdefRefs = "#_ASMDEF_REFS_#";
            public const string Namespace = "#_NAMESPACE_#";
            public const string TypePrefix = "#_TYPE_PREFIX_#";
            public const string BaseType = "#_BASE_TYPE_#";
        }

        [PropertySpace, FoldoutGroup("Source Code")]
        [Button(ButtonSizes.Large, Name = "Generate Scripts"), GUIColor(0f, 1f, 0f)]
        private void GenerateScripts()
        {
            string databaseBaseType, tableBaseType;

            if (this.codeDatabaseType == DatabaseType.VisualNovel)
            {
                databaseBaseType = "VisualNovelDatabase";
                tableBaseType = "VisualNovelDataTables";
            }
            else
            {
                databaseBaseType = "Database";
                tableBaseType = "Tables";
            }

            var dataHandlerBaseType = this.codeDataHandlerType == GameDataHandlerType.Json
                                      ? "GameDataHandlerJson"
                                      : "GameDataHandlerBinary";

            var projectFolder = MakePath(Application.dataPath, this.ProjectFolder);

            var folderScripts = MakePath(projectFolder, "Scripts");
            var folderScriptsEditor = MakePath(folderScripts, this.projectScriptEditor);
            var folderScriptsRuntime = MakePath(folderScripts, this.projectScriptRuntime);

            var folderRuntimeDatabase = MakePath(folderScriptsRuntime, "Database");
            var folderRuntimeGameData = MakePath(folderScriptsRuntime, "GameData");
            var folderRuntimeGameDataEditor = MakePath(folderRuntimeGameData, "Editor");
            var folderRuntimeEvents = MakePath(folderScriptsRuntime, "Events");
            var folderRuntimeEventsBehaviours = MakePath(folderRuntimeEvents, "Behaviours");

            var folderRuntimeDatabaseGenerated = MakePath(folderRuntimeDatabase, "_Generated");
            var folderRuntimeGameDataGenerated = MakePath(folderRuntimeGameData, "_Generated");
            var folderRuntimeGameDataEditorGenerated = MakePath(folderRuntimeGameDataEditor, "_Generated");
            var folderRuntimeEventsGenerated = MakePath(folderRuntimeEvents, "_Generated");
            var folderRuntimeEventsBehavioursGenerated = MakePath(folderRuntimeEventsBehaviours, "_Generated");

            var fileDatabaseDatabase = MakeCsFilePrefix(folderRuntimeDatabaseGenerated, "Database");
            var fileDatabaseTables = MakeCsFilePrefix(folderRuntimeDatabaseGenerated, "Tables");

            var fileGameDataGameSettings = MakeCsFilePrefix(folderRuntimeGameDataGenerated, "GameSettings");
            var fileGameDataPlayerData = MakeCsFilePrefix(folderRuntimeGameDataGenerated, "PlayerData");
            var fileGameDataGameData = MakeCsFilePrefix(folderRuntimeGameDataGenerated, "GameData");
            var fileGameDataGameDataHandler = MakeCsFilePrefix(folderRuntimeGameDataGenerated, "GameDataHandler");
            var fileGameDataGameDataContainer = MakeCsFilePrefix(folderRuntimeGameDataGenerated, "GameDataContainer");
            var fileGameDataGameDataManager = MakeCsFilePrefix(folderRuntimeGameDataGenerated, "GameDataManager");

            var fileGameDataGameSettingsEditor = MakeCsFilePrefix(folderRuntimeGameDataEditorGenerated, "GameSettingsEditor");
            var fileGameDataPlayerDataEditor = MakeCsFilePrefix(folderRuntimeGameDataEditorGenerated, "PlayerDataEditor");
            var fileGameDataGameDataEditor = MakeCsFilePrefix(folderRuntimeGameDataEditorGenerated, "GameDataEditor");

            var fileEventsEventInvoker = MakeCsFilePrefix(folderRuntimeEventsGenerated, "EventInvoker");
            var fileEventsEventManager = MakeCsFilePrefix(folderRuntimeEventsGenerated, "EventManager");
            var fileEventsDataCommandRegisterer = MakeCsFilePrefix(folderRuntimeEventsBehavioursGenerated, "DataCommandRegisterer");

            var fileAsmdefEditor = MakeAsmdefFile(folderScriptsEditor, this.codeAsmdefEditorName);
            var fileAsmdefRuntime = MakeAsmdefFile(folderScriptsRuntime, this.codeAsmdefRuntimeName);

            CreateFolders(
                folderScriptsEditor,
                folderRuntimeDatabaseGenerated,
                folderRuntimeGameDataGenerated,
                folderRuntimeGameDataEditorGenerated,
                folderRuntimeEventsGenerated,
                folderRuntimeEventsBehavioursGenerated
            );

            switch (this.codeGenerateFileType)
            {
                case GenerateFileType.Database:
                    CreateFiles(
                        (fileDatabaseDatabase, ProcessCsTemplate(DatabaseTemplate.Template, databaseBaseType)),
                        (fileDatabaseTables, ProcessCsTemplate(TableTemplate.Template, tableBaseType))
                    );
                    break;

                case GenerateFileType.GameData:
                    CreateFiles(
                        (fileGameDataGameSettings, ProcessCsTemplate(GameSettingsTemplate.Template)),
                        (fileGameDataPlayerData, ProcessCsTemplate(PlayerDataTemplate.Template)),
                        (fileGameDataGameData, ProcessCsTemplate(GameDataTemplate.Template)),
                        (fileGameDataGameDataHandler, ProcessCsTemplate(GameDataHandlerTemplate.Template, dataHandlerBaseType)),
                        (fileGameDataGameDataContainer, ProcessCsTemplate(GameDataContainerTemplate.Template)),
                        (fileGameDataGameDataManager, ProcessCsTemplate(GameDataManagerTemplate.Template)),

                        (fileGameDataGameSettingsEditor, ProcessCsTemplate(GameSettingsEditorTemplate.Template)),
                        (fileGameDataPlayerDataEditor, ProcessCsTemplate(PlayerDataEditorTemplate.Template)),
                        (fileGameDataGameDataEditor, ProcessCsTemplate(GameDataEditorTemplate.Template))
                    );
                    break;

                case GenerateFileType.EventSystem:
                    CreateFiles(
                        (fileEventsEventInvoker, ProcessCsTemplate(EventInvokerTemplate.Template)),
                        (fileEventsEventManager, ProcessCsTemplate(EventManagerTemplate.Template)),
                        (fileEventsDataCommandRegisterer, ProcessCsTemplate(DataCommandRegistererTemplate.Template))
                    );
                    break;

                case GenerateFileType.Asmdef:
                    CreateFiles(
                        (fileAsmdefEditor, ProcessAsmdefTemplate(AsmdefEditorTemplate.Template)),
                        (fileAsmdefRuntime, ProcessAsmdefTemplate(AsmdefRuntimeTemplate.Template))
                    );
                    break;

                default:
                    CreateFiles(
                        (fileDatabaseDatabase, ProcessCsTemplate(DatabaseTemplate.Template, databaseBaseType)),
                        (fileDatabaseTables, ProcessCsTemplate(TableTemplate.Template, tableBaseType)),

                        (fileGameDataGameSettings, ProcessCsTemplate(GameSettingsTemplate.Template)),
                        (fileGameDataPlayerData, ProcessCsTemplate(PlayerDataTemplate.Template)),
                        (fileGameDataGameData, ProcessCsTemplate(GameDataTemplate.Template)),
                        (fileGameDataGameDataHandler, ProcessCsTemplate(GameDataHandlerTemplate.Template, dataHandlerBaseType)),
                        (fileGameDataGameDataContainer, ProcessCsTemplate(GameDataContainerTemplate.Template)),
                        (fileGameDataGameDataManager, ProcessCsTemplate(GameDataManagerTemplate.Template)),

                        (fileGameDataGameSettingsEditor, ProcessCsTemplate(GameSettingsEditorTemplate.Template)),
                        (fileGameDataPlayerDataEditor, ProcessCsTemplate(PlayerDataEditorTemplate.Template)),
                        (fileGameDataGameDataEditor, ProcessCsTemplate(GameDataEditorTemplate.Template)),

                        (fileEventsEventInvoker, ProcessCsTemplate(EventInvokerTemplate.Template)),
                        (fileEventsEventManager, ProcessCsTemplate(EventManagerTemplate.Template)),
                        (fileEventsDataCommandRegisterer, ProcessCsTemplate(DataCommandRegistererTemplate.Template)),

                        (fileAsmdefEditor, ProcessAsmdefTemplate(AsmdefEditorTemplate.Template)),
                        (fileAsmdefRuntime, ProcessAsmdefTemplate(AsmdefRuntimeTemplate.Template))
                    );
                    break;
            }

            AssetDatabase.Refresh();
        }

        private string ProcessAsmdefTemplate(string template, string customRefs = "")
            => template.Replace(AutoGenKey.AsmdefEditor, this.codeAsmdefEditorName)
                       .Replace(AutoGenKey.AsmdefRuntime, this.codeAsmdefRuntimeName)
                       .Replace(AutoGenKey.AsmdefRefs, customRefs ?? string.Empty);

        private string ProcessCsTemplate(string template)
            => template.Replace(AutoGenKey.Namespace, this.codeNamespace)
                       .Replace(AutoGenKey.TypePrefix, this.codeTypePrefix);

        private string ProcessCsTemplate(string template, string baseType)
            => ProcessCsTemplate(template).Replace(AutoGenKey.BaseType, baseType);

        private string MakeCsFilePrefix(string path, string name)
            => MakeCsFile(path, this.codeTypePrefix, name);
    }
}