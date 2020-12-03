namespace HegaCore.AutoGen.Templates
{
    internal static class EventManagerTemplate
    {
        public const string Template = @"using System.Collections.Generic;
using VisualNovelData.Data;
using HegaCore;

namespace #_NAMESPACE_#
{
    using Events.Commands;

    public sealed partial class #_TYPE_PREFIX_#EventManager : EventManager<#_TYPE_PREFIX_#PlayerData, #_TYPE_PREFIX_#GameData, #_TYPE_PREFIX_#GameDataHandler, #_TYPE_PREFIX_#GameDataContainer, #_TYPE_PREFIX_#EventInvoker, #_TYPE_PREFIX_#EventManager>
    {
        public override void Initialize(in ReadEventData eventData, #_TYPE_PREFIX_#GameDataContainer dataContainer,
                                        in ReadDictionary<int, string> eventMap)
        {
            base.Initialize(eventData, dataContainer, eventMap);

            this.gameObject.AddComponent<#_TYPE_PREFIX_#DataCommandRegisterer>();
        }
    }
}
";
    }
}