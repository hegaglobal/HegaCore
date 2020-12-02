namespace HegaCore.AutoGen.Templates
{
    internal static class PlayerDataTemplate
    {
        public const string Template = @"using System;
using HegaCore;

namespace #_NAMESPACE_#
{
    [Serializable]
    public sealed partial class #_TYPE_PREFIX_#PlayerData : PlayerData<#_TYPE_PREFIX_#PlayerData>
    {
    }
}
";
    }
}