﻿namespace HegaCore.AutoGen.Templates
{
    internal static class GameDataEditorTemplate
    {
        public const string Template = @"#if UNITY_EDITOR

using HegaCore.Editor;

namespace #_NAMESPACE_#.Editor
{
    public sealed partial class #_TYPE_PREFIX_#GameDataEditor : GameDataEditor<#_TYPE_PREFIX_#PlayerData, #_TYPE_PREFIX_#GameSettings, #_TYPE_PREFIX_#GameData, #_TYPE_PREFIX_#GameDataHandler, #_TYPE_PREFIX_#PlayerDataEditor, #_TYPE_PREFIX_#GameSettingsEditor>
    {
    }
}

#endif
";
    }
}