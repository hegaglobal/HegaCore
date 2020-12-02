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
        [SerializeField, LabelText("Project Systems")]
        private string projectSystemsFolder = "Game";

        [TitleGroup("Names")]
        [SerializeField, LabelText("Project")]
        private string projectName = "Game";

        [TitleGroup("Source Code")]
        [SerializeField, LabelText("ASMDEF")]
        private string codeAsmdefName = "Game";

        [TitleGroup("Source Code")]
        [SerializeField, LabelText("Namespace")]
        private string codeNamespace = "Game";

        [TitleGroup("Source Code")]
        [SerializeField, LabelText("Type Prefix")]
        private string codeTypePrefix = "Game";

        [TitleGroup("Source Code")]
        [SerializeField, LabelText("Database Type"), EnumToggleButtons]
        private DatabaseType codeDatabaseType = DatabaseType.Base;

        public string ProjectFolder => this.projectFolder.Replace('\\', '/');

        public string HegaCoreFolder => this.hegaCoreFolder.Replace('\\', '/');

        public string ProjectSystemsFolder => this.projectSystemsFolder.Replace('\\', '/');

        public enum DatabaseType
        {
            Base, VisualNovel
        }
    }
}
