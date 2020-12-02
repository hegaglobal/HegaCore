using UnityEditor;
using UnityEngine;
using HegaCore.Editor;
using Sirenix.OdinInspector;
using UnuGames;

namespace HegaCore.AutoGen
{
    public partial class HegaProjectConfig
    {
        private static class AutoGenKey
        {
            public const string ProjectAsmdef = "#_PROJECT_ASMDEF_#";
            public const string Namespace = "#_NAMESPACE_#";
            public const string TypePrefix = "#_TYPE_PREFIX_#";
            public const string BaseType = "#_BASE_TYPE_#";
        }

        [PropertySpace, TitleGroup("Source Code")]
        [Button(ButtonSizes.Large, Name = "Generate")]
        private void Generate()
        {
        }
    }
}