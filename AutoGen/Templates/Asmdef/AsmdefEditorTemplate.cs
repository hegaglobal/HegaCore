namespace HegaCore.AutoGen.Templates
{
    internal static class AsmdefEditorTemplate
    {
        public const string Template = @"{
    ""name"": ""#_ASMDEF_EDITOR_#"",
    ""references"": [
        ""#_ASMDEF_RUNTIME_#"",
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
        ""Unity.GoogleSpreadsheetDownloader"",
        ""VisualNovelData"",
        ""HegaCore"",
        ""RedBlueGames.TextTyper"",
        ""HegaCore.Steamworks""#_ASMDEF_REFS_#
    ],
    ""includePlatforms"": [
        ""Editor""
    ],
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