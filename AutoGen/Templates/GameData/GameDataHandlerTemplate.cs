namespace HegaCore.AutoGen.Templates
{
    internal static class GameDataHandlerTemplate
    {
        public const string Template = @"using System;
using HegaCore;

namespace #_NAMESPACE_#
{
    [Serializable]
    public sealed partial class #_TYPE_PREFIX_#GameDataHandler : #_BASE_TYPE_#<#_TYPE_PREFIX_#PlayerData, #_TYPE_PREFIX_#GameSettings, #_TYPE_PREFIX_#GameData>
    {
        public override #_TYPE_PREFIX_#GameData New(bool corrupted = false)
            => new #_TYPE_PREFIX_#GameData(corrupted);
    }
}
";
    }
}