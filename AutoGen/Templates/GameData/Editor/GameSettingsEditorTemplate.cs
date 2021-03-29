namespace HegaCore.AutoGen.Templates
{
    internal static class GameSettingsEditorTemplate
    {
        public const string Template = @"#if UNITY_EDITOR

using HegaCore.Editor;

namespace #_NAMESPACE_#.Editor
{
    public sealed partial class #_TYPE_PREFIX_#GameSettingsEditor : GameSettingsEditor<#_TYPE_PREFIX_#GameSettings>
    {
    }
}

#endif
";
    }
}