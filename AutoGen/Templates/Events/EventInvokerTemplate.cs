namespace HegaCore.AutoGen.Templates
{
    internal static class EventInvokerTemplate
    {
        public const string Template = @"using HegaCore;
using VisualNovelData.Data;

namespace #_NAMESPACE_#
{
    public sealed partial class #_TYPE_PREFIX_#EventInvoker : EventInvoker<#_TYPE_PREFIX_#PlayerData, #_TYPE_PREFIX_#GameData, #_TYPE_PREFIX_#GameDataHandler, #_TYPE_PREFIX_#GameDataContainer>
    {
        public #_TYPE_PREFIX_#EventInvoker(in ReadEventData eventData, #_TYPE_PREFIX_#GameDataContainer dataContainer) : base(eventData, dataContainer)
        {
        }
    }
}
";
    }
}