namespace HegaCore.AutoGen.Templates
{
    internal static class PlayerDataEditorTemplate
    {
        public const string Template = @"#if UNITY_EDITOR

using HegaCore.Editor;

namespace #_NAMESPACE_#.Editor
{
    public sealed partial class #_TYPE_PREFIX_#PlayerDataEditor : PlayerDataEditor<#_TYPE_PREFIX_#PlayerData>
    {
    }
}

#endif
";
    }
}