namespace HegaCore.AutoGen.Templates
{
    internal static class TableTemplate
    {
        public const string Template = @"using HegaCore;

namespace #_NAMESPACE_#
{
    public sealed partial class #_TYPE_PREFIX_#Tables : #_BASE_TYPE_#
    {
    }
}
";
    }
}