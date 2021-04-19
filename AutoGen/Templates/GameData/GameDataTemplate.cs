namespace HegaCore.AutoGen.Templates
{
    internal static class GameDataTemplate
    {
        public const string Template = @"using System;
using HegaCore;

namespace #_NAMESPACE_#
{
    [Serializable]
    public sealed partial class #_TYPE_PREFIX_#GameData : GameData<#_TYPE_PREFIX_#PlayerData, #_TYPE_PREFIX_#GameSettings>
    {
        public #_TYPE_PREFIX_#GameData() : base() { }

        public #_TYPE_PREFIX_#GameData(bool corrupted) : base(corrupted) { }
    }
}
";
    }
}