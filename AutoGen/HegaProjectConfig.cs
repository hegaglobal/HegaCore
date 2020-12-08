using UnityEngine;
using Sirenix.OdinInspector;

namespace HegaCore.AutoGen
{
    [CreateAssetMenu(fileName = nameof(HegaProjectConfig), menuName = "Hega Project Config")]
    public partial class HegaProjectConfig : ScriptableObject
    {
        [TitleGroup("Folders")]
        [SerializeField, LabelText("Project"), InfoBox("@\"Assets/\" + $value.Replace('\\\\','/')")]
        private string projectFolder = "Game";

        [Space]
        [TitleGroup("Folders")]
        [SerializeField, LabelText("Hega Core"), InfoBox("@\"Assets/\" + $value.Replace('\\\\','/')")]
        private string hegaCoreFolder = "HegaCore";

        [Space]
        [TitleGroup("Folders")]
        [InfoBox("@\"Assets/\" + projectFolder + \"/Addressables/Systems/\" + $value.Replace('\\\\','/')")]
        [SerializeField, LabelText("Addressables Systems")]
        private string projectAddressablesSystemsFolder = "Game";

        [Space]
        [TitleGroup("Folders")]
        [InfoBox("@\"Assets/\" + projectFolder + \"/\" + $value.Replace('\\\\','/')")]
        [SerializeField, LabelText("Editor Scripts")]
        private string projectScriptEditor = "Editor";

        [Space]
        [TitleGroup("Folders")]
        [InfoBox("@\"Assets/\" + projectFolder + \"/\" + $value.Replace('\\\\','/')")]
        [SerializeField, LabelText("Runtime Scripts")]
        private string projectScriptRuntime = "Runtime";

        [TitleGroup("Source Code")]
        [SerializeField, LabelText("ASMDEF Editor")]
        private string codeAsmdefEditorName = "Game.Editor";

        [TitleGroup("Source Code")]
        [SerializeField, LabelText("ASMDEF Runtime")]
        private string codeAsmdefRuntimeName = "Game.Runtime";

        [TitleGroup("Source Code")]
        [SerializeField, LabelText("Namespace")]
        private string codeNamespace = "Game";

        [TitleGroup("Source Code")]
        [SerializeField, LabelText("Type Prefix")]
        private string codeTypePrefix = "Game";

        [TitleGroup("Source Code")]
        [SerializeField, LabelText("Database Type"), EnumToggleButtons]
        private DatabaseType codeDatabaseType = DatabaseType.Base;

        [TitleGroup("Source Code")]
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
