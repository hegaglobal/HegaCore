namespace HegaCore.AutoGen.Templates
{
    internal static class GameDataManagerTemplate
    {
        public const string Template = @"using HegaCore;

namespace #_NAMESPACE_#
{
    public sealed partial class #_TYPE_PREFIX_#GameDataManager : GameDataManager<#_TYPE_PREFIX_#PlayerData, #_TYPE_PREFIX_#GameSettings, #_TYPE_PREFIX_#GameData, #_TYPE_PREFIX_#GameDataHandler, #_TYPE_PREFIX_#GameDataContainer, #_TYPE_PREFIX_#GameDataManager>
    {
    }
}
";
    }
}