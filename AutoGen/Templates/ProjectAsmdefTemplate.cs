namespace HegaCore.AutoGen.Templates
{
    internal static class ProjectAsmdefTemplate
    {
        public const string Template = @"{
    ""name"": ""#_PROJECT_ASMDEF_#"",
    ""references"": [
        ""DOTween.Modules"",
        ""Live2D.Cubism"",
        ""TinyCsvParser"",
        ""UIMan"",
        ""UIMan.TextMeshPro"",
        ""UniTask"",
        ""UniTask.Addressables"",
        ""UniTask.DOTween"",
        ""UniTask.Linq"",
        ""UniTask.TextMeshPro"",
        ""Unity.Addressables"",
        ""Unity.AddressablesManager"",
        ""Unity.ObjectPooling"",
        ""Unity.ObjectPooling.Addressables"",
        ""Unity.ResourceManager"",
        ""Unity.Supplements"",
        ""Unity.QuaStateMachine"",
        ""Unity.TextMeshPro"",
        ""UnityEngine.UI"",
        ""Unity.GoogleSpreadsheetDownloader""
        ""VisualNovelData"",
        ""HegaCore"",
        ""RedBlueGames.TextTyper"",
        ""HegaCore.Steamworks""
    ],
    ""includePlatforms"": [],
    ""excludePlatforms"": [],
    ""allowUnsafeCode"": false,
    ""overrideReferences"": false,
    ""precompiledReferences"": [],
    ""autoReferenced"": true,
    ""defineConstraints"": [],
    ""versionDefines"": [],
    ""noEngineReferences"": false
}
";
    }
}