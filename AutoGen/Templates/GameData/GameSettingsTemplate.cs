namespace HegaCore.AutoGen.Templates
{
    internal static class GameSettingsTemplate
    {
        public const string Template = @"using System;
using HegaCore;

namespace #_NAMESPACE_#
{
    [Serializable]
    public sealed partial class #_TYPE_PREFIX_#GameSettings : GameSettings<#_TYPE_PREFIX_#GameSettings>
    {
    }
}
";
    }
}