using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditorInternal;

namespace HegaCore.AutoGen
{
    [CreateAssetMenu(fileName = nameof(HegaProjectConfig), menuName = "Hega Project Config")]
    public partial class HegaProjectConfig : ScriptableObject
    {
        [FoldoutGroup("Folders")]
        [SerializeField, LabelText("Project"), InfoBox("@\"Assets/\" + $value.Replace('\\\\','/')")]
        private string projectFolder = "Game";

        [Space]
        [FoldoutGroup("Folders")]
        [SerializeField, LabelText("Hega Core"), InfoBox("@\"Assets/\" + $value.Replace('\\\\','/')")]
        private string hegaCoreFolder = "HegaCore";

        [Space]
        [FoldoutGroup("Folders")]
        [InfoBox("@\"Assets/\" + projectFolder + \"/Addressables/Systems/\" + $value.Replace('\\\\','/')")]
        [SerializeField, LabelText("Addressables Systems")]
        private string projectAddressablesSystemsFolder = "Game";

        [Space]
        [FoldoutGroup("Folders")]
        [InfoBox("@\"Assets/\" + projectFolder + \"/\" + $value.Replace('\\\\','/')")]
        [SerializeField, LabelText("Editor Scripts")]
        private string projectScriptEditor = "Editor";

        [Space]
        [FoldoutGroup("Folders")]
        [InfoBox("@\"Assets/\" + projectFolder + \"/\" + $value.Replace('\\\\','/')")]
        [SerializeField, LabelText("Runtime Scripts")]
        private string projectScriptRuntime = "Runtime";

        [Space]
        [FoldoutGroup("Scripting Define Symbols")]
        [SerializeField, HideLabel]
        private string[] scriptingDefineSymbols = new[] {
            "CUBISM_LOADER",
            "CUBISM_USE_MUTUAL_TEXTURE",
            "UNITASK_DOTWEEN_SUPPORT"
        };

        [Space]
        [FoldoutGroup("Assembly Definitions")]
        [SerializeField, LabelText("Editor")]
        private AssemblyDefinitionAsset[] asmdefEditor = null;

        [Space]
        [FoldoutGroup("Assembly Definitions")]
        [SerializeField, LabelText("Runtime")]
        private AssemblyDefinitionAsset[] asmdefRuntime = null;

        [FoldoutGroup("Source Code")]
        [SerializeField, LabelText("ASMDEF Editor")]
        private string codeAsmdefEditorName = "Game.Editor";

        [FoldoutGroup("Source Code")]
        [SerializeField, LabelText("ASMDEF Runtime")]
        private string codeAsmdefRuntimeName = "Game.Runtime";

        [FoldoutGroup("Source Code")]
        [SerializeField, LabelText("Namespace")]
        private string codeNamespace = "Game";

        [FoldoutGroup("Source Code")]
        [SerializeField, LabelText("Type Prefix")]
        private string codeTypePrefix = "Game";

        [FoldoutGroup("Source Code")]
        [SerializeField, LabelText("Database Type"), EnumToggleButtons]
        private DatabaseType codeDatabaseType = DatabaseType.Base;

        [FoldoutGroup("Source Code")]
        [SerializeField, LabelText("Game Data Save Type"), EnumToggleButtons]
        private GameDataHandlerType codeDataHandlerType = GameDataHandlerType.Binary;

        public string ProjectFolder => this.projectFolder.Replace('\\', '/');

        public string HegaCoreFolder => this.hegaCoreFolder.Replace('\\', '/');

        public string ProjectSystemsFolder => this.projectAddressablesSystemsFolder.Replace('\\', '/');

        public enum DatabaseType
        {
            Base, VisualNovel
        }

        public enum GameDataHandlerType
        {
            Binary, Json
        }
    }
}
